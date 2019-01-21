using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ViberBot.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StartDriverKeyboardReply
    {
        [EnumMember(Value = "controlPointNear")]
        ControlPointNear,
        [EnumMember(Value = "controlPointSearch")]
        ControlPointSearch,
        [EnumMember(Value = "toBegining")]
        ToBegining
    }
}