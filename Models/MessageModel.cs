using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ViberBot.Models
{
    [DataContract(Name = "payload", Namespace = "")]
    public class PayloadModel<T>
    {
        [DataMember(Name = "botId", Order = 1)]
        public int BotId { get; set; }

        [DataMember(Name = "agentId", Order = 2)]
        public Guid AgentId { get; set; }

        [DataMember(Name = "msg", Order = 3)]
        public T Message { get; set; }
    }

    [DataContract(Namespace = "")]
    public enum Size
    {
        [EnumMember(Value = "small")]
        Small,
        [EnumMember(Value = "regular")]
        Regular,
        [EnumMember(Value = "large")]
        Large
    }

    [DataContract(Namespace = "")]
    public enum HorizontalAlign
    {
        [EnumMember(Value = "left")]
        Left,
        [EnumMember(Value = "center")]
        Center,
        [EnumMember(Value = "right")]
        Right
    }

    [DataContract(Namespace = "")]
    public enum VerticalAlign
    {
        [EnumMember(Value = "bottom")]
        Bottom,
        [EnumMember(Value = "middle")]
        Middle,
        [EnumMember(Value = "top")]
        Top
    }

    [DataContract(Namespace = "")]
    public enum PlaceType
    {
        [EnumMember(Value = "message")]
        Message,
        [EnumMember(Value = "window")]
        Window,
        [EnumMember(Value = "undefined")]
        Undefined
    }
}