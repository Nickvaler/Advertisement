using Advertisement.Domain.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Advertisement.Domain.Interfaces
{
    public interface IAdTagsRepository
    {
        public Task CreateAsync(AdTags adTags);

        public Task CreateAsync(List<AdTags> adTags);

        public Task SaveAsync();
    }
}
