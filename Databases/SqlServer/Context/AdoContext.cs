using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Bpms.WorkflowEngine.Databases.SqlServer.Context
{
    public class AdoContext : IAdoContext
    {
        private string ConnectionString { get; set; }

        public AdoContext(string connectionString)
        {
            ConnectionString = connectionString;
          
        }

        public async Task<List<T>> GetAll<T>(string query)
        {
            await using (var connection = new SqlConnection(ConnectionString))
            {
                var res = (await connection.QueryAsync<T>(query)).ToList();
                return res;
            }
        }

        public async Task<List<dynamic>> GetAll(string query)
        {
            await using (var connection = new SqlConnection(ConnectionString))
            {
                var res = (await connection.QueryAsync(query)).ToList();
                return res;
            }
        }

        public async Task<SqlDataReader> ExecuteAsync(string query)
        {
            await using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand(query, connection);
                await connection.OpenAsync();
                var dataReader = await command.ExecuteReaderAsync();
                
                return dataReader;
            }
        }
    }
}