using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ViberBot.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum KeyboardReply
    {
        SendLocationBefore,
        AttachPicture,
        SendLocationAfter,
        SendMessage,
        OtherAction
    }
}