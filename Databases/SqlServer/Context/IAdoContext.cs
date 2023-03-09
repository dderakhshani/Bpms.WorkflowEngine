using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Bpms.WorkflowEngine.Databases.SqlServer.Context
{
    public interface IAdoContext
    {
        Task<List<T>> GetAll<T>(string query);

        Task<SqlDataReader> ExecuteAsync(string query);
        Task<List<dynamic>> GetAll(string query);

    }
}