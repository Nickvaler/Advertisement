using Advertisement.Domain.Core;
using Advertisement.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Advertisement.Infrastructure.Data
{
    public class AdTagsRepository : IAdTagsRepository
    {
        private readonly ApplicationContext _applicationContext;

        public AdTagsRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task CreateAsync(AdTags adTags)
        {
            await _applicationContext.AdTags.AddAsync(adTags);
        }

        public async Task CreateAsync(List<AdTags> adTags)
        {
            await _applicationContext.AdTags.AddRangeAsync(adTags);
        }

        public async Task SaveAsync()
        {
            await _applicationContext.SaveChangesAsync();
        }
    }
}
