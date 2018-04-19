using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Configuration;

namespace WebsiteUtilities {
    /// <summary>
    /// SQLDatabase object that uses the "DatabaseSMI" connection string from web.config.
    /// </summary>
    public class SQLDatabaseSMI : SQLDatabase {
        /// <summary>
        /// Gets the connection string with the name "DatabaseSMI"
        /// </summary>
        public override string GetConnectionString() {
            return WebConfigurationManager.OpenWebConfiguration("/").ConnectionStrings.ConnectionStrings["DatabaseSMI"].ConnectionString;
        }
    }

    /// <summary>
    /// SQLDatabase object that uses the "DatabaseWeb" connection string from web.config.
    /// </summary>
    public class SQLDatabaseWeb : SQLDatabase {
        /// <summary>
        /// Gets the connection string with the name "DatabaseWeb"
        /// </summary>
        public override string GetConnectionString() {
            return WebConfigurationManager.OpenWebConfiguration("/").ConnectionStrings.ConnectionStrings["DatabaseWeb"].ConnectionString;
        }
    }

    /// <summary>
    /// SQLDatabase object that uses the "DatabaseDebug" connection string from web.config.
    /// </summary>
    public class SQLDatabaseDebug : SQLDatabase {
        /// <summary>
        /// Gets the connection string with the name "DatabaseDebug"
        /// </summary>
        public override string GetConnectionString() {
            return WebConfigurationManager.OpenWebConfiguration("/").ConnectionStrings.ConnectionStrings["DatabaseDebug"].ConnectionString;
        }
    }

    /// <summary>
    /// SQLDatabase object that uses the "DatabaseDebug" connection string from web.config.
    /// </summary>
    public class SQLDatabaseReporting : SQLDatabase {
        /// <summary>
        /// Gets the connection string with the name "DatabaseReporting"
        /// </summary>
        public override string GetConnectionString() {
            return WebConfigurationManager.OpenWebConfiguration("/").ConnectionStrings.ConnectionStrings["DatabaseReporting"].ConnectionString;
        }
    }

    /// <summary>
    /// Used to make queries on the SQL database. The connection will use the "DatabaseDefault" connection string from web.config.
    /// </summary>
    public class SQLDatabase {
        private bool _logErrors = true;
        private SqlConnection _scConnection = null;
        private List<Exception> _exceptionList = null;
        private List<SqlError> _messageList = null;
        /// <summary>
        /// Initializes the SQL object and sets up the connection parameters.
        /// </summary>
        public SQLDatabase() {
            _exceptionList = new List<Exception>();
            _messageList = new List<SqlError>();
            try {
                _scConnection = new SqlConnection(GetConnectionString());
                _scConnection.InfoMessage += new SqlInfoMessageEventHandler(SQLInfoMessageHandler);
            } catch (Exception ex) {
                if (_logErrors) {
                    ErrorHandler.WriteLog("WebsiteUtilities.SQLDatabase", "Could not create SQL connection.", ErrorHandler.ErrorEventID.ConnectionError, ex);
                }
                _exceptionList.Add(ex);
                _scConnection = null;
            }
        }




        // Summary:
        //     The timeout for the SqlCommand in seconds.
        public int CommandTimeout { get; set; }




        /// <summary>
        /// Returns the connection string.
        /// </summary>
        public virtual string GetConnectionString() {
            return WebConfigurationManager.OpenWebConfiguration("/").ConnectionStrings.ConnectionStrings["DatabaseDefault"].ConnectionString;
        }

        /// <summary>
        /// Logs SQL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        public void SQLInfoMessageHandler(object sender, SqlInfoMessageEventArgs ea) {
            ErrorHandler.WriteLog("WebsiteUtilities.SQLDatabase.SQLInfoMessageHandler", ea.Errors[0].Message.ToString(), ErrorHandler.ErrorEventID.SQLError);
            foreach (SqlError err in ea.Errors) {
                // AddRange doesn't work on SqlErrorCollection :'(
                _messageList.Add(err);
            }
        }

