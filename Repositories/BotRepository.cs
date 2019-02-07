using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml;
using Dapper;
using ViberBot.Entities;

namespace ViberBot.Repositories
{
    public class BotRepository : IBotRepository
    {
        private readonly string connectionString;

        public BotRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<IEnumerable<Bot>> GetAll()
        {
            var sql = "SELECT * FROM dbo.Bot";

            using(var connection = new SqlConnection(connectionString))
            {
                return await connection.QueryAsync<Bot>(sql);
            }
        }

        public async Task<Bot> GetById(int id)
        {
            var sql = "SELECT * FROM dbo.Bot WHERE Id = @id";

            using(var connection = new SqlConnection(connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<Bot>(sql, new { id });
            }
        }
    }

    public interface IBotRepository
    {
        Task<Bot> GetById(int id);
        Task<IEnumerable<Bot>> GetAll();
    }
}