using System;
using System.Collections.Generic;

namespace Lndr.MdsOnline.Web.Helpers.DataAccess
{
    public interface IRepository
    {
        IEnumerable<T> FindAll<T>(string sql, Action<SqlCommandFactory> action = null);

        void ExecuteNonQuery(string sql, Action<SqlCommandFactory> action = null);

        T ExecuteScalar<T>(string sql, Action<SqlCommandFactory> action = null);

        IPagination<T> FindAll<T>(string sql, int pagina = 1, int tamanhoPagina = 10, Action<SqlCommandFactory> action = null);
    }
}