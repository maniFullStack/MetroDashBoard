using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Configuration;

namespace WebsiteUtilities {
    public static class ErrorHandler {
        private static bool _hasBeenChecked = false;
        private static bool _logToEventLog = false;
        private static bool _logToDatabase = false;
        private static bool _logToFile = false;

        /// <summary>
        /// Error event IDs. Used to categorize errors.
        /// </summary>
        public enum ErrorEventID {
            /// <summary>
            /// Default. Don't use this one if it can be avoided.
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// General errors.
            /// </summary>
            General = 1,
            /// <summary>
            /// Error in SQL queries.
            /// </summary>
            SQLError = 4,
            /// <summary>
            /// Error in LogHandler. Do not use this.
            /// </summary>
            LogError = 5,
            /// <summary>
            /// Error in LogHandler. Do not use this.
            /// </summary>
            LogErrorSQL = 6,
            /// <summary>
            /// Error in LogHandler. Do not use this.
            /// </summary>
            LogErrorEvent = 7,
            /// <summary>
            /// Error in LogHandler. Do not use this.
            /// </summary>
            LogErrorFile = 8,
            /// <summary>
            /// Connection error.
            /// </summary>
            ConnectionError = 9
        }

        /// <summary>
        /// Writes log information to the Application, database and flat file logs (if enabled).
        /// </summary>
        /// <param name="appSource">The source of the error. "Website-" will be automatically prepended to this. Max 40 chars.</param>
        public static int WriteLog(string appSource, string message, ErrorEventID eventID) {
            return WriteLog(appSource, message, eventID, null, true);
        }

        /// <summary>
        /// Writes log information to the Application, database and flat file logs (if enabled). Includes information about the passed exception automatically.
        /// </summary>
        /// /// <param name="appSource">The source of the error. "Website-" will be automatically prepended to this. Max 40 chars.</param>
        public static int WriteLog(string appSource, string message, ErrorEventID eventID, Exception excInfo) {
            return WriteLog(appSource, message, eventID, excInfo, true);
        }

