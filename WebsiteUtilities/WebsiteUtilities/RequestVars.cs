using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Sockets;

namespace WebsiteUtilities {
    public class RequestVars {
        /// <summary>
        /// Returns a value as a specific type from the query string or the default value if it can't be parsed.
        /// </summary>
        /// <typeparam name="T">The type that the object should be.</typeparam>
        /// <param name="qsid">The name corresponding to the query string value you are trying to get.</param>
        /// <param name="defaultvalue">The default value to return. (ie. In case it doesn't exist or there is an error converting.)</param>
        /// <returns>The value of the query string variable, or the default value on error/bad input.</returns>
        public static T Get<T>(string qsid, T defaultvalue) where T : IConvertible {
            object qsobject = HttpContext.Current.Request.QueryString[qsid];
            if (qsobject == null) { return defaultvalue; }

            try {
                if (typeof(int) == typeof(T)) {
                    return (T)Convert.ChangeType(Conversion.StringToInt(qsobject.ToString(), defaultvalue.ToInt32(null)), typeof(int));
                } else {
                    return (T)Convert.ChangeType(qsobject, typeof(T));
                }
            } catch {
                return defaultvalue;
            }
        }

        /// <summary>
        /// Returns a value as a specific type from the query string or the default value if it can't be parsed and uses Server.HtmlEncode on it before returning it. Useful for form input fields.
        /// </summary>
        /// <param name="qsid">The name corresponding to the query string value you are trying to get.</param>
        /// <param name="defaultvalue">The default value to return. (ie. In case it doesn't exist or there is an error converting.)</param>
        public static string GetEncoded(string qsid, string defaultvalue) {
            return HttpContext.Current.Server.HtmlEncode(Get<string>(qsid, defaultvalue));
        }


        /// <summary>
        /// Returns a value as a specific Type from form POST data or the default value if it cannot be parsed.
        /// </summary>
        /// <typeparam name="T">The type that the object should be.</typeparam>
        /// <param name="qsid">The name corresponding to the form object value you are trying to get.</param>
        /// <param name="defaultvalue">The default value to return. (ie. In case it doesn't exist or there is an error converting.)</param>
        /// <returns>The value of the POST string variable, or the default value on error/bad input.</returns>
        public static T Post<T>(string qsid, T defaultvalue) where T : IConvertible {
            object qsobject = HttpContext.Current.Request.Form[qsid];
            if (qsobject == null) { return defaultvalue; }

            try {
                if (typeof(int) == typeof(T)) {
                    return (T)Convert.ChangeType(Conversion.StringToInt(qsobject.ToString(), defaultvalue.ToInt32(null)), typeof(int));
                } else {
                    return (T)Convert.ChangeType(qsobject, typeof(T));
                }
            } catch {
                return defaultvalue;
            }
        }
        /// <summary>
        /// Returns a value as a specific Type from form POST data or the default value if it cannot be parsed and encodes it using Server.HtmlEncode. Useful for form input fields.
        /// </summary>
        /// <param name="qsid">The name corresponding to the form object value you are trying to get.</param>
        /// <param name="defaultvalue">The default value to return. (ie. In case it doesn't exist or there is an error converting.)</param>
        /// <returns></returns>
        public static string PostEncoded(string qsid, string defaultvalue) {
            return HttpContext.Current.Server.HtmlEncode(Post<string>(qsid, defaultvalue));
        }

        /// <summary>
        /// Attempts to find and return the IPv4 address of the current address.
        /// </summary>
        /// <returns>A string containing the IP address, "No IPv4 address found" if no address was found or "Unknown" if something went terribly wrong.</returns>
        public static string GetRequestIPv4Address() {
            try {
                string IP = String.Empty;
                foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress)) {
                    if (IPA.AddressFamily == AddressFamily.InterNetwork) {
                        IP = IPA.ToString();
                        break;
                    }
                }
                if (String.IsNullOrEmpty(IP)) {
                    IP = "No IPv4 address found";
                }
                return IP;
            } catch {
                return "Unknown";
            }
        }
    }
}
