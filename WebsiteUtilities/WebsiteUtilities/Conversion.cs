using System;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Configuration;

namespace WebsiteUtilities {
    /// <summary>
    /// This class contains many static methods for converting anything from strings &amp; numbers to data tables &amp; Excel files.
    /// </summary>
    public static class Conversion {
        private const string dateformat = "dd/MM/yyyy h:mm:ss tt";
        private const string xmldateformat = "yyyy-MM-ddTHH:mm:ss";

        /// <summary>
        /// Empty private constructor for static (only) class.
        /// </summary>
 

        /// <summary>
        /// This function is used to convert a string to an integer.
        /// <para>Returns: the integer or 0 if invalid.</para>
        /// </summary>
        /// <param name="convert">The string to convert to integer.</param>
        /// <returns>0 on error or the string as an integer.</returns>
        public static int StringToInt(this string convert) {
            return StringToInt(convert, 0);
        }
        /// <summary>
        /// This function is used to convert a string to an long.
        /// <para>Returns: the long or 0 if invalid.</para>
        /// </summary>
        /// <param name="convert">The string to convert to long.</param>
        /// <returns>0 on error or the string as a long.</returns>
        public static long StringToLong(this string convert)
        {
            return StringToLong(convert, 0);
        }
        /// <summary>
        /// This function is used to convert a string to an integer.
        /// <para>Returns: the integer or 0 if invalid.</para>
        /// </summary>
        /// <param name="convert">The string to convert to integer.</param>
        /// <param name="returnValue">Changes default return from 0 to specified number</param>
        /// <returns>0 on error or the string as an long.</returns>
        public static long StringToLong(this string convert, int returnValue)
        {
            long num;
            try
            {
                if (convert == null || convert.Equals(""))
                    return returnValue;
                num = long.Parse(convert, NumberStyles.Any);
            }
            catch
            {
                num = returnValue;
            }
            return num;




        }
        /// <summary>
        /// This function is used to convert a string to an integer.
        /// <para>Returns: the integer or 0 if invalid.</para>
        /// </summary>
        /// <param name="convert">The string to convert to integer.</param>
        /// <param name="returnValue">Changes default return from 0 to specified number</param>
        /// <returns>0 on error or the string as an integer.</returns>
        public static int StringToInt(this string convert, int returnValue) {
            int num = returnValue;
            try {
                if (convert == null || convert.Equals("")) { return returnValue; }
                num = Int32.Parse(convert, NumberStyles.Any);
            } catch {
                num = returnValue;
            }
            return num;
        }
        /// <summary>
        /// This function is used to convert a string to an integer between a minimum and maximum value. If the value is outside of the bounds, it will be constrained to the bound value.
        /// </summary>
        /// <param name="convert">The string to convert to integer.</param>
        /// <param name="minVal">The minimum value to constrain the converted integer to (inclusive).</param>
        /// <param name="maxVal">The maximum value to constrain the converted integer to (inclusive).</param>
        /// <param name="invalidValue">The value to return if <paramref name="convert"/> cannot be converted.</param>
        /// <returns>An integer constrained within the minVal and maxVal (inclusive) or <paramref name="invalidValue"/> if it cannot be parsed. </returns>
        public static int StringToInt(this string convert, int minVal, int maxVal, int invalidValue) {
            int num = invalidValue;
            try {
                if (convert == null || convert.Equals("")) { return invalidValue; }
                num = Int32.Parse(convert);
                if (num > maxVal) num = maxVal;
                if (num < minVal) num = minVal;
            } catch {
                num = invalidValue;
            }
            return num;
        }

        /// <summary>
        /// This function is used to convert a string to a double.
        /// <para>Returns: the integer or 0 if invalid.</para>
        /// </summary>
        /// <param name="convert">The string to convert to double type.</param>
        /// <returns>0 on error or the string as a double.</returns>
        public static double StringToDbl(this string convert) {
            double num = 0;
            try {
                if (convert == null || convert.Equals("")) { return 0; }
                num = Double.Parse(convert, NumberStyles.Any);
            } catch {
                num = 0;
            }
            return num;
        }

        /// <summary>
        /// Convert the'True' or 'False' that an MSSQL bit returns to an int
        /// <para>Returns: 1 if 'True', otherwise 0</para>
        /// </summary>
        /// <param name="convert">The string boolean value to convert to 1.</param>
        /// <returns>1 if 'True', otherwise 0</returns>
        public static int StringBooleanToInt(this string convert) {
            return convert.Equals("True") ? 1 : 0;
        }

