using Microsoft.AspNetCore.Mvc;
using ViberBot.Services;

namespace ViberBot.Controllers
{
    [Route("api/[controller]")]
    public class ViberController : ControllerBase
    {
        [HttpPost]
        public async void Index([FromServices] IBotService viberBotService)
        {
            await viberBotService.HandleRequest(HttpContext);
        }
    }
}