        /// <summary>
        /// Executes a non-query with parameters on the database connection specified in the constructor.
        /// </summary>
        /// <param name="querytext">The text of the query.</param>
        /// <param name="sqlParameters">A (params) array of SqlParameters to pass into the SqlCommand object.</param>
        /// <returns>The number of rows affected or -1 on error.</returns>
        public int NonQuery(string querytext, params SqlParameter[] sqlParameters) {
            _exceptionList.Clear();
            _messageList.Clear();
            int numRows;
            SqlCommand com = null;
            try {
                _scConnection.Open();
                com = new SqlCommand(querytext, _scConnection);
                com.Parameters.AddRange(sqlParameters);
                numRows = com.ExecuteNonQuery();
                com.Parameters.Clear();//Clear the params array so they can be reused
            } catch (Exception ex) {
                _exceptionList.Add(ex);
                if (_logErrors) {
                    StringBuilder paramlist = new StringBuilder();
                    foreach (SqlParameter prm in sqlParameters) {
                        paramlist.Append(prm.ParameterName)
                                 .Append("='")
                                 .Append(prm.Value)
                                 .Append("'\n");
                    }
                    ErrorHandler.WriteLog("WebsiteUtilities.SQLDatabase.NonQuery", "There was an error executing the non query (with params).\nQuery Text: " + querytext + "\nParameter List:\n" + paramlist.ToString(), ErrorHandler.ErrorEventID.SQLError, ex);
                }
                numRows = -1;
            } finally {
                _scConnection.Close();
                com.Parameters.Clear(); //Added Sept 15, 2011 by Andrew so I could use the same paremter set twice and garbage collection couldn't keep up with me!
            }
            return numRows;
        }

        /// <summary>
        /// Executes a non-query with parameters on the database connection specified in the constructor.
        /// </summary>
        /// <param name="querytext">The text of the query.</param>
        /// <param name="paramlist">An SQLParamList object containing SqlParameters to pass into the SqlCommand object.</param>
        /// <returns>The number of rows affected or -1 on error.</returns>
        public int NonQuery(string querytext, SQLParamList paramlist) {
            return NonQuery(querytext, paramlist.ToArray());
        }

        /// <summary>
        /// Executes a stored procedure.
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure. Do not prepend "EXEC".</param>
        /// <param name="sqlParameters">A params array of SqlParameters to pass into the SqlCommand object.</param>
        /// <returns>A DataTable representing the returned dataset. If more than one dataset is returned from the query, the last one will be returned from this method.</returns>
        public DataTable ExecStoredProcedureDataTable(string procedureName, params SqlParameter[] sqlParameters) {
            _exceptionList.Clear();
            _messageList.Clear();
            DataTable rtrn;
            try {
                _scConnection.Open();
                SqlDataAdapter sa = new SqlDataAdapter(procedureName, _scConnection);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sa.SelectCommand.Parameters.AddRange(sqlParameters);
                DataSet dsData = new DataSet();
                sa.Fill(dsData);
                if (dsData.Tables.Count > 0) {
                    rtrn = dsData.Tables[dsData.Tables.Count - 1];
                } else {
                    rtrn = new DataTable();
                }
                sa.SelectCommand.Parameters.Clear();//Clear the params array so they can be reused
            } catch (Exception ex) {
                _exceptionList.Add(ex);
                if (_logErrors) {
                    StringBuilder paramlist = new StringBuilder();
                    foreach (SqlParameter prm in sqlParameters) {
                        paramlist.Append(prm.ParameterName)
                                 .Append("='")
                                 .Append(prm.Value)
                                 .Append("'\n");
                    }
                    ErrorHandler.WriteLog("WebsiteUtilities.SQLDatabase.ExecStoredProcedureDataTable", "There was an error executing the stored procedure (data table).\nProcedure Name: " + procedureName + "\nParameter List:\n" + paramlist.ToString(), ErrorHandler.ErrorEventID.SQLError, ex);
                }
                rtrn = new DataTable();
            } finally {
                _scConnection.Close();
            }
            return rtrn;
        }

        /// <summary>
        /// Executes a stored procedure.
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure. Do not prepend "EXEC".</param>
        /// <param name="paramlist">An SQLParamList object containing SqlParameters to pass into the SqlCommand object.</param>
        /// <returns>A DataTable representing the returned dataset. If more than one dataset is returned from the query, the last one will be returned from this method.</returns>
        public DataTable ExecStoredProcedureDataTable(string procedureName, SQLParamList paramlist) {
            return ExecStoredProcedureDataTable(procedureName, paramlist.ToArray());
        }

