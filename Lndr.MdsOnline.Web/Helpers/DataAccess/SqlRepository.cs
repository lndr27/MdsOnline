using Lndr.MdsOnline.Web.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Lndr.MdsOnline.Web.Helpers.DataAccess
{
    public class SqlRepository : IRepository
    {
        private string _connectionString;

        public SqlRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }        

        public IEnumerable<T> FindAll<T>(string sql, Action<SqlCommandFactory> action = null)
        {
            using (var connection = new SqlConnection(this._connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = sql;
                action?.Invoke(new SqlCommandFactory(command));

                using (var reader = command.ExecuteReader())
                {
                    var schemaTable = reader.GetSchemaTable();

                    while (reader.Read())
                    {
                        var result = (T)Activator.CreateInstance(typeof(T));

                        foreach (DataRow row in schemaTable.Rows)
                        {
                            try
                            {
                                var propertyName = (string)row["ColumnName"];
                                var propInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                                var value = reader[propertyName];
                                propInfo.SetValue(result, value == DBNull.Value ? null : value);
                            }
                            catch (Exception) { }
                        }
                        yield return result;
                    }
                }
            }
        }

        public T FindOne<T>(string sql, Action<SqlCommandFactory> action = null)
        {
            var result = this.FindAll<T>(sql, action);
            if (result.IsNullOrEmpty())
            {
                return default(T);
            }
            return result.SingleOrDefault();
        }

        public void ExecuteNonQuery(string sql, Action<SqlCommandFactory> action = null)
        {
            using (var connection = new SqlConnection(this._connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = sql;
                action?.Invoke(new SqlCommandFactory(command));
                command.ExecuteNonQuery();
            }
        }

        public T ExecuteScalar<T>(string sql, Action<SqlCommandFactory> action = null)
        {
            using (var connection = new SqlConnection(this._connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = sql;
                action?.Invoke(new SqlCommandFactory(command));
                return (T)command.ExecuteScalar();
            }
        }

        public IPagination<T> FindAll<T>(string sql, int pagina = 1, int tamanhoPagina= 10, Action<SqlCommandFactory> action = null)
        {
            Guard.ArgumentoForaDaFaixa("pagina", pagina, 1);
            Guard.ArgumentoForaDaFaixa("tamanoPagina", tamanhoPagina, 1);

            var offset = (pagina - 1) * tamanhoPagina;
            var sqlWrapper = string.Format(@"
SELECT *
FROM (
	SELECT *, ROW_NUMBER() OVER (ORDER BY (SELECT 1)) Row_ID
	FROM ({0})T1
) T2
WHERE Row_ID BETWEEN {1} AND {2}", sql, offset, (offset + tamanhoPagina));

            return new Pagination<T>()
            {
                Pagina = pagina,
                TotalPaginas = (int)Math.Ceiling(this.ObterTotalItens(sql, action) / (double)tamanhoPagina),
                Lista = this.FindAll<T>(sqlWrapper, action)
            };
        }

        private int ObterTotalItens(string sql, Action<SqlCommandFactory> action = null)
        {
            return this.ExecuteScalar<int>(string.Format(@"SELECT COUNT(1) Qtd FROM ({0}) T", sql), action);
        }
    }
}