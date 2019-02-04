using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using System.Xml;
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
            return await Task.FromResult<IEnumerable<ViberBotSetting>>(null);
        }

        public async Task<ViberBotSetting> GetById(int id)
        {
            var sql = @"SELECT * FROM dbo.ViberBot WHERE Id = @id";

            return await Task.FromResult<ViberBotSetting>(null);
        }
    }

    public interface IViberBotRepository
    {
        Task<ViberBotSetting> GetById(int id);
        Task<IEnumerable<ViberBotSetting>> GetAll();
    }
}

namespace ViberBot.Entities
{
    [Table("dbo.ViberBot")]
    public class ViberBotSetting
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string AppKey { get; set; }
        public string ViberIncomeHost { get; set; }
        public XmlElement InvitationMessage { get; set; }
    }
}