        /// <summary>
        /// Executes a stored procedure.
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure. Do not prepend "EXEC".</param>
        /// <param name="sqlParameters">A params array of SqlParameters to pass into the SqlCommand object.</param>
        /// <returns>A DataSet representing the returned dataset(s).</returns>
        public DataSet ExecStoredProcedureDataSet(string procedureName, params SqlParameter[] sqlParameters) {
            _exceptionList.Clear();
            _messageList.Clear();
            DataSet rtrn;
            try {
                _scConnection.Open();
                SqlDataAdapter sa = new SqlDataAdapter(procedureName, _scConnection);
                sa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sa.SelectCommand.Parameters.AddRange(sqlParameters);
                rtrn = new DataSet();
                sa.Fill(rtrn);
                sa.SelectCommand.Parameters.Clear();//Clear the params array so they can be reused
            } catch (Exception ex) {
                _exceptionList.Add(ex);
                if (_logErrors) {
                    StringBuilder paramlist = new StringBuilder();
                    foreach (SqlParameter prm in sqlParameters) {
                        paramlist.Append(prm.ParameterName)
                                 .Append("='")
                                 .Append(prm.Value)
                                 .Append("'\n");
                    }
                    ErrorHandler.WriteLog("WebsiteUtilities.SQLDatabase.ExecStoredProcedureDataSet", "There was an error executing the stored procedure (data set).\nProcedure Name: " + procedureName + "\nParameter List:\n" + paramlist.ToString(), ErrorHandler.ErrorEventID.SQLError, ex);
                }
                rtrn = new DataSet();
            } finally {
                _scConnection.Close();
            }
            return rtrn;
        }

        /// <summary>
        /// Executes a stored procedure.
        /// </summary>
        /// <param name="procedureName">The name of the stored procedure. Do not prepend "EXEC".</param>
        /// <param name="paramlist">An SQLParamList object containing SqlParameters to pass into the SqlCommand object.</param>
        /// <returns>A DataSet representing the returned dataset(s).</returns>
        public DataSet ExecStoredProcedureDataSet(string procedureName, SQLParamList paramlist) {
            return ExecStoredProcedureDataSet(procedureName, paramlist.ToArray());
        }

        /// <summary>
        /// Executes a query on the database with parameters.
        /// </summary>        
        /// <param name="querytext">The text of the query.</param>
        /// <param name="sqlParameters">A (params) array of SqlParameters to pass into the SqlCommand object.</param>
        /// <returns>An object of type System.Data.DataSet containing the results of the queried data.</returns>
        public DataSet QueryDataSet(string querytext, params SqlParameter[] sqlParameters) {
            _exceptionList.Clear();
            _messageList.Clear();
            DataSet rtrn;
            try {
                _scConnection.Open();
                SqlDataAdapter sa = new SqlDataAdapter(querytext, _scConnection);
                //try {
                sa.SelectCommand.Parameters.AddRange(sqlParameters);
                //} catch { }
                //try {
                //    sa.UpdateCommand.Parameters.AddRange(sqlParameters);
                //} catch { }
                //try {
                //    sa.DeleteCommand.Parameters.AddRange(sqlParameters);
                //} catch { }
                //try {
                //    sa.InsertCommand.Parameters.AddRange(sqlParameters);
                //} catch { }
                rtrn = new DataSet();
                sa.Fill(rtrn);
                sa.SelectCommand.Parameters.Clear();//Clear the params array so they can be reused
            } catch (Exception ex) {
                _exceptionList.Add(ex);
                if (_logErrors) {
                    StringBuilder paramlist = new StringBuilder();
                    foreach (SqlParameter prm in sqlParameters) {
                        paramlist.Append(prm.ParameterName)
                                 .Append("='")
                                 .Append(prm.Value)
                                 .Append("'\n");
                    }
                    ErrorHandler.WriteLog("WebsiteUtilities.SQLDatabase.QueryDataSet", "There was an error executing the query (data set).\nQuery Text: " + querytext + "\nParameter List:\n" + paramlist.ToString(), ErrorHandler.ErrorEventID.SQLError, ex);
                }
                rtrn = new DataSet();
            } finally {
                _scConnection.Close();
            }
            return rtrn;
        }

        /// <summary>
        /// Executes a query on the database with parameters.
        /// </summary>        
        /// <param name="querytext">The text of the query.</param>
        /// <param name="paramlist">An SQLParamList object containing SqlParameters to pass into the SqlCommand object.</param>
        /// <returns>An object of type System.Data.DataSet containing the results of the queried data.</returns>
        public DataSet QueryDataSet(string querytext, SQLParamList paramlist) {
            return QueryDataSet(querytext, paramlist.ToArray());
        }

