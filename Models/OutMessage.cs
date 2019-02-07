using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace ViberBot.Models
{
    public class OutMessage
    {
        [XmlElement("text")]
        public string Text { get; set; }
        
        [XmlElement("pic")]
        public string Picture { get; set; }
        
        [XmlElement("location")]
        public Location Location { get; set; }

        [XmlElement("buttonplace")]
        public PlaceType? ButtonPlace { get; set; }

        [XmlArray("buttonlist")]
        [XmlArrayItem("button")]
        public Collection<Button> Buttons { get; set; }
    }

    public enum Size
    {
        [XmlEnum("small")]
        Small,
        [XmlEnum("regular")]
        Regular,
        [XmlEnum("large")]
        Large
    }

    public enum HorizontalAlign
    {
        [XmlEnum("left")]
        Left,
        [XmlEnum("center")]
        Center,
        [XmlEnum("right")]
        Right
    }

    public enum VerticalAlign
    {
        [XmlEnum("bottom")]
        Bottom,
        [XmlEnum("middle")]
        Middle,
        [XmlEnum("top")]
        Top
    }

    public enum PlaceType
    {
        [XmlEnum("message")]
        Message,
        [XmlEnum("window")]
        Window,
        [XmlEnum("undefined")]
        Undefined
    }
}