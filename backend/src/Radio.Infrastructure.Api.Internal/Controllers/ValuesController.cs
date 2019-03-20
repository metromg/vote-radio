using Microsoft.AspNetCore.Mvc;

namespace Radio.Infrastructure.Api.Internal.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "Hello Internal";
        }
    }
}