        /// <summary>
        /// Executes a query on the database with parameters.
        /// </summary>        
        /// <param name="querytext">The text of the query.</param>
        /// <param name="paramlist">An SQLParamList object containing SqlParameters to pass into the SqlCommand object.</param>
        /// <returns>An object of type System.Data.DataTable containing the results of the queried data.</returns>
        public DataTable QueryDataTable(string querytext, params SqlParameter[] sqlParameters) {
            _exceptionList.Clear();
            _messageList.Clear();
            DataTable rtrn;
            try {
                _scConnection.Open();
                SqlDataAdapter sa = new SqlDataAdapter(querytext, _scConnection);
                //try {
                sa.SelectCommand.Parameters.AddRange(sqlParameters);
                //} catch { }
                //try {
                //    sa.UpdateCommand.Parameters.AddRange(sqlParameters);
                //} catch { }
                //try {
                //    sa.DeleteCommand.Parameters.AddRange(sqlParameters);
                //} catch { }
                //try {
                //    sa.InsertCommand.Parameters.AddRange(sqlParameters);
                //} catch { }

                DataSet dsData = new DataSet();
                sa.Fill(dsData);
                if (dsData.Tables.Count > 0) {
                    rtrn = dsData.Tables[dsData.Tables.Count - 1];
                } else {
                    rtrn = new DataTable();
                }
                sa.SelectCommand.Parameters.Clear();//Clear the params array so they can be reused
            } catch (Exception ex) {
                _exceptionList.Add(ex);
                if (_logErrors) {
                    StringBuilder paramlist = new StringBuilder();
                    foreach (SqlParameter prm in sqlParameters) {
                        paramlist.Append(prm.ParameterName)
                                 .Append("='")
                                 .Append(prm.Value)
                                 .Append("'\n");
                    }
                    ErrorHandler.WriteLog("WebsiteUtilities.SQLDatabase.QueryDataTable", "There was an error executing the query (data table).\nQuery Text: " + querytext + "\nParameter List:\n" + paramlist.ToString(), ErrorHandler.ErrorEventID.SQLError, ex);
                }
                rtrn = new DataTable();
            } finally {
                _scConnection.Close();
            }
            return rtrn;
        }

        /// <summary>
        /// Executes a query on the database with parameters.
        /// </summary>        
        /// <param name="querytext">The text of the query.</param>
        /// <param name="paramlist">A SQLParamList object containing SqlParameters to pass into the SqlCommand object.</param>
        /// <returns>An object of type System.Data.DataTable containing the results of the queried data.</returns>
        public DataTable QueryDataTable(string querytext, SQLParamList paramlist) {
            return QueryDataTable(querytext, paramlist.ToArray());
        }

        /// <summary>
        /// Will execute an insert query and return the identity value for the inserted row. Used to insert a row and retrieve the auto-generated identity value at the same time. On error, it returns the default value of T.
        /// </summary>
        /// <typeparam name="T">The type of the identity.</typeparam>
        /// <param name="querytext">An insert query.</param>
        /// <param name="sqlParameters">A (params) array of SqlParameters to pass into the SqlCommand object.</param>
        /// <returns>The identity value for the inserted row.</returns>
        public int QueryAndReturnIdentity(string querytext, params SqlParameter[] sqlParameters) {
            _exceptionList.Clear();
            _messageList.Clear();
            int rtrn;
            try {
                _scConnection.Open();
                querytext += (!querytext.Trim().EndsWith(";") ? ";" : "") + " SELECT CAST(SCOPE_IDENTITY() as int);"; //Append the identity seed retrieval query
                SqlCommand com = new SqlCommand(querytext, _scConnection); //Create the command
                com.Parameters.AddRange(sqlParameters);
                int scalarval = (int)com.ExecuteScalar(); //Execute the command
                rtrn = scalarval; //If it worked, set the return val to it
                com.Parameters.Clear();//Clear the params array so they can be reused
            } catch (Exception ex) {
                _exceptionList.Add(ex);
                if (_logErrors) {

                    StringBuilder paramlist = new StringBuilder();
                    foreach (SqlParameter prm in sqlParameters) {
                        paramlist.Append(prm.ParameterName)
                                 .Append("='")
                                 .Append(prm.Value)
                                 .Append("'\n");
                    }
                    //ErrorHandler.WriteLog("SQLDatabase.QueryAndReturnIdentity", "There was an error executing the query (with params).\nQuery Text: " + querytext + "\nParameter List:\n" + paramlist.ToString(), ErrorHandler.ErrorEventID.SQLError, ex);
                }
                rtrn = -1;
            } finally {
                _scConnection.Close();
            }
            return rtrn;
        }

