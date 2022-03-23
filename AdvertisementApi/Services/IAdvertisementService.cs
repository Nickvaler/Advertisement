using Advertisement.Models;
using Advertisement.Models.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvertisementApi.Services
{
    public interface IAdvertisementService
    {
        Task CreateAdAsync(Advertising ad);
        Task<List<Ad>> GetAdsByTagsAsync(List<string> tags);
    }
}
