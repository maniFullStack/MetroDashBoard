using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteUtilities;

namespace SharedClasses {
    public static class ReportingTools {


        /// <summary>
        /// Formats a string decimal into a percentage to one decimal place.
        /// </summary>
        /// <param name="value">The decimal number as a string</param>
        public static string FormatPercent( string value) {
            return FormatPercent( value, 1 );
        }

        /// <summary>
        /// Formats a string decimal into a percentage to the given number of decimal places.
        /// </summary>
        /// <param name="value">The decimal number as a string</param>
        /// <param name="decimalPlaces">The number of decimal places to show.</param>
        public static string FormatPercent( string value, int decimalPlaces ) {
            double percent = Conversion.StringToDbl( value, Double.MinValue );
            if ( String.IsNullOrEmpty( value ) ) {
                return "-";
            } else {
                return FormatPercent( percent, decimalPlaces );
            }
        }

        /// <summary>
        /// Formats a string decimal into a percentage to the given number of decimal places.
        /// </summary>
        /// <param name="value">The decimal number as a string</param>
        /// <param name="decimalPlaces">The number of decimal places to show.</param>
        public static string FormatPercent( double percent, int decimalPlaces ) {
            if ( percent == Double.MinValue ) {
                return "-";
            } else {
                string dec = String.Empty;
                if ( decimalPlaces > 0 ) {
                    dec = "." + new String( '0', decimalPlaces );
                }
                string output = String.Format( "{0:0" + dec + "%}", percent );
                if ( percent <= 0 ) {
                    output = String.Format( "<span class='sub-zero'>{0}</span>", output );
                }
                return output;
            }
        }

        /// <summary>
        /// Formats a string decimal into an index value to one decimal place.
        /// </summary>
        /// <param name="value">The decimal number as a string</param>
        public static string FormatIndex( string value ) {
            return FormatIndex( value, 1 );
        }

        /// <summary>
        /// Formats a string decimal into an index value to the given number of decimal places.
        /// </summary>
        /// <param name="value">The decimal number as a string</param>
        /// <param name="decimalPlaces">The number of decimal places to show.</param>
        public static string FormatIndex( string value, int decimalPlaces ) {
            double indexVal = Conversion.StringToDbl( value, Double.MinValue );
            if ( String.IsNullOrEmpty( value ) || indexVal == Double.MinValue ) {
                return "-";
            } else {
                string dec = String.Empty;
                if ( decimalPlaces > 0 ) {
                    dec = "." + new String( '0', decimalPlaces );
                }
                string output = String.Format( "{0:0" + dec + "}", indexVal );
                if ( indexVal <= 0 ) {
                    output = String.Format( "<span class='sub-zero'>{0}</span>", output );
                }
                return output;
            }
        }

        /// <summary>
        /// Converts a number of minutes to a time value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string MinutesToNiceTime( string value ) {
            int mins = Convert.ToInt32( Conversion.StringToDbl( value, -100000 ) );
            if ( mins == -100000 ) {
                return "-";
            }
            if ( mins < 60 ) { //Less than an hour
                return String.Format( "{0}m", mins );
            } else if (mins < 1440) { //Less than a day
                int hrs = mins / 60;
                mins = mins % 60;
                return String.Format( "{0}h {1}m", hrs, mins );
            } else { // A day or more
                int days = mins / 1440;
                int hrs = ( mins % 1440 ) / 60;
                mins = mins % 60;
                return String.Format( "{0}d {1}h {2}m", days, hrs, mins );
            }
        }

