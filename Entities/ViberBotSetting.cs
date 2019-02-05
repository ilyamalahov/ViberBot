using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ViberBot.Entities
{
    [Table("dbo.ViberBot")]
    public class ViberBotSetting
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string AppKey { get; set; }
        public string ViberIncomeHost { get; set; }
        public XElement InvitationMessage { get; set; }
    }
}