        /// <summary>
        /// Will execute an insert query and return the identity value for the inserted row. Used to insert a row and retrieve the auto-generated identity value at the same time. On error, it returns the default value of T.
        /// </summary>
        /// <typeparam name="T">The type of the identity.</typeparam>
        /// <param name="querytext">An insert query.</param>
        /// <param name="paramlist">A SQLParamList object containing SqlParameters to pass into the SqlCommand object.</param>
        /// <returns>The identity value for the inserted row.</returns>
        public int QueryAndReturnIdentity(string querytext, SQLParamList paramlist) {
            return QueryAndReturnIdentity(querytext, paramlist.ToArray());
        }

        /// <summary>
        /// Runs a series of queries in a transaction and rolls back the transaction.
        /// </summary>
        /// <param name="queries">A string array of queries to be run in the transaction.</param>
        /// <param name="returnvalues">The return values (rows affected) for each query.</param>
        /// <returns></returns>
        public bool Transaction(string[] queries, out int[] returnvalues) {
            _exceptionList.Clear();
            _messageList.Clear();
            SqlCommand com = _scConnection.CreateCommand();
            SqlTransaction trans = null;
            bool success = false;
            int cnt = 0;//Counter for current query
            try {
                //Open the connection
                _scConnection.Open();

                //Begin the transaction
                trans = _scConnection.BeginTransaction();

                //Assign Transaction to Command
                com.Transaction = trans;

                //Set up a list to hold the return values
                List<int> returnvals = new List<int>();

                //Loop through the queries and execute them
                foreach (string querytext in queries) {
                    cnt++;
                    com.CommandText = querytext;
                    returnvals.Add(com.ExecuteNonQuery());
                }

                //Add the return values to the array
                returnvalues = returnvals.ToArray();

                //Commit the transaction
                trans.Commit();

                //The transaction went through, everything is good
                success = true;
            } catch (Exception ex) {
                //Something went wrong, rollback the transaction
                trans.Rollback();
                //Add the exception information to the log
                _exceptionList.Add(ex);
                if (_logErrors) {
                    string querytext = "";
                    int i = 0;
                    //Loop through the query texts and add them to the string
                    foreach (string qtxt in queries) {
                        querytext += ++i + ". " + qtxt + "\n";
                    }
                    //ErrorHandler.WriteLog("SQLDatabase.Transaction", "There was an error executing a query (" + cnt + ") in the transaction.\nQuery Text:\n" + querytext, ErrorHandler.ErrorEventID.SQLError, ex);
                }
                //Set the return values array to null since there are no return values
                returnvalues = null;
            } finally {
                //Close the connection
                _scConnection.Close();
            }
            //Return whether or not it worked
            return success;
        }

        /// <summary>
        /// Executes a scalar query with parameters and returns the value as type T.
        /// </summary>
        /// <typeparam name="T">The type of the value being returned.</typeparam>
        /// <param name="querytext">The query to run.</param>
        /// <param name="sqlParameters">A (params) array of SqlParameters to pass into the SqlCommand object.</param>
        /// <returns>The scalar value of type T returned by the query.</returns>
        public T QueryScalarValue<T>(string querytext, params SqlParameter[] sqlParameters) {
            return QueryScalarValue<T>(querytext, default(T), sqlParameters);
        }
        /// <summary>
        /// Executes a scalar query with parameters and returns the value as type T.
        /// </summary>
        /// <typeparam name="T">The type of the value being returned.</typeparam>
        /// <param name="querytext">The query to run.</param>
        /// <param name="paramlist">An SQLParamList object containing SqlParameters to pass into the SqlCommand object.</param>
        /// <returns>The scalar value of type T returned by the query.</returns>
        public T QueryScalarValue<T>(string querytext, SQLParamList paramlist) {
            return QueryScalarValue<T>(querytext, default(T), paramlist.ToArray());
        }

