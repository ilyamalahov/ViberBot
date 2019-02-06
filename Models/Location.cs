using System.Xml.Serialization;

namespace ViberBot.Models
{
    public class Location
    {
        [XmlElement("lon")]
        public double Lontitude { get; set; }
        
        [XmlElement("lat")]
        public double Latitude { get; set; }
    }
}