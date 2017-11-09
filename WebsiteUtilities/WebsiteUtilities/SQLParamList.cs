using System.Collections.Generic;
using System.Data.SqlClient;

namespace WebsiteUtilities {
    /// <summary>
    /// Allows for adding SqlParameters to a list using chaining.
    /// </summary>
    public class SQLParamList {
        List<SqlParameter> _list = new List<SqlParameter>();
        /// <summary>
        /// Adds a SqlParameter object to the list.
        /// </summary>
        /// <param name="name">The name of the parameter (including the @ sign).</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The modified SQLParamList object.</returns>
        public SQLParamList Add(string name, object value) {
            _list.Add(new SqlParameter(name, value));
            return this;
        }

        //Commenting this out for now. The parameters get unhooked from the sql command after it's run. They may lose their info.
        /// <summary>
        /// Adds a paramter with the direction specified as "Output".
        /// </summary>
        /// <param name="name">The name of the output parameter</param>
        /// <param name="parameterSize">The size of the parameter. For ints, this is 4. For varchar, it is the max length of the column.</param>
        /// <param name="outputParameterReference">A reference to the output parameter for use in retrieving the value.</param>
        /// <returns>The modified SQLParamList object.</returns>
        public SQLParamList AddOutputParam(string name, int parameterSize, out SqlParameter outputParameterReference) {
            outputParameterReference = new SqlParameter();
            outputParameterReference.Direction = System.Data.ParameterDirection.Output;
            outputParameterReference.ParameterName = name;
            outputParameterReference.Size = parameterSize;
            _list.Add(outputParameterReference);
            return this;
        }

        /// <summary>
        /// Adds a SqlParameter to the list.
        /// </summary>
        /// <param name="param">The SqlParam to add to the list.</param>
        public SQLParamList Add(SqlParameter param) {
            _list.Add(param);
            return this;
        }
        /// <summary>
        /// Retrieves the SqlParameter array.
        /// </summary>
        /// <returns>An array of SqlParameter objects.</returns>
        public SqlParameter[] ToArray() {
            return _list.ToArray();
        }

        /// <summary>
        /// Returns the number of items currently in the parameter list.
        /// </summary>
        public int Count {
            get {
                return _list.Count;
            }
        }
    }
}
