using System;
using System.Text;
using System.Collections.Generic;

namespace WebsiteUtilities {
    public class JSONBuilder {
        /// <summary>
        /// An internal type for handling the currently open type of object.
        /// </summary>
        private enum InternalType {
            None,
            Object,
            Array
        }
        /// <summary>
        /// The JSON string builder.
        /// </summary>
        StringBuilder _jsonsb;

        /// <summary>
        /// The type depth stack that ensures the user is calling things properly.
        /// </summary>
        Stack<InternalType> _typeDepth = new Stack<InternalType>();
        
        /// <summary>
        /// Creates an empty JSON object.
        /// </summary>
        public JSONBuilder() : this(false) { }
        /// <summary>
        /// Creates an empty JSON object or array depending on the value of <paramref name="isArray"/>.
        /// </summary>
        /// <param name="isArray">If true, it will create an empty JSON array.</param>
        public JSONBuilder(bool isArray) {
            if (isArray) {
                _jsonsb = new StringBuilder("[");
                _typeDepth.Push(InternalType.Array);
            } else {
                _jsonsb = new StringBuilder("{");
                _typeDepth.Push(InternalType.Object);
            }
        }

        /// <summary>
        /// Adds a name value pair where the value is of type string to the current object.
        /// </summary>
        /// <param name="name">The name in the name value pair.</param>
        /// <param name="str">The string value of the name value pair.</param>
        public JSONBuilder AddString(string name, string str) {
            if (str == null) {
                str = "null";
            } else {
                str = "\"" + str.Replace(@"\", @"\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "").Replace("\t", "\\t") + "\"";
            }
            if (_jsonsb[_jsonsb.Length - 1].Equals('{') || _jsonsb[_jsonsb.Length - 1].Equals('[')) {
                _jsonsb.Append("\"").Append(name).Append("\":").Append(str);
            } else {
                _jsonsb.Append(",\"").Append(name).Append("\":").Append(str);
            }

            return this;
        }

        /// <summary>
        /// Adds a value of type string to the current object. This can only be used while in an array.
        /// </summary>
        /// <param name="str">The string value to add.</param>
        public JSONBuilder AddString(string str) {
            if (_typeDepth.Peek() == InternalType.Array) {
                if (str == null) {
                    str = "null";
                } else {
                    str = "\"" + str.Replace( @"\" , @"\\" ).Replace( "\"" , "\\\"" ).Replace( "\n" , "\\n" ).Replace( "\r" , "" ) + "\"";
                }
                if (_jsonsb[_jsonsb.Length - 1].Equals('{') || _jsonsb[_jsonsb.Length - 1].Equals('[')) {
                    _jsonsb.Append(str);
                } else {
                    _jsonsb.Append(",").Append(str);
                }
                return this;
            } else {
                throw new ApplicationException("You cannot add a single string to a JSON object (requires name/value pairs), only to an array. Call the AddArray() method first or use AddString(string, string).");
            }
        }

        /// <summary>
        /// Adds a name value pair where the value is of type int to the current object.
        /// </summary>
        /// <param name="name">The name in the name value pair.</param>
        /// <param name="num">The int value of the name value pair.</param>
        public JSONBuilder AddInt(string name, int num) {
            if (_jsonsb[_jsonsb.Length - 1].Equals('{') || _jsonsb[_jsonsb.Length - 1].Equals('[')) {
                _jsonsb.Append("\"").Append(name).Append("\":").Append(num);
            } else {
                _jsonsb.Append(",\"").Append(name).Append("\":").Append(num);
            }
            return this;
        }

        /// <summary>
        /// Adds a value of type int to the current object. This can only be used while in an array.
        /// </summary>
        /// <param name="num">The int value to add.</param>
        public JSONBuilder AddInt(int num) {
            if (_typeDepth.Peek() == InternalType.Array) {
                if (_jsonsb[_jsonsb.Length - 1].Equals('{') || _jsonsb[_jsonsb.Length - 1].Equals('[')) {
                    _jsonsb.Append(num);
                } else {
                    _jsonsb.Append(",").Append(num);
                }
                return this;
            } else {
                throw new ApplicationException("You cannot add a single int to a JSON object (requires name/value pairs), only to an array. Call the AddArray() method first or use AddInt(string, int).");
            }
        }

        /// <summary>
        /// Adds a name value pair where the value is of type bool to the current object.
        /// </summary>
        /// <param name="name">The name in the name value pair.</param>
        /// <param name="val">The boolean value of the name value pair.</param>
        public JSONBuilder AddBool(string name, bool val) {
            if (_jsonsb[_jsonsb.Length - 1].Equals('{') || _jsonsb[_jsonsb.Length - 1].Equals('[')) {
                _jsonsb.Append("\"").Append(name).Append("\":").Append(val ? "true" : "false");
            } else {
                _jsonsb.Append(",\"").Append(name).Append("\":").Append(val ? "true" : "false");
            }
            return this;
        }

        /// <summary>
        /// Adds a value of type int to the current object. This can only be used while in an array.
        /// </summary>
        /// <param name="val">The boolean value to add.</param>
        public JSONBuilder AddBool(bool val) {
            if (_typeDepth.Peek() == InternalType.Array) {
                if (_jsonsb[_jsonsb.Length - 1].Equals('{') || _jsonsb[_jsonsb.Length - 1].Equals('[')) {
                    _jsonsb.Append(val ? "true" : "false");
                } else {
                    _jsonsb.Append(",").Append(val ? "true" : "false");
                }
                return this;
            } else {
                throw new ApplicationException("You cannot add a single bool to a JSON object (requires name/value pairs), only to an array. Call the AddArray() method first or use AddBool(string, bool).");
            }
        }

