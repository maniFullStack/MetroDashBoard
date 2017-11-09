using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

namespace WebsiteUtilities {
    public class SessionWrapper {
        /// <summary>
        /// Retrieves an element of the type specified from the session (if it exists). Returns the default value of the type if there's an error or if it doesn't exist.
        /// </summary>
        /// <typeparam name="T">The type of object to get from the session.</typeparam>
        /// <param name="key">The session index name.</param>
        /// <returns>Returns the value of the session variable or the default value of the type if there's an error or if it doesn't exist.</returns>
        public static T Get<T>(string key) {
            return Get<T>(key, default(T));
        }
        /// <summary>
        /// Retrieves an element of the type specified from the session (if it exists). Returns the default value passed if there's an error or if it doesn't exist.
        /// </summary>
        /// <typeparam name="T">The type of object to get from the session.</typeparam>
        /// <param name="key">The session index name.</param>
        /// <param name="defaultValue">The default value to return if something is wrong.</param>
        /// <returns>Returns the value of the session variable or the default value of the type if there's an error or if it doesn't exist.</returns>
        public static T Get<T>(string key, T defaultValue) {
            HttpContext current = HttpContext.Current;
            if (current == null) {
                return defaultValue;
            }

            if (current.Session.Mode == SessionStateMode.Off) {
                throw new Exception("Session elements cannot be retrieved when session is disabled.");
            }

            object value = current.Session[key];
            if (value != null) {
                try {
                    return (T)value;
                } catch (InvalidCastException) {
                    if (typeof(T) == typeof(int)) {
                        int temp = Conversion.StringToInt(value.ToString(), -9999);
                        if (temp != -9999) {
                            return (T)Convert.ChangeType(temp, typeof(int));
                        }
                    }
                    //throw new InvalidCastException();
                }
            }
            return defaultValue;
        }
        /// <summary>
        /// This method will add or overwrite a key value pair currently in the session.
        /// </summary>
        /// <typeparam name="T">The type of object to add to the session.</typeparam>
        /// <param name="key">The string representing the object in the session.</param>
        /// <param name="value">The value to be stored.</param>
        /// <returns>True if successful; else false.</returns>
        public static bool Add<T>(string key, T value) {
            HttpContext current = HttpContext.Current;
            if (current == null || current.Session == null) {
                return false;
            }
            if (current.Session.Mode == SessionStateMode.Off) {
                throw new Exception("Session elements cannot be added when session is disabled.");
            }
            if (current.Session[key] != null) {
                Remove(key);
            }
            current.Session.Add(key, value);
            return true;
        }

        /// <summary>
        /// Removes the element with the given key from the session.
        /// </summary>
        /// <param name="key">The element to remove from the session.</param>
        public static bool Remove(string key) {
            HttpContext current = HttpContext.Current;
            if (current == null) {
                return false;
            }
            if (current.Session.Mode == SessionStateMode.Off) {
                throw new Exception("Session elements cannot be removed when session is disabled.");
            }
            current.Session.Remove(key);
            return true;
        }

        /// <summary>
        /// Returns true if an element with the passed key exists in the session.
        /// </summary>
        /// <param name="key">The key to find in the session dictionary.</param>
        public static bool Exists(string key) {
            HttpContext current = HttpContext.Current;
            if (current == null) {
                return false;
            }
            if (current.Session.Mode == SessionStateMode.Off) {
                throw new Exception("Session elements cannot be removed when session is disabled.");
            }
            return (current.Session[key] != null);
        }

        /// <summary>
        /// Clears all session elements with the specified prefix.
        /// </summary>
        /// <param name="prefix">The string prefixed onto the beginning of the session key.</param>
        /// <returns>true if HttpContext exists and no errors</returns>
        public static bool ClearSessionElementsWithPrefix(string prefix) {
            HttpContext current = HttpContext.Current;
            if (current == null) {
                return false;
            }
            if (current.Session.Mode == SessionStateMode.Off) {
                throw new Exception("Session elements cannot be removed when session is disabled.");
            }
            //We have to create a remove List here because we can't modify the session collection while in the foreach
            List<string> toBeRemoved = new List<string>();
            foreach (string key in current.Session.Keys) {
                if (key.StartsWith(prefix)) {
                    toBeRemoved.Add(key);
                }
            }
            foreach (string key in toBeRemoved) {
                Remove(key);
            }
            return true;
        }
    }
}