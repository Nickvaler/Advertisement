using Advertisement.Domain.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Advertisement.Services.Interfaces;

namespace AdvertisementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase
    {
        private const string SuccessText = "Объявление добавлено";
        private const string NoFoundText = "Не найдено объявлений по заданным тэгам";
        private readonly IAdvertisement _advertisement;
        public AdController(IAdvertisement advertisement)
        {
            _advertisement = advertisement;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Advertising advertisement)
        {
            if (advertisement == null)
            {
                return BadRequest();
            }
            await _advertisement.CreateAdAsync(advertisement);
            return Ok(SuccessText);
        }

        [HttpGet]
        public async Task<ActionResult<List<Ad>>> Get(List<string> tags)
        {
            var result = await _advertisement.GetAdsByTagsAsync(tags);
            return result.Count == 0 ? Ok(NoFoundText) : Ok(result);
        }
    }
}
