using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ViberBot.Models
{
    [DataContract(Name = "msg", Namespace = "")]
    public class InMessage
    {        
        // [XmlElement("text")]
        [DataMember(Name="text")]
        public string Text { get; set; }
        
        // [XmlElement("location")]
        [DataMember(Name="location")]
        public Location Location { get; set; }

        // [XmlElement("pic")]
        [DataMember(Name="pic")]
        public string Picture { get; set; }
        
        // [XmlElement("media")]
        [DataMember(Name="media")]
        public string Video { get; set; }

        // [XmlElement("buttonmsgtoken")]
        [DataMember(Name="buttonmsgtoken")]
        public long? MessageToken { get; set; }

        // [XmlElement("buttonid")]
        [DataMember(Name="buttonid")]
        public int? ButtonId { get; set; }
    }
}