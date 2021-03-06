using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ViberBot.Entities
{
    [Table("dbo.Bot")]
    public class Bot
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string Title { get; set; }
        public string InvitationMessage { get; set; }
        public string AppKey { get; set; }
        public string Remark { get; set; }
        public XElement XmlInfo { get; set; }
    }
}