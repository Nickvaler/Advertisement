using Advertisement.Domain.Core;
using Advertisement.Domain.Interfaces;
using Advertisement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Advertisement.Services.Interfaces;

namespace Advertisement.Infrastructure.Business
{
    public class AdvertisementService : IAdvertisement
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IAdRepository _adRepository;
        private readonly ITagsRepository _tagsRepository;
        private readonly IAdTagsRepository _adTagsRepository;

        public AdvertisementService(ApplicationContext applicationContext,
            IAdRepository adRepository,
            ITagsRepository tagsRepository,
            IAdTagsRepository adTagsRepository)
        {
            _applicationContext = applicationContext;
            _adRepository = adRepository;
            _tagsRepository = tagsRepository;
            _adTagsRepository = adTagsRepository;
        }

        public async Task CreateAdAsync(Advertising advertising)
        {
            var notCreatedTags = new List<Tag>();
            var createdTags = await _tagsRepository.GetIdsByTagsAsync(advertising.Tags);
            notCreatedTags.AddRange(advertising.Tags.Where(tag => createdTags.All(x => x.Title != tag)).Select(tag => new Tag()
            {
                Title = tag
            }));
            await using var transaction = await _applicationContext.Database.BeginTransactionAsync();
            try
            {
                await _tagsRepository.CreateAsync(notCreatedTags);

                var ad = new Ad()
                {
                    Text = advertising.Text
                };

                await _adRepository.CreateAsync(ad);
                await _adRepository.SaveAsync();

                var adTags = (createdTags.Select(tag => new AdTags()
                {
                    IdsId = ad.Id,
                    TagsId = tag.Id
                })).ToList();
                adTags.AddRange(notCreatedTags.Select(item => new AdTags()
                {
                    IdsId = ad.Id,
                    TagsId = item.Id
                }));
                await _adTagsRepository.CreateAsync(adTags);

                await _adTagsRepository.SaveAsync();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }

        public async Task<List<Ad>> GetAdsByTagsAsync(List<string> tags) => await _adRepository.GetAdsByTagsAsync(tags);
    }
}
