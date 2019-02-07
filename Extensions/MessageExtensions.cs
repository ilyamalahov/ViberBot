using System.Linq;
using ViberBot.Models;

namespace ViberBot.Extensions
{
    public static class MessageExtensions
    {
        public static MessageType DetermineMessageType(this OutMessage message)
        {
            if(message.Picture != null)
            {
                return MessageType.Picture;
            }
            else if(message.Location != null)
            {
                return MessageType.Location;                
            }
            else if (message.ButtonPlace == PlaceType.Message)
            {
                return MessageType.RichMedia;
            }
            else if (message.ButtonPlace == PlaceType.Window)
            {
                return MessageType.Keyboard;
            }
            
            return MessageType.Text;
        }
        public static MessageType DetermineMessageType(this InMessage message)
        {
            if(message.Picture != null)
            {
                return MessageType.Picture;
            }
            else if(message.Video != null)
            {
                return MessageType.Video;                
            }
            else if (message.Location != null)
            {
                return MessageType.Location;
            }
            else if (message.ButtonId != null)
            {
                return MessageType.Keyboard;
            }
            
            return MessageType.Text;
        }
    }
}

namespace ViberBot.Models
{
    public enum MessageType
    {
        Text,
        Picture,
        Video,
        Location,
        RichMedia,
        Keyboard
    }
}