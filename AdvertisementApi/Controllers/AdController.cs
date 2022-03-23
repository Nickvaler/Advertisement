using AdvertisementApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Advertisement.Models;
using Advertisement.Models.DbModels;

namespace AdvertisementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : ControllerBase
    {
        private readonly IAdvertisementService _advertisementService;
        public AdController(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Advertising advertisement)
        {
            if (advertisement == null)
            {
                return BadRequest();
            }
            await _advertisementService.CreateAdAsync(advertisement);
            return Ok("Объявление добавлено");
        }

        [HttpGet]
        public async Task<ActionResult<List<Ad>>> Get(List<string> tags)
        {
            var result = await _advertisementService.GetAdsByTagsAsync(tags);
            if (result.Count == 0)
            {
                return Ok("Не найдено объявлений по заданным тэгам");
            }
            return Ok(result);
        }
    }
}
