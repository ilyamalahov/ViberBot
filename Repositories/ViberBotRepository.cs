using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml;
using Dapper;
using ViberBot.Entities;

namespace ViberBot.Repositories
{
    public class ViberBotRepository : IViberBotRepository
    {
        private readonly string connectionString;

        public ViberBotRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<IEnumerable<ViberBotSetting>> GetAll()
        {
            var sql = "SELECT * FROM dbo.ViberBot";

            using(var connection = new SqlConnection(connectionString))
            {
                return await connection.QueryAsync<ViberBotSetting>(sql);
            }
        }

        public async Task<ViberBotSetting> GetById(int id)
        {
            var sql = "SELECT * FROM dbo.ViberBot WHERE Id = @id";

            using(var connection = new SqlConnection(connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<ViberBotSetting>(sql, new { id });
            }
        }
    }

    public interface IViberBotRepository
    {
        Task<ViberBotSetting> GetById(int id);
        Task<IEnumerable<ViberBotSetting>> GetAll();
    }
}