using System;
using System.Linq;
using Viber.Bot.Enums;
using ViberBot.Models;

namespace ViberBot.Extensions
{
    public static class EnumExtensions
    {
        public static TextHorizontalAlign? ToTextHorizontalAlign(this HorizontalAlign? align)
        {
            switch (align)
            {
                case HorizontalAlign.Left: 
                    return TextHorizontalAlign.Left;
                case HorizontalAlign.Center: 
                    return TextHorizontalAlign.Center;
                case HorizontalAlign.Right: 
                    return TextHorizontalAlign.Right;
            }

            return null;
        }
        
        public static TextVerticalAlign? ToTextVerticalAlign(this VerticalAlign? align)
        {
            switch (align)
            {
                case VerticalAlign.Bottom: 
                    return TextVerticalAlign.Bottom;
                case VerticalAlign.Middle: 
                    return TextVerticalAlign.Middle;
                case VerticalAlign.Top: 
                    return TextVerticalAlign.Top;
            }

            return null;
        }
        
        public static TextSize? ToTextSize(this Size? size)
        {
            switch (size)
            {
                case Size.Small: 
                    return TextSize.Small;
                case Size.Regular: 
                    return TextSize.Regular;
                case Size.Large: 
                    return TextSize.Large;
            }

            return null;
        }
    }
}