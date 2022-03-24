using Advertisement.Domain.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Advertisement.Domain.Interfaces
{
    public interface IAdRepository
    {
        public Task CreateAsync(Ad ad);

        public Task CreateAsync(List<Ad> ads);

        public Task SaveAsync();
        public Task<List<Ad>> GetAdsByTagsAsync(List<string> tags);
    }
}