        /// <summary>
        /// Private function that allows for passing whether or not the errors should recurse if the log has an error when writing.
        /// </summary>
        private static int WriteLog(string appSource, string message, ErrorEventID eventID, Exception causeExc, bool recurse) {
            string src;
            string msg;
            int eventtype;
            string callingfunction = "";

            int errorID = -1;
            appSource = appSource != null ? appSource : "BadSource";
            message = message != null ? message : "<Bad Message>";
            try {
                src = "Website-" + (appSource.Length > 40 ? appSource.Substring(0, 40) : appSource);
                msg = message;
                eventtype = (int) eventID;
            } catch {
                src = "Website-" + appSource;
                msg = message.ToString();
                eventtype = -1;
            }

            //Get the function that called this one
            try {
                StackTrace callStack = new StackTrace();
                for (int i = 0; i < callStack.FrameCount; i++) {
                    MethodBase mb = callStack.GetFrame(i).GetMethod();
                    callingfunction = mb.Name + "->" + callingfunction;
                }
            } catch { callingfunction = "<Unknown>"; }

            //Get the page URL
            string pageURL;
            try {
                pageURL = (HttpContext.Current != null && HttpContext.Current.Request != null) ? HttpContext.Current.Request.Url.ToString() : "<No Context or Request>";
            } catch { pageURL = "<No Context or Request>"; }
            //Get the server name
            string serverName;
            try {
                serverName = System.Net.Dns.GetHostName();
            } catch { serverName = "Unknown-Server"; }

            //Check if we should be logging errors to specific places based on the current configuration
            

            // Write error log to (Application) EVENT LOG
            if (eventID != ErrorEventID.LogErrorEvent && LogToEventLog) {
                try {
                    if (!EventLog.SourceExists(src)) {
                        // Create 
                        EventLog.CreateEventSource(src, "Application");
                    }
                    if (causeExc != null) {
                        EventLog.WriteEntry(src, msg + "\nServer: " + serverName + "\nPage URL: " + pageURL + "\nCalling Function: " + callingfunction + "\n" + GenerateExceptionInfoString(causeExc, false) + (causeExc.InnerException != null ? "\n---Inner Exception---\n" + GenerateExceptionInfoString(causeExc.InnerException, false) + "\n---End Inner Exception---" : ""), EventLogEntryType.Error, eventtype);
                    } else {
                        EventLog.WriteEntry(src, msg, EventLogEntryType.Error, eventtype);
                    }
                } catch (Exception ex) {
                    if (recurse) {
                        WriteLog("ErrorLog-Event", "There was an error writing to the event log.", ErrorEventID.LogError, ex, false);
                    }
                }
            }

            // Write error log to DATABASE
            if (eventID != ErrorEventID.ConnectionError && eventID != ErrorEventID.LogErrorSQL && LogToDatabase) {
                try {
                    string logTableName;
                    if (Array.IndexOf<string>(WebConfigurationManager.OpenWebConfiguration("/").AppSettings.Settings.AllKeys, "LogTableName") > -1) {
                        logTableName = WebConfigurationManager.OpenWebConfiguration("/").AppSettings.Settings["LogTableName"].Value;
                    } else {
                        logTableName = "tblSYSAppErrorLogs";
                    }

                    SQLDatabase sql = new SQLDatabase();
                    string ms = msg;
                    if (causeExc != null) {
                        ms += "\n" + GenerateExceptionInfoString(causeExc, false) + (causeExc.InnerException != null ? "\n---Inner Exception---\n" + GenerateExceptionInfoString(causeExc.InnerException, false) + "\n---End Inner Exception---" : "");
                        ms = (ms.Length > 1500 ? ms.Substring(0, 1500) : ms);
                    } else {
                        ms = (ms.Length > 1500 ? ms.Substring(0, 1500) : ms);
                    }
                    ms = "Calling Function: " + callingfunction + "\n" + ms;
                    sql.LogErrors = false;
                    string sqlquery = "INSERT INTO [" + logTableName + "] (ApplicationSource,EventSource,Message,EventDate,Server,PageURL) " +
                                " VALUES (@AppSrc, @EvtSrc, @Msg, GetDate(), @SvrName, @Page)";
                    //sql.NonQueryWithParams(sqlquery, new SQLDatabase.SQLParamList().Add("@AppSrc", src).Add("@EvtSrc", eventtype).Add("@Msg", ms).Add("@SvrName", serverName).Add("@Page", pageURL));
                    errorID = sql.QueryAndReturnIdentity(sqlquery, new SQLParamList().Add("@AppSrc", src).Add("@EvtSrc", eventtype).Add("@Msg", ms).Add("@SvrName", serverName).Add("@Page", pageURL));
                    if (sql.HasError) {
                        WriteLog("ErrorLog-SQLLog", "There was an error writing a log to the SQL server database. SQL Query: " + sqlquery, ErrorEventID.LogErrorSQL, sql.ExceptionList[0], false);
                    }
                } catch (Exception ex) {
                    if (recurse) {
                        WriteLog("ErrorLog-SQL", "There was an error writing to the SQL server database.", ErrorEventID.LogErrorSQL, ex, false);
                    }
                }
            }

            //Write to error log to FILE
            if (eventID != ErrorEventID.LogErrorFile && LogToFile) {
                try {
                    string path;

                    if (Array.IndexOf<string>(WebConfigurationManager.OpenWebConfiguration("/").AppSettings.Settings.AllKeys, "LogFileDirectory") > -1) {
                        path = WebConfigurationManager.OpenWebConfiguration("/").AppSettings.Settings["LogFileDirectory"].Value;
                    } else {
                        path = "~/Files/Logs/";
                    }
                    path = HttpContext.Current.Server.MapPath(path);
                    
                    if (Directory.Exists(path)) {
                        TextWriter tw = new StreamWriter(path + "ErrorLog-" + serverName + "-" + DateTime.Now.ToString("yyyyMMdd") + ".log", true);
                        tw.WriteLine("-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-");
                        tw.WriteLine("Date: " + DateTime.Now.ToLongDateString());
                        tw.WriteLine("Time: " + DateTime.Now.ToLongTimeString());
                        tw.WriteLine("Server: " + serverName);
                        tw.WriteLine("Page URL: " + pageURL);
                        tw.WriteLine("Calling Function: " + callingfunction);
                        tw.WriteLine("SMIOS Error Event ID: " + eventtype.ToString());
                        tw.WriteLine("Error Source: " + src);
                        tw.WriteLine("Message: " + msg);
                        if (causeExc != null) {
                            tw.WriteLine("Exception:\n" + GenerateExceptionInfoString(causeExc, false));
                            if (causeExc.InnerException != null) {
                                tw.WriteLine("Inner Exception:");
                                tw.WriteLine(Conversion.XMLEncodeString(GenerateExceptionInfoString(causeExc.InnerException, false)));
                            }
                        }
                        tw.WriteLine();
                        tw.Close();

                        tw = new StreamWriter(path + "ErrorLog-" + serverName + "-" + DateTime.Now.ToString("yyyyMMdd") + ".xml", true);
                        tw.WriteLine("<error>");
                        tw.WriteLine("\t<date>" + Conversion.DateTimeToXMLString(DateTime.Now) + "</date>");
                        tw.WriteLine("\t<server>" + Conversion.XMLEncodeString(serverName) + "</server>");
                        tw.WriteLine("\t<pageurl>" + Conversion.XMLEncodeString(pageURL) + "</pageurl>");
                        tw.WriteLine("\t<callfunction>" + Conversion.XMLEncodeString(callingfunction) + "</callfunction>");
                        tw.WriteLine("\t<smioseventid>" + Conversion.XMLEncodeString(eventtype.ToString()) + "</smioseventid>");
                        tw.WriteLine("\t<apperrorsource>" + Conversion.XMLEncodeString(src) + "</apperrorsource>");
                        tw.WriteLine("\t<message>" + Conversion.XMLEncodeString(msg) + "</message>");
                        if (causeExc != null) {
                            tw.WriteLine(GenerateExceptionInfoString(causeExc, true));
                            if (causeExc.InnerException != null) {
                                tw.WriteLine("\t<innerexception>");
                                tw.WriteLine(Conversion.XMLEncodeString(GenerateExceptionInfoString(causeExc.InnerException, true)));
                                tw.WriteLine("\t</innerexception>");
                            }
                        }
                        tw.WriteLine("</error>");
                        tw.Close();
                    }
                } catch (Exception ex) {
                    if (recurse) {
                        WriteLog("ErrorLog-File", "There was an error writing to the log file.", ErrorEventID.LogError, ex, false);
                    }
                }
            }
            return errorID;
        }

