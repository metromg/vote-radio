using Microsoft.AspNetCore.Mvc;

namespace Radio.Infrastructure.Api.Internal.Controllers
{
    [Route("[action]")]
    public class NextSongController : Controller
    {
        [HttpGet]
        public string Next()
        {
            return "test.mp3";
        }
    }
}
