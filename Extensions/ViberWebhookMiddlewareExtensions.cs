using Microsoft.AspNetCore.Builder;
using ViberBot.Middlewares;

namespace ViberBot.Extensions
{
    public static class ViberWebhookMiddlewareExtensions
    {
        public static IApplicationBuilder UseViberWebhook(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ViberWebhookMiddleware>();
        }
    }
}