        /// <summary>
        /// Adds a value of type float to the current object.
        /// </summary>
        /// <param name="name">The name of the float.</param>
        /// <param name="num">The float value to add.</param>
        public JSONBuilder AddFloat(string name, float num) {
            if (_jsonsb[_jsonsb.Length - 1].Equals('{') || _jsonsb[_jsonsb.Length - 1].Equals('[')) {
                _jsonsb.Append("\"").Append(name).Append("\":").Append(num);
            } else {
                _jsonsb.Append(",\"").Append(name).Append("\":").Append(num);
            }
            return this;
        }

        /// <summary>
        /// Adds a value of type float to an array. This can only be used while in an array.
        /// </summary>
        /// <param name="num">The float value to add.</param>
        public JSONBuilder AddFloat(float num) {
            if (_typeDepth.Peek() == InternalType.Array) {
                if (_jsonsb[_jsonsb.Length - 1].Equals('{') || _jsonsb[_jsonsb.Length - 1].Equals('[')) {
                    _jsonsb.Append(num);
                } else {
                    _jsonsb.Append(",").Append(num);
                }
                return this;
            } else {
                throw new ApplicationException("You cannot add a single float to a JSON object (requires name/value pairs), only to an array. Call the AddArray() method first or use AddFloat(string, float).");
            }
        }

        /// <summary>
        /// This adds a named object that has already been created. This does not need to be closed. <paramref name="obj"/> should be a validly formatted JSON object or array.
        /// </summary>
        /// <param name="name">The name of the object to add.</param>
        /// <param name="obj">A validly formatted JSON object or array to add.</param>
        public JSONBuilder AddObject(string name, string obj) {
            if (_jsonsb[_jsonsb.Length - 1].Equals('{') || _jsonsb[_jsonsb.Length - 1].Equals('[')) {
                _jsonsb.Append("\"").Append(name).Append("\":").Append(obj);
            } else {
                _jsonsb.Append(",\"").Append(name).Append("\":").Append(obj);
            }
            return this;
        }
        /// <summary>
        /// This starts a named object.
        /// </summary>
        /// <param name="name">The name of the object to add.</param>
        public JSONBuilder AddObject(string name) {
            if (_jsonsb[_jsonsb.Length - 1].Equals('{') || _jsonsb[_jsonsb.Length - 1].Equals('[')) {
                _jsonsb.Append("\"").Append(name).Append("\":{");
            } else {
                _jsonsb.Append(",\"").Append(name).Append("\":{");
            }
            _typeDepth.Push(InternalType.Object);
            return this;
        }
        /// <summary>
        /// This starts an unnamed object for use in an array.
        /// </summary>
        public JSONBuilder AddObject() {
            if (_typeDepth.Peek() == InternalType.Array) {
                if (_jsonsb[_jsonsb.Length - 1].Equals('{') || _jsonsb[_jsonsb.Length - 1].Equals('[')) {
                    _jsonsb.Append("{");
                } else {
                    _jsonsb.Append(",{");
                }
                _typeDepth.Push(InternalType.Object);
                return this;
            } else {
                throw new ApplicationException("You cannot add a single object to a JSON object (requires name/value pairs), only to an array. Call the AddArray() method first or use AddObject(string).");
            }
        }

        /// <summary>
        /// This closes an object that has been opened using AddObject() or AddObject(string).
        /// </summary>
        public JSONBuilder CloseObject() {
            if (_typeDepth.Peek() == InternalType.Object && _typeDepth.Count > 1) {
                _jsonsb.Append("}");
                _typeDepth.Pop();
                return this;
            } else {
                throw new ApplicationException("You cannot close an object if you are not currently working with an object. Either close the array first (if open) or open an object.");
            }
        }

        /// <summary>
        /// This starts a named array.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JSONBuilder AddArray(string name) {
            if (_jsonsb[_jsonsb.Length - 1].Equals('{') || _jsonsb[_jsonsb.Length - 1].Equals('[')) {
                _jsonsb.Append("\"").Append(name).Append("\":[");
            } else {
                _jsonsb.Append(",\"").Append(name).Append("\":[");
            }
            _typeDepth.Push(InternalType.Array);
            return this;
        }
        /// <summary>
        /// Starts a nested array. Can only be used within an already open array.
        /// </summary>
        public JSONBuilder AddArray() {
            if (_typeDepth.Peek() == InternalType.Array) {
                if (_jsonsb[_jsonsb.Length - 1].Equals('{') || _jsonsb[_jsonsb.Length - 1].Equals('[')) {
                    _jsonsb.Append("[");
                } else {
                    _jsonsb.Append(",[");
                }
                _typeDepth.Push(InternalType.Array);
                return this;
            } else {
                throw new ApplicationException("You cannot add a single array to a JSON object (requires name/value pairs), only to an array. Call the AddArray(string) method first.");
            }
        }

        /// <summary>
        /// Closes a currently open array.
        /// </summary>
        public JSONBuilder CloseArray() {
            if (_typeDepth.Peek() == InternalType.Array && _typeDepth.Count > 1) {
                _jsonsb.Append("]");
                _typeDepth.Pop();
                return this;
            } else {
                throw new ApplicationException("You cannot close an array if you are not currently working with an array. Either close the object first (if open) or open an array.");
            }
        }

        /// <summary>
        /// Outputs the final JSON string.
        /// </summary>
        public override string ToString() {
            if (_typeDepth.Count > 1) {
                throw new ApplicationException("Not all arrays/objects have been properly closed. Please close all open objects/arrays properly before calling ToString().");
            }
            if (_typeDepth.Peek() == InternalType.Array) {
                return _jsonsb.ToString() + "]";
            } else {
                return _jsonsb.ToString() + "}";
            }
        }
    }
}