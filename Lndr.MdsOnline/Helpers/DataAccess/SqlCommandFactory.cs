using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Lndr.MdsOnline.Helpers.DataAccess
{
    public class SqlCommandFactory
    {
        private SqlCommand _command;

        public SqlCommandFactory(SqlCommand command)
        {
            this._command = command;
        }

        public void SetCommandType(CommandType commandType)
        {
            this._command.CommandType = commandType;
        }

        public SqlParameter AddWithValue(string parameterName, object value)
        {
            return this._command.Parameters.AddWithValue(parameterName, value);
        }

        public void AddWithValues<T>(string parameterName, IEnumerable<T> values)
        {
            var paramNames = new List<string>();
            for (int i = 0; i < values.Count(); ++i)
            {
                var paramName = string.Format("@Param{0}", i);
                paramNames.Add(paramName);
                this._command.Parameters.AddWithValue(paramName, values.ElementAt(i));
            }
            this._command.CommandText = this._command.CommandText.ToLower().Replace(parameterName.ToLower(), string.Join(",", paramNames));
        }
    }
}