        /// <summary>
        /// Generates a string containing information about the exceptions message, source and a stack trace.
        /// </summary>
        /// <param name="ex">The exception to generate the info string from.</param>
        private static string GenerateExceptionInfoString(Exception ex, bool asXML) {
            if (ex == null) { return ""; }
            if (asXML) {
                string xmlstring = "<exceptioninfo>";
                xmlstring += "\n\t<message>" + Conversion.XMLEncodeString(ex.Message) + "</message>";
                xmlstring += "\n\t<source>" + Conversion.XMLEncodeString(ex.Source) + "</source>";
                xmlstring += "\n\t<stacktrace>" + Conversion.XMLEncodeString(ex.StackTrace) + "</stacktrace>";
                xmlstring += "\n</exceptioninfo>";
                return xmlstring;
            } else {
                return "-- Exception Information --\nMessage: " + ex.Message + "\nSource: " + ex.Source + "\nStack Trace: " + ex.StackTrace + "\n-- End Exception Information --";
            }
        }

        /// <summary>
        /// Returns a string value for the name of the event type.
        /// </summary>
        /// <param name="evt">The event ID.</param>
        public static string ErrorName(ErrorEventID evt) {
            return ErrorName((int) evt);
        }

        /// <summary>
        /// Returns a string value for the name of the event type.
        /// </summary>
        /// <param name="evt">The event ID.</param>
        public static string ErrorName(int evt) {
            switch (evt) {
                case (int) ErrorEventID.Unknown:
                    return "Unknown";
                case (int)ErrorEventID.SQLError:
                    return "SQL Error";
                case (int)ErrorEventID.LogError:
                    return "Log Error (General)";
                case (int)ErrorEventID.LogErrorSQL:
                    return "Log Error (Adding SQL Log)";
                case (int)ErrorEventID.LogErrorEvent:
                    return "Log Error (Adding Event Log)";
                case (int)ErrorEventID.LogErrorFile:
                    return "Log Error (Adding File Log)";
                case (int)ErrorEventID.ConnectionError:
                    return "Connection Error";                
                default:
                    return "";
            }
        }

        /// <summary>
        /// Whether or not to log to the Event Log
        /// </summary>
        private static bool LogToEventLog {
            get {
                if (_hasBeenChecked) {
                    return _logToEventLog;
                } else {
                    CheckEnabledLogs();
                    return _logToEventLog;
                }
            }
        }

        /// <summary>
        /// Whether or not to log to the Database
        /// </summary>
        private static bool LogToDatabase {
            get {
                if (_hasBeenChecked) {
                    return _logToDatabase;
                } else {
                    CheckEnabledLogs();
                    return _logToDatabase;
                }
            }
        }

        /// <summary>
        /// Whether or not to log to the File
        /// </summary>
        private static bool LogToFile {
            get {
                if (_hasBeenChecked) {
                    return _logToFile;
                } else {
                    CheckEnabledLogs();
                    return _logToFile;
                }
            }
        }

        /// <summary>
        /// Checks for what logs are enabled in Web.Config
        /// </summary>
        private static void CheckEnabledLogs() {
            try {
                _logToFile = Conversion.StringToBool(WebConfigurationManager.OpenWebConfiguration("/").AppSettings.Settings["LogToFile"].Value);
            } catch { _logToFile = false; }
            
            try {
                _logToDatabase = Conversion.StringToBool(WebConfigurationManager.OpenWebConfiguration("/").AppSettings.Settings["LogToDatabase"].Value);
            } catch { _logToDatabase = false; }

            try {
                _logToEventLog = Conversion.StringToBool(WebConfigurationManager.OpenWebConfiguration("/").AppSettings.Settings["LogToEventLog"].Value);
            } catch { _logToEventLog = false; }
        }
    }
}