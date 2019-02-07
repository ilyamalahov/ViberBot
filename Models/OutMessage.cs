using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ViberBot.Models
{
    [DataContract(Namespace="")]
    public class OutMessage
    {        
        [DataMember(Name = "text", Order = 1)]
        public string Text { get; set; }

        [DataMember(Name = "pic", Order = 2)]
        public string Picture { get; set; }

        [DataMember(Name = "location", Order = 3)]
        public Location Location { get; set; }

        [DataMember(Name = "buttonplace", Order = 4)]
        public PlaceType? ButtonPlace { get; set; }

        [DataMember(Name = "buttonlist", Order = 5)]
        public ICollection<Button> Buttons { get; set; }
    }
}