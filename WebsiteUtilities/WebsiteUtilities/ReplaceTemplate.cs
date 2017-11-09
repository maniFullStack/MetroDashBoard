using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;

namespace WebsiteUtilities {
    /// <summary>
    /// This class is used to read a template file and replace specific key phrases with information.
    /// </summary>
    public class ReplaceTemplate {
        private struct ReplaceInfo {
            public string[] Values;
            public Func<string, string, int, string> OutputFunction;
        }

        private Dictionary<string, string> _replaceValues = new Dictionary<string, string>();
        private Dictionary<string, ReplaceInfo> _replaceInfoValues = new Dictionary<string, ReplaceInfo>();
        /// <summary>
        /// The content loaded from the template file. Note: No values have been replaced.
        /// </summary>
        public string TemplateContent { get; private set; }

        /// <summary>
        /// Adds a value to be replaced in the template.
        /// 
        /// Throws ArgumentException.
        /// </summary>
        /// <param name="key">The value to be replaced.</param>
        /// <param name="value">The value to replace it with.</param>
        public void AddReplacementValue(string key, string value) {
            if (key == null) {
                throw new ArgumentException("The key parameter cannot be null.");
            }
            if (value == null) {
                throw new ArgumentException("The values parameter cannot be null.");
            }
            _replaceValues.Add(key, value);
        }

        /// <summary>
        /// Adds a value to be replaced in the template with the ability to customize the output using a function.
        /// 
        /// Throws ArgumentException.
        /// </summary>
        /// <param name="key">The value to be replaced.</param>
        /// <param name="values">The array value to be replaced.</param>
        /// <param name="customReplaceFunction">The function to be run when replacing. Will be passed the key and value and should return the final output string.</param>
        public void AddReplacementValue(string key, string[] values, Func<string, string, int, string> replaceFunction) {
            if (key == null) {
                throw new ArgumentException("The key parameter cannot be null.");
            }
            if (values == null) {
                throw new ArgumentException("The values parameter cannot be null.");
            }
            if (replaceFunction == null) {
                throw new ArgumentException("The replace function parameter cannot be null.");
            }
            ReplaceInfo repl;
            repl.OutputFunction = replaceFunction;
            repl.Values = values;
            _replaceInfoValues.Add(key, repl);
        }

        /// <summary>
        /// Creates a new Template and loads the content from the passed file.
        /// </summary>
        /// <param name="templateFile">The path to the file that contains the content for this template.</param>
        public ReplaceTemplate(string templateFile) {
            if (!File.Exists(templateFile)) {
                templateFile = HttpContext.Current.Server.MapPath(templateFile);
                if (!File.Exists(templateFile)) {
                    throw new FileNotFoundException("Invalid template file specified. The template file does not exist.");
                }
            }
            try {
                TemplateContent = String.Join("\n", File.ReadAllLines(templateFile));
            } catch (Exception) {
                throw;
            }
        }

        /// <summary>
        /// Gets the template and replaces the keys from ReplaceValues.
        /// </summary>
        /// <returns></returns>
        public string GetTemplate() {
            return GetTemplate("{", "}");
        }
        /// <summary>
        /// Gets the template and replaces the string
        /// </summary>
        /// <param name="beginDelimitator">The delimiter that begins the replacement string in the template. Defaults to "{".</param>
        /// <param name="endDelimitator">The delimiter that ends the replacement string in the template. Defaults to "}".</param>
        /// <returns></returns>
        public string GetTemplate(string beginDelimitator, string endDeliminator) {
            StringBuilder sb = new StringBuilder(TemplateContent);
            foreach (string key in _replaceValues.Keys) {
                sb.Replace(beginDelimitator + key + endDeliminator, _replaceValues[key]);
            }
            foreach (string key in _replaceInfoValues.Keys) {
                ReplaceInfo repl = _replaceInfoValues[key];
                if (repl.OutputFunction != null) {
                    StringBuilder sb2 = new StringBuilder();
                    for (int i = 0; i < repl.Values.Length; i++){
                        sb2.Append(repl.OutputFunction(key, repl.Values[i], i));
                    }
                    sb.Replace(beginDelimitator + key + endDeliminator, sb2.ToString());
                }
            }
            return sb.ToString();
        }
    }
}
