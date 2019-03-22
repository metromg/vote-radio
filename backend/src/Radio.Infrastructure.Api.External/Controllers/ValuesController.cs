using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Radio.Core.Domain;

namespace Radio.Infrastructure.Api.External.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ValuesController : Controller
    {
        private readonly IRepository<Song> _songRepository;

        public ValuesController(IRepository<Song> songRepository)
        {
            _songRepository = songRepository;
        }

        [HttpGet]
        public string Get()
        {
            var song = _songRepository.Get().FirstOrDefault();

            return $"Hello External {song.Title}";
        }
    }
}