        /// <summary>
        /// This function is used to convert a string to a double.
        /// <para>Returns: the integer or 0 if invalid.</para>
        /// </summary>
        /// <param name="convert">The string to convert to double type.</param>
        /// <returns>0 on error or the string as a double.</returns>
        public static double StringToDbl(this string convert, double defaultVal) {
            double num = defaultVal;
            try {
                if (convert == null || convert.Equals("")) { return defaultVal; }
                num = Double.Parse(convert, NumberStyles.Any);
            } catch {
                num = defaultVal;
            }
            return num;
        }

        /// <summary>
        /// This function is used to convert a object, string or null to string.
        /// <para>Returns: the string or String.Empty.</para>
        /// </summary>
        /// <param name="convert">The object to convert to string.</param>
        /// <returns>string</returns>
        public static string ObjectToString(this object convert) {
            return (convert == null ? string.Empty : convert.ToString());
        }


        /// <summary>
        /// Converts an integer to a boolean. 1 = True, Anything else = False
        /// </summary>
        /// <param name="toBool">The integer to be converted to a boolean.</param>
        /// <returns>1 = True, Anything else = False</returns>
        public static bool IntToBool(this int toBool) {
            return ((toBool == 1) ? true : false);
        }

        /// <summary>
        /// Converts a string to a boolean. Any format of the word "true" or the number "1" = true. Anything else = false
        /// </summary>
        /// <param name="toBool">The string to be converted to a boolean.</param>
        /// <returns>"true", "1" = True, Anything else = False</returns>
        public static bool StringToBool(this string toBool) {
            if (toBool != null) {
                return (((toBool.ToLower().Equals("true")) || (toBool.Equals("1"))));
            } 
            return false;
            
        }

        /// <summary>
        /// This function converts a specific entity's safe XML supported equivalent back into the original entity. Used in conjunction with XMLDecodeString.
        /// </summary>
        /// <param name="fromxmlsafe">The XML string to encode.</param>
        /// <returns>An encoded XML string.</returns>
        public static string XMLEncodeString(this string toxmlsafe) {
            if (toxmlsafe != null) {
                toxmlsafe = toxmlsafe.Replace("<", "&lt;")
                                    .Replace(">", "&gt;")
                                    .Replace("&", "&amp;")
                                    .Replace("'", "&apos;")
                                    .Replace("\"", "&quot;");
            } else {
                toxmlsafe = "";
            }
            return toxmlsafe;
        }

        /// <summary>
        /// This function converts specific entities in a string to their safe XML supported equivalent. Used in conjunction with XMLEncodeString.
        /// </summary>
        /// <param name="fromxmlsafe">The XML string to decode.</param>
        /// <returns>A decoded XML string.</returns>
        public static string XMLDecodeString(this string fromxmlsafe) {
            if (fromxmlsafe == null) {
                fromxmlsafe = "";
            } else {
                fromxmlsafe = fromxmlsafe.Replace("&lt;", "<")
                                        .Replace("&gt;", ">")
                                        .Replace("&amp;", "&")
                                        .Replace("&apos;", "'")
                                        .Replace("&quot;", "\"");
            }
            return fromxmlsafe;
        }

        /// <summary>
        /// This function converts a DateTime object to the XML safe string in the format: yyyy-MM-ddTHH:mm:ss
        /// </summary>
        /// <param name="toConvert">The DateTime to convert to a string.</param>
        /// <returns>An XML date/time string in the format "yyyy-MM-ddTHH:mm:ss" or an empty string if there was an error.</returns>
        public static string DateTimeToXMLString(this DateTime toConvert) {
            try {
                return toConvert.ToString(xmldateformat);
            } catch {
                return "";
            }
        }

        /// <summary>
        /// Converts an ISO 8601 date/time string (yyyy-MM-ddTHH:mm:ss) to a DateTime object. Milliseconds, if they exist, are stripped.
        /// </summary>
        /// <param name="toConvert">The XML date/time string to convert.</param>
        /// <returns>A date time object. If there was an error loading the string, the DateTime will just be a new object.</returns>
        public static DateTime XMLDateToDateTime(this string toConvert) {
            DateTime temp;
            try {
                if (toConvert == null || toConvert.Equals("")) {
                    temp = new DateTime();
                } else {
                    if (toConvert.Length == 23) {
                        toConvert = toConvert.Substring(0, 19);
                    }
                    temp = DateTime.ParseExact(toConvert, xmldateformat, null);
                }
            } catch {
                temp = new DateTime();
            }
            return temp;
        }

        /// <summary>
        /// Converts a date/time string extracted from the database (dd/MM/yyyy h:mm:ss tt), to a datetime object.
        /// </summary>
        /// <param name="fromdb">The database date/time string to convert.</param>
        public static DateTime DBDateStringToDateTime(this string fromdb) {
            DateTime temp;
            try {
                temp = DateTime.ParseExact(fromdb, dateformat, null);
            } catch {
                temp = new DateTime();
            }
            return temp;
        }

