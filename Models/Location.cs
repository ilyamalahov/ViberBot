using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ViberBot.Models
{
    [DataContract(Name = "location", Namespace = "")]
    public class Location
    {
        [DataMember(Name = "lon", Order = 1)]
        public double Lontitude { get; set; }
        
        [DataMember(Name = "lat", Order = 2)]
        public double Latitude { get; set; }
    }
}