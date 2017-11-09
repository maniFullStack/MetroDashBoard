using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WebsiteUtilities {
    public static class ExtensionMethods {
        /// <summary>
        /// Truncates a string to the specified max length. If the string is shorter than the max length, it will be returned as is.
        /// </summary>
        /// <param name="value">The string to truncate.</param>
        /// <param name="maxLength">The maximum number of characters you want in the string.</param>
        /// <returns>A truncated version of the string if applicable.</returns>
        public static string Truncate(this string value, int maxLength) {
            if (value == null) { return null; }
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

    }

    public static class ConversionExtensionMethods
    {

        public static IEnumerable<DataRow> ToRows(this DataTable dt)
        {
            return dt.Rows.Cast<DataRow>();
        }

        public static IEnumerable<DataRow> ToRows(this DataTable dt, Func<DataRow, bool> query )
        {
            return ToRows(dt).Where(query);
        }

        public static DataTable Where(this DataTable dt, Func<DataRow, bool> query)
        {
            return dt.ToRows(query).CopyToDataTable();
        }

        public static string DefaultIfEmpty(this string str, string defaultValue)
        {
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }
            return str;
        }

        public static string TryGetField(this DataRow dr, string name)
        {
            if (!dr.Table.Columns.Contains(name))
            {
                return string.Format("Key {0} not found", name);
            }
            return dr[name].ToString();
        }


        public static string TryGetField(this DataRow dr, string name, string defaultValue)
        {
            if (!dr.Table.Columns.Contains(name))
            {
                return defaultValue;
            }

            return dr[name].ToString();

        }

    }


}
