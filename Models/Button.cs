using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ViberBot.Models
{
    [DataContract(Name = "button", Namespace = "")]
    public class Button
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "style")]
        public ButtonStyle Style { get; set; }

        [DataMember(Name = "columns")]
        public int? Columns { get; set; }

        [DataMember(Name = "rows")]
        public int? Rows { get; set; }
    }
}