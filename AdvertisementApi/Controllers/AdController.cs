using Advertisement.Domain.Core;
using Advertisement.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvertisementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase
    {
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
            return Ok("Объявление добавлено");
        }

        [HttpGet]
        public async Task<ActionResult<List<Ad>>> Get(List<string> tags)
        {
            var result = await _advertisement.GetAdsByTagsAsync(tags);
            if (result.Count == 0)
            {
                return Ok("Не найдено объявлений по заданным тэгам");
            }
            return Ok(result);
        }
    }
}