        /// <summary>
        /// Converts a number of minutes to hours with decimals.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetNiceHours( string value ) {
            double hrs = Conversion.StringToDbl( value );
            if ( hrs <= 0 ) {
                return "-";
            } else {
                return String.Format( "{0:0.0}h", hrs );
            }
        }

        /// <summary>
        /// Converts a string to proper capitalized first letters (Used for Names)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetNiceName( string value)
        {           
            value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
            return value;
        }

        /// <summary>
        /// Calculates a percentage for B2B/MB/T2B scores from the given data row.
        /// </summary>
        /// <param name="dr">The datarow to look up.</param>
        /// <param name="question">The column question prefix.</param>
        /// <param name="scoreSuffix">The suffix that determines which score percentage is calculated.</param>
        public static string CalculatePercent( DataRow dr, string question, string scoreSuffix ) {
            double total =  dr[question + "_B2B"].ToString().StringToDbl() + 
                            dr[question + "_MB"].ToString().StringToDbl() + 
                            dr[question + "_T2B"].ToString().StringToDbl();
            if ( total == 0 ) {
                return "-";
            } else {
                double val = dr[question + scoreSuffix].ToString().StringToDbl();
                return FormatPercent( ( val / total ).ToString() );
            }
        }

        /// <summary>
        /// Adjusts a date based on the server and user's timezones.
        /// </summary>
        /// <param name="date">The date to be adjusted.</param>
        /// <param name="toServerTime">If true, the date will be adjusted from the User's timezone to the server's timezone. If false, vice versa.</param>
        /// <param name="user">The user making the request.</param>
        public static DateTime AdjustDateTime( DateTime date, bool toServerTime, UserInfo user ) {
            string timezone;
            if ( user == null ) {
                //Default user timezone is PST.
                timezone = "Pacific Standard Time";
            } else {
                timezone = user.Timezone;
            }
            return AdjustDateTime( date, toServerTime, timezone );
        }

        /// <summary>
        /// Adjusts a date based on the server and user's timezones.
        /// </summary>
        /// <param name="date">The date to be adjusted.</param>
        /// <param name="toServerTime">If true, the date will be adjusted from the given timezone to the server's timezone. If false, vice versa.</param>
        /// <param name="timezone">The timezone to adjust to/from.</param>
        public static DateTime AdjustDateTime( DateTime date, bool toServerTime, string timezone ) {
            //Default server timezone is EST.
            string sStartTZ = toServerTime ? timezone : "Eastern Standard Time";
            string sAdjustedTZ = toServerTime ? "Eastern Standard Time" : timezone;

            TimeZoneInfo startTZ = TimeZoneInfo.FindSystemTimeZoneById( sStartTZ );
            DateTime utcDateTime = date.Subtract( startTZ.BaseUtcOffset );

            TimeZoneInfo adjustedTZ = TimeZoneInfo.FindSystemTimeZoneById( sAdjustedTZ );
            return utcDateTime.Add( adjustedTZ.BaseUtcOffset );
        }


        /// <summary>
        /// Generates a date adjusted to the user's timezone. Returns empty string if the date is not valid.
        /// </summary>
        /// <param name="xmldatetime">The date time in XML/ISO8601 as a string. In SQL, use this: CONVERT(varchar(24), [Column], 126)</param>
        /// <param name="dateFormat">The output date format.</param>
        /// <param name="user">The current user.</param>
        public static string AdjustAndDisplayDate( string xmldatetime, ConversionDateFormatType dateFormat, UserInfo user ) {
            return AdjustAndDisplayDate( Conversion.XMLDateToDateTime( xmldatetime ), dateFormat, user );
        }
        /// <summary>
        /// Generates a date adjusted to the user's timezone. Returns empty string if the date is not valid.
        /// </summary>
        /// <param name="xmldatetime">The date time in XML/ISO8601 as a string. In SQL, use this: CONVERT(varchar(24), [Column], 126)</param>
        /// <param name="dateFormat">The output date format.</param>
        /// <param name="user">The current user.</param>
        public static string AdjustAndDisplayDate( DateTime date, ConversionDateFormatType dateFormat, UserInfo user ) {
            if ( date == DateTime.MinValue ) {
                return String.Empty;
            }
            return Conversion.TranslateDate( AdjustDateTime( date, false, user ), System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, dateFormat );
        }

        /// <summary>
        /// Generates a date adjusted to the user's timezone. Returns empty string if the date is not valid.
        /// </summary>
        /// <param name="xmldatetime">The date time in XML/ISO8601 as a string. In SQL, use this: CONVERT(varchar(24), [Column], 126)</param>
        /// <param name="dateFormat">The output date format.</param>
        /// <param name="user">The current user.</param>
        public static string AdjustAndDisplayDate( string xmldatetime, string dateFormat, UserInfo user ) {
            return AdjustAndDisplayDate( Conversion.XMLDateToDateTime( xmldatetime ), dateFormat, user );
        }

        /// <summary>
        /// Generates a date adjusted to the user's timezone. Returns empty string if the date is not valid.
        /// </summary>
        /// <param name="xmldatetime">The date time in XML/ISO8601 as a string. In SQL, use this: CONVERT(varchar(24), [Column], 126)</param>
        /// <param name="dateFormat">The output date format.</param>
        /// <param name="user">The current user.</param>
        public static string AdjustAndDisplayDate( DateTime date, string dateFormat, UserInfo user ) {
            if ( date == DateTime.MinValue ) {
                return String.Empty;
            }

            return AdjustDateTime( date, false, user ).ToString( dateFormat );
        }

        /// <summary>
        /// Takes an object and converts it to a string, HTML encodes it, and converts new lines to <br />'s.
        /// </summary>
        /// <param name="outputData">The data to output.</param>
        public static string CleanData( object outputData ) {
            if ( outputData == null ) {
                return String.Empty;
            } else {
                return System.Web.HttpContext.Current.Server.HtmlEncode( outputData.ToString() ).Replace( "\n", "<br />" );
            }
        }
    }
}