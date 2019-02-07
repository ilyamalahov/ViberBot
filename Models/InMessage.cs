using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ViberBot.Models
{
    [DataContract(Namespace = "")]
    public class InMessage:BaseMessage
    {
        [DataMember(Name = "pic")]
        public string Picture { get; set; }

        [DataMember(Name = "media")]
        public string Video { get; set; }

        [DataMember(Name = "buttonmsgtoken")]
        public long? MessageToken { get; set; }

        [DataMember(Name = "buttonid")]
        public int? ButtonId { get; set; }
    }
}