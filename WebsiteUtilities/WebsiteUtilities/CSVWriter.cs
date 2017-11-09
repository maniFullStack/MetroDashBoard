//Code from: http://www.codeproject.com/Articles/415732/Reading-and-Writing-CSV-Files-in-Csharp
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WebsiteUtilities {
    /// <summary>
    /// Class to store one CSV row
    /// </summary>
    public class CSVRow : List<string> {
        /// <summary>
        /// The text of the line. Set after the row has been written.
        /// </summary>
        public string LineText { get; set; }
    }

    /// <summary>
    /// Class to write data to a CSV file
    /// </summary>
    public class CSVWriter : StreamWriter {
        /// <summary>
        /// Creates a new CSVWriter object that will write to the underlying stream.
        /// </summary>
        /// <param name="stream"></param>
        public CSVWriter(Stream stream)
            : base(stream) {
        }

        /// <summary>
        /// Creates a new CSVWriter object that will write to the passed file.
        /// </summary>
        /// <param name="filename"></param>
        public CSVWriter(string filename)
            : base(filename) {
        }

        /// <summary>
        /// Writes a single row to a CSV file.
        /// </summary>
        /// <param name="row">The row to be written</param>
        public void WriteRow(CSVRow row) {
            StringBuilder builder = new StringBuilder();
            bool firstColumn = true;
            foreach (string value in row) {
                // Add separator if this isn't the first value
                if (!firstColumn)
                    builder.Append(',');
                // Implement special handling for values that contain comma, quote or new line characters
                // Enclose in quotes and double up any double quotes
                if (value.IndexOfAny(new char[] { '"', ',', (char) 10, (char) 13 }) != -1) {
                    builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                } else {
                    builder.Append(value);
                }
                firstColumn = false;
            }
            row.LineText = builder.ToString();
            WriteLine(row.LineText);
        }
    }


}
