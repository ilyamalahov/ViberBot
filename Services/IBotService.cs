using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ViberBot.Services
{
    public interface IBotService
    {
        Task HandleRequest(HttpContext context);
    }
}