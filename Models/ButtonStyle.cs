using System.Xml.Serialization;

namespace ViberBot.Models
{
    public class ButtonStyle
    {
        [XmlElement("bgColor")]
        public string BackgroundColor { get; set; }

        [XmlElement("textVerticalAlign")]
        public VerticalAlign? TextVerticalAlign { get; set; }
        
        [XmlElement("textHorizontalAlign")]
        public HorizontalAlign? TextHorizontalAlign { get; set; }

        [XmlElement("textOpacity")]
        public int? TextOpacity { get; set; }

        [XmlElement("textSize")]
        public Size? TextSize { get; set; }
    }
}