        /// <summary>
        /// Executes a scalar query with parameters and returns the value as type T or the defaultreturn value if there is an error in conversion.
        /// </summary>
        /// <typeparam name="T">The type of the value being returned.</typeparam>
        /// <param name="querytext">The query to run.</param>
        /// <param name="defaultreturn">The default value to return if there is an error converting the value.</param>
        /// <param name="sqlParameters">A (params) array of SqlParameters to pass into the SqlCommand object.</param>
        /// <returns>The scalar value of type T returned by the query.</returns>
        public T QueryScalarValue<T>(string querytext, T defaultreturn, params SqlParameter[] sqlParameters) {
            _exceptionList.Clear();
            _messageList.Clear();
            T rtrn = defaultreturn;
            try {
                _scConnection.Open();
                SqlCommand com = new SqlCommand(querytext, _scConnection); //Create the command
                com.Parameters.AddRange(sqlParameters); //Add the parameters
                T scalarval = (T)com.ExecuteScalar(); //Execute the command
                rtrn = scalarval; //If it worked, set the return val to it
                com.Parameters.Clear();//Clear the params array so they can be reused
            } catch (Exception ex) {
                _exceptionList.Add(ex);
                if (_logErrors) {
                    StringBuilder paramlist = new StringBuilder();
                    foreach (SqlParameter prm in sqlParameters) {
                        paramlist.Append(prm.ParameterName)
                                 .Append("='")
                                 .Append(prm.Value)
                                 .Append("'\n");
                    }
                    //ErrorHandler.WriteLog("SQLDatabase.QueryScalarValue", "There was an error executing the query (scalar value).\nQuery Text: " + querytext + "\nParameter List:\n" + paramlist.ToString(), ErrorHandler.ErrorEventID.SQLError, ex);
                }
                rtrn = defaultreturn;
            } finally {
                _scConnection.Close();
            }
            return rtrn;
        }

        /// <summary>
        /// Executes a scalar query with parameters and returns the value as type T or the defaultreturn value if there is an error in conversion.
        /// </summary>
        /// <typeparam name="T">The type of the value being returned.</typeparam>
        /// <param name="querytext">The query to run.</param>
        /// <param name="defaultreturn">The default value to return if there is an error converting the value.</param>
        /// <param name="paramlist">An SQLParamList object containing SqlParameters to pass into the SqlCommand object.</param>
        /// <returns>The scalar value of type T returned by the query.</returns>
        public T QueryScalarValue<T>(string querytext, T defaultreturn, SQLParamList paramlist) {
            return QueryScalarValue<T>(querytext, defaultreturn, paramlist.ToArray());
        }

        /// <summary>
        /// Used to escape special charecters in a LIKE query, assumes the "ESCAPE '\'" clause is set. Note: Automatically calls SQLDatabase.RemoveSQLInject().
        /// </summary>        
        /// <param name="data">
        /// The string data to be escaped
        /// </param>
        /// <returns>
        /// The escaped SQL string.
        /// </returns>
        public static string EscapeLIKE(string data) {
            return data.Replace("\\", "\\\\").Replace("_", "\\_").Replace("[", "\\[").Replace("%", "\\%");
        }

        /// <summary>
        /// The list of current exceptions after the last query.
        /// </summary>
        public List<Exception> ExceptionList {
            get {
                List<Exception> lst = new List<Exception>(_exceptionList);
                return lst;
            }
        }

        /// <summary>
        /// Whether or not the object has any exceptions from the previous query.
        /// </summary>
        public bool HasError {
            get {
                return _exceptionList.Count > 0;
            }
        }

        /// <summary>
        /// The list of current exceptions after the last query.
        /// </summary>
        public List<SqlError> MessageList {
            get {
                List<SqlError> lst = new List<SqlError>(_messageList);
                return lst;
            }
        }

        /// <summary>
        /// Whether or not any sql info messages were returned from the query.
        /// </summary>
        public bool HasMessages {
            get {
                return _messageList.Count > 0;
            }
        }

        /// <summary>
        /// Whether or not any sql info messages were returned with a severity of CustomMessageSeverity from the query.
        /// </summary>
        public bool HasCustomMessage {
            get {
                if (_messageList.Count > 0) {
                    foreach (SqlError se in _messageList) {
                        if (se.Class == CustomMessageSeverity) {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// The severity to be used with custom messages from Stored Procedure RAISERROR calls
        /// </summary>
        private const int CustomMessageSeverity = 9;

        /// <summary>
        /// Returns the first message in the message list with a severity equal to CustomMessageSeverity.  Returns String.Empty otherwise
        /// </summary>
        public string FirstCustomMessage {
            get {
                foreach (SqlError se in _messageList) {
                    if (se.Class == CustomMessageSeverity) {
                        return se.Message;
                    }
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// This property allows you to define whether or not you want to allow
        /// the object to log errors. This should only be used in the error log
        /// class unless you know exactly what you're doing.
        /// </summary>
        public bool LogErrors {
            get {
                return _logErrors;
            }
            set {
                _logErrors = value;
            }
        }
    }
}