using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Radio.Core.Domain.MasterData;
using Radio.Core.Services.MasterData;

namespace Radio.Infrastructure.Api.External.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ImageController : Controller
    {
        private readonly IImageRepository _imageRepository;
        private readonly IImageService _imageService;

        public ImageController(IImageRepository imageRepository, IImageService imageService)
        {
            _imageRepository = imageRepository;
            _imageService = imageService;
        }

        [HttpGet]
        public async Task<FileStreamResult> GetImageAsync(Guid imageId)
        {
            var image = await _imageRepository.GetByIdAsync(imageId);
            var fileInfo = _imageService.FileFromImage(image);

            return File(fileInfo.Data(), fileInfo.ContentType);
        }
    }
}
