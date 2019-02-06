using System.Xml.Serialization;

namespace ViberBot.Models
{
    public class Button
    {
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("style")]
        public ButtonStyle Style { get; set; }

        [XmlElement("columns")]
        public int? Columns { get; set; }

        [XmlElement("rows")]
        public int? Rows { get; set; }
    }
}