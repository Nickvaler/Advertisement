using Advertisement.Domain.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Advertisement.Domain.Interfaces
{
    public interface IAdvertisement
    {
        Task CreateAdAsync(Advertising ad);
        Task<List<Ad>> GetAdsByTagsAsync(List<string> tags);
    }
}
