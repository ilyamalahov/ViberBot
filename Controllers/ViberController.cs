using Microsoft.AspNetCore.Mvc;
using ViberBot.Services;

namespace ViberBot.Controllers
{
    [Route("api/[controller]")]
    public class ViberController : ControllerBase
    {
        [HttpPost]
        public async void Index([FromServices] IRequestService requestService)
        {
            await requestService.HandleRequest(HttpContext);
        }
    }
}