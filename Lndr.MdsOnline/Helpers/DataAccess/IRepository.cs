using System;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Helpers.DataAccess
{
    public interface IRepository
    {
        IEnumerable<T> FindAll<T>(string sql, Action<SqlCommandFactory> action = null);

        void ExecuteNonQuery(string sql, Action<SqlCommandFactory> action = null);

        T ExecuteScalar<T>(string sql, Action<SqlCommandFactory> action = null);
    }
}