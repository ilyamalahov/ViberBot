using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ViberBot.Models
{
    [DataContract(Name = "msg", Namespace = "")]
    public class InMessage
    {
        [DataMember(Name = "text", Order = 1)]
        public string Text { get; set; }

        [DataMember(Name = "pic", Order = 2)]
        public string Picture { get; set; }

        [DataMember(Name = "media", Order = 3)]
        public string Video { get; set; }

        [DataMember(Name = "location", Order = 4)]
        public Location Location { get; set; }

        [DataMember(Name = "buttonmsgtoken", Order = 5)]
        public long? MessageToken { get; set; }

        [DataMember(Name = "buttonid", Order = 6)]
        public int? ButtonId { get; set; }
    }
}