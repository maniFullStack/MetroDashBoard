using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Diagnostics;
using System.Xml;
using System.Data;
using System.Data.SqlClient;

namespace WebsiteUtilities {
    /// <summary>
    /// This class is a static class used for validating data.
    /// </summary>f
    public static class Validation {
        /// <summary>
        /// Empty private constructor for static (only) class.
        /// </summary>
        

        /// <summary>
        /// This function does regular expression matching and returns a true or false value for the results of the pattern match.
        /// </summary>
        /// <param name="toCheck">The string to be checked.</param>
        /// <param name="regEx">The regular expression pattern.</param>
        /// <returns>Whether (true) or not (false) the pattern matched.</returns>
        public static bool RegExMatch(this string toCheck, string regEx) {
            Regex regexp = new Regex(regEx);
            return regexp.IsMatch(toCheck);
        }

        public static bool RegExMatch(this string toCheck, string regEx, out MatchCollection matches) {
            Regex regexp = new Regex(regEx);
            matches = regexp.Matches(toCheck);
            return regexp.IsMatch(toCheck);
        }

        /// <summary>
        /// Perform regular expression matching returning a count for occurences
        /// </summary>
        /// <param name="toCheck">The string to be checked.</param>
        /// <param name="regEx">The regular expression pattern.</param>
        /// <returns>Number of occurences of pattern in toCheck string</returns>
        public static int RegExCount(this string toCheck, string regEx) {
            Regex regexp = new Regex(regEx);
            return regexp.Matches(toCheck).Count;
        }

        /// <summary>
        /// This method checks data to see if it is valid.
        /// </summary>
        /// <param name="strVal">The string data to check.</param>
        /// <param name="typeValid">This specifies the type of validation to use.</param>
        /// <returns></returns>
        public static bool RegExCheck(this string strVal, ValidationType typeValid) {
            switch (typeValid) {
                case ValidationType.Name:
                    return RegExMatch(strVal, @"^[a-zA-Z ]+$");
                case ValidationType.NameNoSpace:
                    return RegExMatch(strVal, @"^[a-zA-Z]+$");
                case ValidationType.Email:
                    return RegExMatch(strVal.ToLower(), @"^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[a-z]{2,20})\b$");
                case ValidationType.Password:
                    return RegExMatch(strVal, @"^[a-zA-Z0-9 !#$@%&*+=?^_{|}~-]{8,}$"); // At least 8 characters 
                case ValidationType.Postal:
                    return RegExMatch(strVal.ToUpper(), @"^[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ] [0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$");
                case ValidationType.PostalAlternate:
                    return RegExMatch(strVal.ToUpper(), @"^[ABCEGHJKLMNPRSTVXY][0-9][ABCEGHJKLMNPRSTVWXYZ][ ]?[0-9][ABCEGHJKLMNPRSTVWXYZ][0-9]$");
                case ValidationType.ZipCode:
                    return RegExMatch( strVal , @"^[0-9]{5}(-[0-9]{4})?$" );
                case ValidationType.Phone:
                    return RegExMatch( strVal , @"^\d{3}-\d{3}-\d{4}$" );
                case ValidationType.PhoneOpen:
                    return RegExMatch(strVal, @"^\(?\d{3}\)?[- \.]?\d{3}[- \.]?\d{4}$");
                case ValidationType.PhoneAlternate:
                    return RegExMatch(strVal, @"^\d{10}$");
                case ValidationType.PhoneExt:
                    return RegExMatch(strVal, @"^\d{1,5}$");
                default:
                    return false;
            }
        }
        /// <summary>
        /// This method checks data to see if it is valid as well as allowing you to specify a minimum/maximum length. (Overrides Password minimum length.)
        /// </summary>
        /// <param name="strVal">The string data to check.</param>
        /// <param name="typeValid">This specifies the type of validation to use.</param>
        /// <param name="minLength">This is the minimum length that the string must be to pass validation.</param>
        /// <param name="maxLength">This is the maximum length that the string must be to pass validation.</param>
        /// <returns></returns>
        public static bool RegExCheck(this string strVal, ValidationType typeValid, int minLength, int maxLength) {
            if (minLength == 0 && strVal.Length == 0) {
                return true;
            } else if (strVal.Length < minLength || strVal.Length > maxLength) {
                return false;
            } else if (typeValid == ValidationType.Password) {
                return RegExMatch(strVal, @"^[a-zA-Z0-9 !#$@%&*+=?^_{|}~-]{" + minLength + "," + maxLength + "}$");
            } else {
                return RegExCheck(strVal, typeValid);
            }
        }

