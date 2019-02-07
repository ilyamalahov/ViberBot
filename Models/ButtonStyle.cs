using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace ViberBot.Models
{
    [DataContract(Name = "style", Namespace = "")]
    public class ButtonStyle
    {
        [DataMember(Name = "bgColor")]
        public string BackgroundColor { get; set; }

        [DataMember(Name = "textVerticalAlign")]
        public VerticalAlign? TextVerticalAlign { get; set; }
        
        [DataMember(Name = "textHorizontalAlign")]
        public HorizontalAlign? TextHorizontalAlign { get; set; }

        [DataMember(Name = "textOpacity")]
        public int? TextOpacity { get; set; }

        [DataMember(Name = "textSize")]
        public Size? TextSize { get; set; }
    }
}