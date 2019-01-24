using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ViberBot.Services
{
    public interface IRequestService
    {
        Task HandleRequest(HttpContext context);
    }
}