        /// <summary>
        /// Used by function IsDateTime() to validate format of string as a date (MM/dd/yyyy)
        /// </summary>
        /// <param name="value">A string to attempt to convert to a date time.</param>
        /// <returns>DateTime from string or DateTime.MinValue if String was invalid</returns>
        public static DateTime TryParseDateTime(this string value) {
            return TryParseDateTime(value, "MM/dd/yyyy", DateTime.MinValue);
        }
        /// <summary>
        /// Attempts to convert a string to a date and checks it in a specified format
        /// </summary>
        /// <param name="value">A string to attempt to convert to a date time.</param>
        /// <param name="format">Standard DateTime format string, must be valid</param>
        /// <returns>DateTime from string or DateTime.MinValue if String or format was invalid</returns>
        public static DateTime TryParseDateTime(this string value, string format) {
            return TryParseDateTime(value, format, DateTime.MinValue);
        }
        /// <summary>
        /// Attempts to convert a string to a date and checks it in a specified format
        /// </summary>
        /// <param name="value">A string to attempt to convert to a date time</param>
        /// <param name="format">Standard DateTime format string, must be valid</param>
        /// <param name="defaultDate">Default return value in case of error</param>
        /// <returns>DateTime from string or specified default value if String or format was invalid</returns>
        public static DateTime TryParseDateTime(this string value, string format, DateTime defaultDate) {
            if (null != value && 0 < value.Length) {
                try {
                    return DateTime.ParseExact(value, format, null);
                } catch { }
            }
            return defaultDate;
        }
        /// <summary>
        /// Checks to see if a string is a valid (MM/dd/yyyy) datetime or not.
        /// </summary>
        /// <param name="value">The string value to attempt to convert to a date time.</param>
        /// <returns>True if the string is a valid date time.</returns>
        public static bool IsDateTime(this string value) {
            return IsDateTime(value, "MM/dd/yyyy");
        }
        /// <summary>
        /// Checks to see if a string is a valid datetime or not.
        /// </summary>
        /// <param name="value">The string value to attempt to convert to a date time.</param>
        /// <param name="format">DateTime format that 'value' must comply with</param>
        /// <returns>True if the string is a valid date time.</returns>
        public static bool IsDateTime(this string value, string format) {
            DateTime date = TryParseDateTime(value, format);
            return (DateTime.MinValue != date);
        }
        /// <summary>
        /// Checks to see if a string is a valid datetime or not.
        /// </summary>
        /// <param name="value">The string value to attempt to convert to a date time.</param>
        /// <param name="format">DateTime format that 'value' must comply with</param>
        /// <param name="datetimeobj">An out parameter which contains the parsed date time object.</param>
        /// <returns>True if the string is a valid date time.</returns>
        public static bool IsDateTime(this string value, string format, out DateTime datetimeobj) {
            datetimeobj = TryParseDateTime(value, format);
            return (DateTime.MinValue != datetimeobj);
        }
        /// <summary>
        /// Checks to see if string is proper datetime format (MM/dd/yyyy), then checks if date is greater than or equal to comparison
        /// </summary>
        /// <param name="value">The string value to attempt to convert to a date time</param>
        /// <param name="compare">DateTime of date to compare value to, if value was a valid date</param>
        /// <returns>True if value is a valid date and is greater or equal to comparison date, and false if else</returns>
        public static bool IsGreaterDateTime(this string value, DateTime compare) {
            DateTime date = TryParseDateTime(value, "MM/dd/yyyy", DateTime.MinValue);
            if (date > DateTime.MinValue) {
                return (date >= compare);
            }
            return false;
        }
        /// <summary>
        /// Checks to see if string is proper datetime format (MM/dd/yyyy), then checks if date is less than or equal to comparison
        /// </summary>
        /// <param name="value">The string value to attempt to convert to a date time</param>
        /// <param name="compare">DateTime of date to compare value to, if value was a valid date</param>
        /// <returns>True if value is a valid date and is lesser or equal to comparison date, and false if else</returns>
        public static bool IsLesserDateTime(this string value, DateTime compare) {
            DateTime date = TryParseDateTime(value, "MM/dd/yyyy", DateTime.MinValue);
            if (date > DateTime.MinValue) {
                return (date <= compare);
            }
            return false;
        }
        
        /// <summary>
        /// Runs the query string passed as a parameter. Returns true if records were returned for the query and false if none were returned.
        /// </summary>
        /// <param name="query">The select statement.</param>
        /// <param name="sqlDB">The database object to run the query on.</param>
        /// <param name="sqlParams">Optional. SQLParamList for the query.</param>
        /// <returns>True if results exist, false otherwise</returns>
        public static bool IsInDBTable (this string query, SQLDatabase sqlDB, SQLParamList sqlParams)
        {
            if (query.Length > 0) {
                DataTable dt = sqlDB.QueryDataTable(query, sqlParams);
                if (dt.Rows.Count >= 1) return true;
            }
            return false;
        }

