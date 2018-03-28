﻿using Lndr.MdsOnline.Web.Helpers.Extensions;
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
    }
}