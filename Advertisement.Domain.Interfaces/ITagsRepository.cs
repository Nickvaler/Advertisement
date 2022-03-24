using Advertisement.Domain.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Advertisement.Domain.Interfaces
{
    public interface ITagsRepository
    {
        public Task CreateAsync(Tag tag);

        public Task CreateAsync(List<Tag> tags);

        public Task SaveAsync();

        public Task<List<Tag>> GetIdsByTagsAsync(List<string> tags);
    }
}