        /// <summary>
        /// Runs the query string passed as a parameter. Returns true if records were returned for the query and false if none were returned.
        /// </summary>
        /// <param name="query">The select statement.</param>
        /// <param name="sqlDB">The database object to run the query on.</param>
        /// <param name="sqlParams">Optional. Any SqlParameters for the query.</param>
        /// <returns>True if results exist, false otherwise</returns>
        public static bool IsInDBTable(this string query, SQLDatabase sqlDB, params SqlParameter[] sqlParams)
        {
            if (query.Length <= 0) return false;

            DataTable dt = sqlDB.QueryDataTable(query, sqlParams);
            if ( dt.Rows.Count >= 1 ) return true;
            return false;
        }
        /// <summary>
        /// Validates whether the given vale falls into a range of values of the same type 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool In<T>(this T obj, params T[] values)
            where T : IComparable
        {

            if (values == null)
                return false;

            return values.Any(o => o.CompareTo(obj) == 0);

            // if no values found then is false 
        }

        public static bool IsNumeric(this string str)
        {
            bool isNumeric;
            {
                int val;
                isNumeric = int.TryParse(str, out val);
                if (isNumeric)
                    return true;
            }
            {
                float val;
                isNumeric = float.TryParse(str, out val);
                if (isNumeric)
                    return true;
            }
            {
                double val;
                isNumeric = double.TryParse(str, out val);
                if (isNumeric)
                    return true;

            }
            {
                long val;
                isNumeric = long.TryParse(str, out val);
                if (isNumeric)
                    return true;
            }
            return false;

        }

        /// <summary>
        /// Checks whether or not a SIN is valid. Does not check formatting, only if the number itself is a valid SIN.
        /// </summary>
        /// <param name="sin">Nine-digit SIN string.</param>
        /// <returns></returns>
        public static bool ValidateSIN(this string sin) {
            //See http://www.ryerson.ca/JavaScript/lectures/forms/textValidation/sinProject.html
            // for what this was based on.
            int throwaway;
            if (sin == null || sin.Length != 9 || !Int32.TryParse(sin, out throwaway)) {
                return false;
            }
            int ttl = 0;
            int check = Int32.Parse(sin[8].ToString());
            for (int i = 0; i < 8; i++) {
                if (i % 2 == 0) {
                    ttl += Int32.Parse(sin[i].ToString());
                } else {
                    int digit = Int32.Parse(sin[i].ToString());
                    if (digit < 5) {
                        //Digits under 5 are just multiplied by 2
                        ttl += digit * 2;
                    } else {
                        //Digits above 5 will add to two-digit numbers (5->10,6->12,7->14,8->16,9->18)
                        // which will have to have each digit summed (10->1,12->3,14->5,16->7,18->9)
                        // Those sums are (in this case) odd numbers from 1 to 9.
                        ttl += (2 * (digit - 5)) + 1; //Odd number in series = 2n-1
                    }
                }
            }
            if (ttl % 10 == 0 && check == 0) {
                //If the total is a multiple of 10, and the check digit is 0
                return true;
            } else {
                //If the total is not a multiple of 10, subtract the remainder
                //  from 10 and see if it's equal to the check digit
                return 10 - (ttl % 10) == check;
            }
        }




    }
    /// <summary>
    /// This enumeration is used within the Validation class to differentiate between different types of validations.
    /// </summary>
    public enum ValidationType {
        /// <summary>
        /// Only letters and spaces allowed.
        /// </summary>
        Name,
        /// <summary>
        /// Only letters allowed.
        /// </summary>
        NameNoSpace,
        /// <summary>
        /// Only valid email address formats allowed.
        /// </summary>
        Email,
        /// <summary>
        /// Only letters, spaces and !#$@%&amp;*+=?^_{|}~- allowed. 8 characters or longer.
        /// </summary>
        Password,
        /// <summary>
        /// Only postal codes in the format "A1A 1A1" allowed.
        /// </summary>
        Postal,
        /// <summary>
        /// Postal codes in the format "A1A1A1" or "A1A 1A1" allowed.
        /// </summary>
        PostalAlternate,
        /// <summary>
        /// Only 5 digit (eg. 12345) or 9 digit (eg. 12345-6789) zip codes allowed.
        /// </summary>
        ZipCode ,
        /// <summary>
        /// Only numbers in the format "###-###-####" allowed.
        /// </summary>
        Phone ,
        /// <summary>
        /// "(###) ###-####" allowed, spaces or dashes with brackets around first three, all optional
        /// </summary>
        PhoneOpen ,
        /// <summary>
        /// Only 10 digit numbers (eg. "##########") allowed.
        /// </summary>
        PhoneAlternate,
        /// <summary>
        /// Only 1 to 5 digit numbers allowed.
        /// </summary>
        PhoneExt
    }
}