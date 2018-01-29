using System.Data;
using System.Data.SqlClient;

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
    }
}