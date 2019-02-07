using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ViberBot.Models
{
    // [XmlRoot("payload")]
    [DataContract(Name = "model", Namespace = "")]
    public class MessageModel<T> where T: class
    {
        // [XmlElement("botId")]
        [DataMember(Name = "botId")]
        public int BotId { get; set; }

        [DataMember(Name = "messageId")]
        public Guid AgentId { get; set; }

        // [XmlElement("msg")]
        [DataMember(Name = "msg")]
        public T Message { get; set; }
    }
}