        /// <summary>
        /// Converts a date time object to a format for use in inserting into database.
        /// </summary>
        /// <param name="date">The database date/time to convert.</param>
        public static string DateTimeToDBDateString(this DateTime date) {
            try {
                if (date.Year == 1) {
                    return "";
                } 
                return date.ToString(xmldateformat) + ".000";
                
            } catch {
                return "";
            }
        }

        /// <summary>
        /// Returns the date string in proper format, respective of the language.
        /// </summary>
        /// <param name="dt">The date to output.</param>
        /// <param name="langcode">The current language ID.</param>
        /// <param name="dateFormatType">Specifies how to format the returned date string.</param>
        public static string TranslateDate(this DateTime dt, string langcode, ConversionDateFormatType dateFormatType) {
            return TranslateDate(dt, langcode, dateFormatType, false);
        }

        /// <summary>
        /// Returns the date string in proper format, respective of the language.
        /// </summary>
        /// <param name="dt">The date to output.</param>
        /// <param name="langcode">The current language ID.</param>
        /// <param name="dateformat">Specifies how to format the returned date string.</param>
        /// <param name="addtz">Whether or not to add timezone information to the end of the date string.</param>
        public static string TranslateDate(this DateTime dt, string langcode, ConversionDateFormatType dateformat, bool addtz) {
            string dateformatstr;
            //Return empty string if the DateTime is the min value
            if (dt == DateTime.MinValue) {
                return "";
            }
            switch (dateformat) {
                case ConversionDateFormatType.FullDateTime:
                    dateformatstr = "dddd, MMMM dd, yyyy h:mm tt"; //6/15/2009 1:45:30 PM -> Monday, June 15, 2009 1:45 PM (en-US)
                    break;
                case ConversionDateFormatType.LongDate:
                    dateformatstr = "dddd, MMMM dd, yyyy"; //6/15/2009 1:45:30 PM -> Monday, June 15, 2009 (en-US)
                    break;
                case ConversionDateFormatType.ShortDate:
                    dateformatstr = "MM/dd/yyyy"; //6/15/2009 1:45:30 PM -> 6/15/2009 (en-US)
                    break;
                case ConversionDateFormatType.MonthName:
                    dateformatstr = "MMMM"; //6/15/2009 1:45:30 PM -> June
                    break;
                case ConversionDateFormatType.ShortMonthName:
                    dateformatstr = "MMM"; //6/15/2009 1:45:30 PM -> Jun
                    break;
                case ConversionDateFormatType.ShortTime:
                    dateformatstr = "h:mm tt"; //6/15/2009 1:45:30 PM -> 1:45 PM
                    break;
                case ConversionDateFormatType.LongTime:
                    dateformatstr = "h:mm:ss tt"; //6/15/2009 1:45:30 PM -> 1:45:30 PM
                    break;
                default:
                    dateformatstr = "MM/dd/yyyy h:mm tt"; //6/15/2009 1:45:30 PM -> 6/15/2009 1:45 PM (en-US)
                    break;
            }
            return dt.ToString(dateformatstr, CultureInfo.CreateSpecificCulture(langcode)) + (addtz ? " EST (GMT -5:00)" : "");
        }

        /// <summary>
        /// Returns the date string in proper format, respective of the language.
        /// </summary>
        /// <param name="datestring">The date in XML date string (ISO 8601) format.</param>
        /// <param name="langcode">The current language ID.</param>
        /// <param name="dateformat">Specifies how to format the returned date string.</param>
        public static string TranslateDate(this string datestring, string langcode, ConversionDateFormatType dateformat) {
            return TranslateDate(datestring, langcode, dateformat, false);
        }

        /// <summary>
        /// Returns the date string in proper format, respective of the language.
        /// </summary>
        /// <param name="datestring">The date in XML date string (ISO 8601) format.</param>
        /// <param name="langcode">The current language ID.</param>
        /// <param name="dateformat">Specifies how to format the returned date string.</param>
        /// <param name="addtz">Whether or not to add timezone information to the end of the date string.</param>
        public static string TranslateDate(this string datestring, string langcode, ConversionDateFormatType dateformat, bool addtz) {
            return TranslateDate(XMLDateToDateTime(datestring), langcode, dateformat, addtz);
        }

