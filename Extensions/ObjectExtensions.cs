using System.Linq;

namespace ViberBot.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToQueryString(this object obj)
        {
            var properties = obj.GetType().GetProperties();

            var pairs = properties.Select(x => x.Name + "=" + x.GetValue(obj));

            return string.Join("&", pairs);
        }
    }
}