        /// <summary>
        /// This method converts a DataTable to a JSON array. Each object in the array corresponds to a row in the table and the column names are the keys for the key value pairs in each record.
        /// </summary>
        /// <param name="dt">The datatable to convert.</param>
        /// <returns>A JSON encoded string.</returns>
        public static string DataTableToJSON(this DataTable dt) {
            StringBuilder json = new StringBuilder("[");
            if (dt.Rows.Count > 0) {
                foreach (DataRow dr in dt.Rows) {
                    json.Append("{");
                    foreach (DataColumn dc in dt.Columns) {
                        json.Append("\"")
                            .Append(dc.ColumnName.Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", String.Empty))
                            .Append("\":\"")
                            .Append(dr[dc.ColumnName].ToString().Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", String.Empty))
                            .Append("\",");
                    }
                    json.Length -= 1;
                    json.Append("},");
                }
                json.Length -= 1;
            }
            json.Append("]");
            return json.ToString();
        }

        /// <summary>
        /// Takes a validated phone number and removes all special characters and formats as 10 digit plain number
        /// </summary>
        /// <param name="sPhone">Validated phone number string</param>
        /// <param name="error"></param>
        /// <returns>Plain phone number "##########"</returns>
        public static string StripPhoneNumber(this string sPhone, ref bool error) {
            return StripPhoneNumber(sPhone, PhoneType.Plain, ref error);
        }
        /// <summary>
        /// akes a validated phone number and removes all special characters and formats as specified
        /// </summary>
        /// <param name="sPhone">Validated phone number string</param>
        /// <param name="ptReturnType">The way the phone number will be formatted</param>
        /// <returns>Formatted phone number</returns>
        public static string StripPhoneNumber(this string sPhone, PhoneType ptReturnType, ref bool error) {
            sPhone = System.Text.RegularExpressions.Regex.Replace(sPhone, "[^0-9]", "");
            if (sPhone.Length != 10) {
                error = true;
                return "";
            }
            switch (ptReturnType) {
                case PhoneType.Dashed:
                    sPhone = String.Format("{0}-{1}-{2}", sPhone.Substring(0, 3), sPhone.Substring(3, 3), sPhone.Substring(6, 4));
                    break;
                case PhoneType.Styled:
                    sPhone = String.Format("({0}) {1}-{2}", sPhone.Substring(0, 3), sPhone.Substring(3, 3), sPhone.Substring(6, 4));
                    break;
                case PhoneType.Spaced:
                    sPhone = String.Format("{0} {1} {2}", sPhone.Substring(0, 3), sPhone.Substring(3, 3), sPhone.Substring(6, 4));
                    break;
                default:
                    break;
            }
            error = false;
            return sPhone;
        }

        /// <summary>
        /// Formats a string number into a percentage to 0 decimal places. If the string is null or empty, an empty string will be returned. If the string is not valid, 0% will be returned.
        /// </summary>
        /// <param name="number">The string containing the number to be parsed and formatted.</param>
        public static string FormatPercent(this string number) {
            return FormatPercent(number, 0);
        }
        /// <summary>
        /// Formats a number into a percentage to 0 decimal places.
        /// </summary>
        /// <param name="number">The number to be formatted.</param>
        public static string FormatPercent(this double number) {
            return FormatPercent(number, 0);
        }

        /// <summary>
        /// Formats a string number into a percentage. If the string is null or empty, an empty string will be returned. If the string is not valid, 0% will be returned.
        /// </summary>
        /// <param name="number">The string containing the number to be parsed and formatted.</param>
        /// <param name="decimalPlaces">The number of decimal places to show.</param>
        public static string FormatPercent(this string number, int decimalPlaces) {
            if (String.IsNullOrEmpty(number)) {
                return String.Empty;
            } else {
                return FormatPercent(StringToDbl(number), decimalPlaces);
            }
        }
        /// <summary>
        /// Formats a number into a percentage.
        /// </summary>
        /// <param name="number">The number to be formatted.</param>
        /// <param name="decimalPlaces">The number of decimal places to show.</param>
        public static string FormatPercent(this double number, int decimalPlaces) {
            if (decimalPlaces > -1) {
                return String.Format("{0:0." + new String('0', decimalPlaces) + "}%", number);
            } else {
                return String.Format("{0}%", number);
            }
        }

        /// <summary>
        /// Formats a number to a certain number of decimal places.
        /// </summary>
        /// <param name="number">The number to format.</param>
        /// <param name="decimalPlaces">The number of decimal places to format it to.</param>
        /// <returns>Returns String.Empty if the number is null or empty or the formatted number otherwise. Defaults to 0 if conversion fails.</returns>
        public static string FormatNumber(this string number, int decimalPlaces) {
            if (String.IsNullOrEmpty(number)) {
                return String.Empty;
            } else {
                return FormatNumber(StringToDbl(number), decimalPlaces);
            }
        }

        /// <summary>
        /// Formats a number to a certain number of decimal places.
        /// </summary>
        /// <param name="number">The number to format.</param>
        /// <param name="decimalPlaces">The number of decimal places to format it to.</param>
        /// <returns>Returns a string containing the formatted number.</returns>
        public static string FormatNumber(this double number, int decimalPlaces) {
            if (decimalPlaces > -1) {
                return String.Format("{0:0." + new String('0', decimalPlaces) + "}", number);
            } else {
                return String.Format("{0}", number);
            }
        }

        /// <summary>
        /// Generates an Excel file (xls, not xlsx) from the passed datatable.
        /// </summary>
        /// <param name="dt">The datatable to convert to the Excel file. Data column names will be in the first row,.</param>
        /// <param name="sheetName">The name of the worksheet in Excel. Spaces are automatically converted to underscores.</param>
        /// <param name="outputFile">The full path to the output file.</param>
        /// <param name="overwrite">If true, the current file will be erased if it exists. Otherwise, the table will be added to the existing file. If the table matches a table that already exists in the file, the file's version will be dropped.</param>
        public static bool DataTableToExcel(this DataTable dt, string sheetName, string outputFile, bool overwrite) {
            return DataTableToExcel(dt, sheetName, outputFile, overwrite, int.MaxValue);
        }
        
        /// <summary>
        /// Generates an Excel file (type depends on connection string from web.config) from the passed datatable. Note that column names are limited to 64 characters. If they are longer they will be truncated.
        /// </summary>
        /// <param name="dt">The datatable to convert to the Excel file. Data column names will be in the first row,.</param>
        /// <param name="sheetName">The name of the worksheet in Excel. Spaces are automatically converted to underscores.</param>
        /// <param name="outputFile">The full path to the output file.</param>
        /// <param name="overwrite">If true, the current file will be erased if it exists. Otherwise, the table will be added to the existing file. If the table matches a table that already exists in the file, the file's version will be dropped.</param>
        /// <param name="maxRows">The maximum number of rows to output from the DataTable.</param>
        public static bool DataTableToExcel(this DataTable dt, string sheetName, string outputFile, bool overwrite, int maxRows) {
            string path;
            return DataTableToExcel(dt, sheetName, outputFile, overwrite, out path);
        }
        /// <summary>
        /// Generates an Excel file (xls, not xlsx) from the passed datatable.
        /// </summary>
        /// <param name="dt">The datatable to convert to the Excel file. Data column names will be in the first row,.</param>
        /// <param name="sheetName">The name of the worksheet in Excel. Spaces are automatically converted to underscores.</param>
        /// <param name="outputFile">The full path to the output file.</param>
        /// <param name="overwrite">If true, the current file will be erased if it exists. Otherwise, the table will be added to the existing file. If the table matches a table that already exists in the file, the file's version will be dropped.</param>
        /// <param name="fullPathToFile">The full path to the file on the file system.</param>
        public static bool DataTableToExcel(this DataTable dt, string sheetName, string outputFile, bool overwrite, out string fullPathToFile) {
            return DataTableToExcel(dt, sheetName, outputFile, overwrite, int.MaxValue, out fullPathToFile);
        }
        /// <summary>
        /// Generates an Excel file (xls, not xlsx) from the passed datatable.
        /// </summary>
        /// <param name="dt">The datatable to convert to the Excel file. Data column names will be in the first row,.</param>
        /// <param name="sheetName">The name of the worksheet in Excel. Spaces are automatically converted to underscores.</param>
        /// <param name="outputFile">The full path to the output file.</param>
        /// <param name="overwrite">If true, the current file will be erased if it exists. Otherwise, the table will be added to the existing file. If the table matches a table that already exists in the file, the file's version will be dropped.</param>
        /// <param name="maxRows">The maximum number of rows to output from the DataTable.</param>
        /// <param name="fullPathToFile">The full path to the file on the file system.</param>
        public static bool DataTableToExcel(this DataTable dt, string sheetName, string outputFile, bool overwrite, int maxRows, out string fullPathToFile) {
            return DataTableToExcel(dt, sheetName, outputFile, overwrite, maxRows, int.MaxValue, out fullPathToFile);
        }

        /// <summary>
        /// Generates an Excel file (type depends on connection string from web.config) from the passed datatable. Note that column names are limited to 64 characters. If they are longer they will be truncated.
        /// </summary>
        /// <param name="dt">The datatable to convert to the Excel file. Data column names will be in the first row,.</param>
        /// <param name="sheetName">The name of the worksheet in Excel. Spaces are automatically converted to underscores.</param>
        /// <param name="outputFile">The full path to the output file.</param>
        /// <param name="overwrite">If true, the current file will be erased if it exists. Otherwise, the table will be added to the existing file. If the table matches a table that already exists in the file, the file's version will be dropped.</param>
        /// <param name="maxRows">The maximum number of rows to output from the DataTable.</param>
        /// <param name="maxCols">The maximum number of columns to output from the DataTable.</param>
        /// <param name="fullPathTofile">The full path to the file on the file system.</param>
        public static bool DataTableToExcel(this DataTable dt, string sheetName, string outputFile, bool overwrite, int maxRows, int maxCols, out string fullPathTofile) {
            fullPathTofile = outputFile;
            if (dt == null || String.IsNullOrEmpty(outputFile)) { throw new ArgumentException("The datatable cannot be null and outputFile must be a valid string."); }
            bool rtrn = false;

            if (overwrite && File.Exists(outputFile)) {
                File.Delete(outputFile);
            }

            string connString = WebConfigurationManager.OpenWebConfiguration("/").ConnectionStrings.ConnectionStrings["Excel"].ConnectionString;
            if (String.IsNullOrEmpty(connString)) {
                OleDbConnectionStringBuilder csb = new OleDbConnectionStringBuilder();
                csb.Provider = "Microsoft.Jet.OLEDB.4.0";
                csb.DataSource = outputFile;
                csb.Add("Extended Properties", "Excel 8.0;HDR=YES;");
                connString = csb.ConnectionString;
            } else {
                connString = String.Format(connString, outputFile);
            }

            using (OleDbConnection conn = new OleDbConnection(connString)) {
                try {
                    conn.Open();
                    fullPathTofile = conn.DataSource;

                    StringBuilder sbCols = new StringBuilder();
                    StringBuilder sbPars = new StringBuilder();
                    StringBuilder sbColsForTable = new StringBuilder();

                    int id = 0, colCount = 0;

                    foreach (DataColumn dc in dt.Columns) {
                        //Check if we've reached the max col count
                        colCount++;
                        if (colCount > maxCols) { break; }

                        string columnName = dc.ColumnName
                                                .Trim()
                                                .Replace(",", "")
                                                .Replace(".", "#")
                                                .Replace("\n", " ")
                                                .Replace("\r", "")
                                                .Replace("[", "")
                                                .Replace("]", "")
                                                .Replace("(", "")
                                                .Replace(")", "");
                        
                        //Strip extra whitespace
                        while (columnName.Contains("  ")) {
                            columnName = columnName.Replace("  ", " ");
                        }

                        if (columnName.Length > 64) {
                            if (WebConfigurationManager.OpenWebConfiguration("/").AppSettings.Settings["TELUSExcelLimitation"] != null && WebConfigurationManager.OpenWebConfiguration("/").AppSettings.Settings["TELUSExcelLimitation"].Value.Equals("1")) {
                                string lastThreeChars = columnName.Substring(columnName.Length - 3, 3); //HARDCODED FIX FOR TELUS WEBSITE (TELUS REPORTING PORTAL)
                                columnName = columnName.Truncate(61);
                                columnName += lastThreeChars; //HARDCODED FIX FOR TELUS WEBSITE (TELUS REPORTING PORTAL)
                            } else {
                                columnName = columnName.Truncate(64); //64 char limitation on Jet Provider
                            }
                        }
                        
                        sbCols.Append(",[");
                        sbCols.Append(columnName);
                        sbCols.Append("]");

                        sbPars.Append(",@Col");
                        sbPars.Append(id++);

                        sbColsForTable.Append(",[");
                        sbColsForTable.Append(columnName);
                        sbColsForTable.Append("] ");
                        if (dc.DataType == typeof(String)) {
                            sbColsForTable.Append("text");
                        } else if (dc.DataType == typeof(Int32)) {
                            sbColsForTable.Append("int");
                        } else if (dc.DataType == typeof(Decimal)) {
                            sbColsForTable.Append("numeric");
                        } else {
                            sbColsForTable.Append("text");
                        }
                    }
                    //Remove starting commas
                    sbCols.Remove(0, 1);
                    sbPars.Remove(0, 1);
                    sbColsForTable.Remove(0, 1);

                    //Create the table
                    OleDbCommand cmd1 = conn.CreateCommand();
                    try {
                        cmd1.CommandText = "DROP TABLE [" + sheetName + "];";
                        cmd1.ExecuteNonQuery();
                    } catch { }
                    cmd1.CommandText = String.Format("CREATE Table [{0}] ({1})", sheetName, sbColsForTable.ToString());
                    cmd1.ExecuteNonQuery();

                    //Insert the values
                    string query = String.Format("INSERT INTO [{0}] ({1}) VALUES ({2})", sheetName, sbCols.ToString(), sbPars.ToString());
                    using (OleDbCommand cmd2 = conn.CreateCommand()) {
                        cmd2.CommandText = query;
                        int cntr = 0;
                        foreach (DataRow dr in dt.Rows) {
                            cmd2.Parameters.Clear();
                            for (int cind = 0; cind < dt.Columns.Count; cind++) {
                                cmd2.Parameters.Add(new OleDbParameter("@Col" + cind, dr[cind]));
                            }
                            cmd2.ExecuteNonQuery();
                            cntr++;
                            if (cntr >= maxRows) {
                                break;
                            }
                        }
                    }
                    rtrn = true;
                } catch {
                    throw;
                } finally {
                    if (conn.State != ConnectionState.Closed) {
                        conn.Close();
                    }
                }
            }
            return rtrn;
        }
        /// <summary>
        /// Generates a CSV file from the passed datatable.
        /// </summary>
        /// <param name="dt">The datatable to convert to the CSV file. Data column names will be in the first row,.</param>
        /// <param name="outputFile">The full path to the output file.</param>
        /// <param name="overwrite">If true, the current file will be erased if it exists. Otherwise, it will be appended.</param>
        public static bool DataTableToCSV(this DataTable dt, string outputFile, bool overwrite) {
            return DataTableToCSV(dt, outputFile, overwrite, Int32.MaxValue);
        }
        /// <summary>
        /// Generates a CSV file from the passed datatable.
        /// </summary>
        /// <param name="dt">The datatable to convert to the CSV file. Data column names will be in the first row,.</param>
        /// <param name="outputFile">The full path to the output file.</param>
        /// <param name="overwrite">If true, the current file will be erased if it exists. Otherwise, it will be appended.</param>
        /// <param name="maxCols">The maximum number of columns to output from the DataTable.</param>
        public static bool DataTableToCSV(this DataTable dt, string outputFile, bool overwrite, int maxCols) {
            return DataTableToCSV(dt, outputFile, overwrite, Int32.MaxValue, maxCols);
        }
        /// <summary>
        /// Generates a CSV file from the passed datatable.
        /// </summary>
        /// <param name="dt">The datatable to convert to the CSV file. Data column names will be in the first row. Note: Currently, newline characters will be replaced by spaces.</param>
        /// <param name="outputFile">The full path to the output file.</param>
        /// <param name="overwrite">If true, the current file will be erased if it exists. Otherwise, it will be appended.</param>
        /// <param name="maxRows">The maximum number of rows to output from the DataTable.</param>
        /// <param name="maxCols">The maximum number of columns to output from the DataTable.</param>
        public static bool DataTableToCSV(this DataTable dt, string outputFile, bool overwrite, int maxRows, int maxCols) {
            if (dt == null || String.IsNullOrEmpty(outputFile)) { throw new ArgumentException("The datatable cannot be null and outputFile must be a valid string."); }
            bool rtrn = false;

            if (overwrite && File.Exists(outputFile)) {
                File.Delete(outputFile);
            }

            try {
                using (CSVWriter csv = new CSVWriter(outputFile)) {
                    int colCount = 0;

                    //Get header row
                    CSVRow header = new CSVRow();
                    foreach (DataColumn dc in dt.Columns) {
                        //Check if we've reached the max col count
                        colCount++;
                        if (colCount > maxCols) { break; }
                        //Add the column name
                        header.Add(dc.ColumnName.Replace("\r", "").Replace("\n", " "));
                    }
                    csv.WriteRow(header);

                    //Insert the data
                    int cntr = 0;
                    foreach (DataRow dr in dt.Rows) {
                        CSVRow row = new CSVRow();
                        for (int cind = 0; cind < dt.Columns.Count; cind++) {
                            row.Add(dr[cind].ToString().Replace("\r", "").Replace("\n", " "));
                        }
                        csv.WriteRow(row);
                        cntr++;
                        if (cntr >= maxRows) {
                            break;
                        }
                    }
                    rtrn = true;
                }
            } catch {
                throw;
            }
            return rtrn;
        }
        /// <summary>
        /// Converts a CSV file into a data table.
        /// </summary>
        /// <param name="csvFilePath">The full path to the CSV file.</param>
        /// <param name="columnNamesInFirstRow">If true, the values in the first row will be assumed to </param>
        /// <returns></returns>
        public static DataTable CSVToDataTable( string csvFilePath, bool columnNamesInFirstRow) {
            DataTable dt = null;

            try {
                using (CSVReader csv = new CSVReader(csvFilePath)) {
                    dt = ConvertCSVToDataTable(csv, columnNamesInFirstRow);
                }
            }
            catch (Exception ex) {
                ErrorHandler.WriteLog("WebsiteUtilities.Conversion.CSVToDataTable(filepath)", "Error converting CSV to DataTable.", ErrorHandler.ErrorEventID.General, ex);
                dt = null;
            }
            return dt;
        }
        /// <summary>
        /// Converts a CSV stream into a data table.
        /// </summary>
        /// <param name="csvFileStream">The stream associated with the CSV file.</param>
        /// <param name="columnNamesInFirstRow">If true, the values in the first row will be assumed to </param>
        /// <returns></returns>
        public static DataTable CSVToDataTable(Stream csvFileStream, bool columnNamesInFirstRow) {
            DataTable dt = null;

            try {
                using (CSVReader csv = new CSVReader(csvFileStream)) {
                    dt = ConvertCSVToDataTable(csv, columnNamesInFirstRow);
                }
            } catch (Exception ex) {
                ErrorHandler.WriteLog("WebsiteUtilities.Conversion.CSVToDataTable(stream)", "Error converting CSV to DataTable.", ErrorHandler.ErrorEventID.General, ex);
                dt = null;
            }
            return dt;
        }
        /// <summary>
        /// Takes a CSVReader and converts it into a data table.
        /// </summary>
        /// <param name="csv">The CSVReader object to convert.</param>
        /// <param name="columnNamesInFirstRow">If true, the first row's values will be treated as the data tables column names. Otherwise, they will be loaded as "Column 1", "Column 2", etc.</param>
        /// <returns>The converted datatable.</returns>
        private static DataTable ConvertCSVToDataTable(CSVReader csv, bool columnNamesInFirstRow) {
            CSVRow row = new CSVRow();
            bool firstRow = true;
            DataTable dt = new DataTable();
            while (csv.ReadRow(row)) {
                if (firstRow) {
                    //Load the columns
                    for (int i = 0; i < row.Count; i++) {
                        if (columnNamesInFirstRow) {
                            dt.Columns.Add(row[i]); //Load from first row
                        } else {
                            dt.Columns.Add("Column " + (i + 1)); //Load basic name
                        }
                    }
                    if (columnNamesInFirstRow) {
                        //If we loaded the column names from the first row, we need to get the next row to load.
                        if (!csv.ReadRow(row)) {
                            break;
                        }
                    }
                    firstRow = false;
                }
                //Load the columns
                DataRow dr = dt.NewRow();
                for (int col = 0; col < row.Count; col++) {
                    dr[col] = row[col];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Transposes (switches rows for columns) a DataTable and returns the new transposed DataTable object. Assumes all values are strings.
        /// </summary>
        /// <param name="inputTable">The DataTable to transpose.</param>
        /// <returns>The transposed DataTable.</returns>
        public static DataTable TransposeDataTable(this DataTable inputTable) {
            if (inputTable == null) { return null; }

            DataTable outputTable = new DataTable();

            // Add columns by looping rows

            // Header row's first column is same as in inputTable
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

            // Header row's second column onwards, 'inputTable's first column taken
            foreach (DataRow inRow in inputTable.Rows) {
                string newColName = inRow[0].ToString();
                outputTable.Columns.Add(newColName);
            }

            // Add rows by looping columns        
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++) {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++) {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;
        }
    }

    /// <summary>
    /// The type of phone number to validate.
    /// </summary>
    public enum PhoneType {
        /// <summary>
        /// #########
        /// </summary>
        Plain,
        /// <summary>
        /// (###) ###-####
        /// </summary>
        Styled,
        /// <summary>
        /// ###-###-####
        /// </summary>
        Dashed,
        /// <summary>
        /// ### ### ####
        /// </summary>
        Spaced
    }
    /// <summary>
    /// This enumeration is used within the Validation class to differentiate between different types of date formats.
    /// </summary>
    public enum ConversionDateFormatType {
        /// <summary>
        /// 6/15/2009 (en-US)
        /// </summary>
        ShortDate,
        /// <summary>
        /// Monday, June 15, 2009 (en-US)
        /// </summary>
        LongDate,
        /// <summary>
        /// 6/15/2009 1:45 PM (en-US)
        /// </summary>
        GeneralDateTime,
        /// <summary>
        /// Monday, June 15, 2009 1:45 PM (en-US)
        /// </summary>
        FullDateTime,
        /// <summary>
        /// June
        /// </summary>
        MonthName,
        /// <summary>
        /// Jun
        /// </summary>
        ShortMonthName,
        /// <summary>
        /// 1:20 PM
        /// </summary>
        ShortTime,
        /// <summary>
        /// 1:45:30 PM
        /// </summary>
        LongTime
    }
}
