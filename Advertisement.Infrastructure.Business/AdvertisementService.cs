using Advertisement.Domain.Core;
using Advertisement.Domain.Interfaces;
using Advertisement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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

            foreach (var tag in advertising.Tags)
            {
                if (!createdTags.Any(x => x.Title == tag))
                {
                    notCreatedTags.Add(new Tag()
                    {
                        Title = tag
                    });
                }
            }

            using (var transaction = _applicationContext.Database.BeginTransaction())
            {
                try
                {
                    await _tagsRepository.CreateAsync(notCreatedTags);

                    var ad = new Ad()
                    {
                        Text = advertising.Text
                    };

                    await _adRepository.CreateAsync(ad);
                    await _adRepository.SaveAsync();

                    var adTags = new List<AdTags>();
                    foreach (var tag in createdTags)
                    {
                        adTags.Add(new AdTags()
                        {
                            IdsId = ad.Id,
                            TagsId = tag.Id
                        });
                    }
                    adTags.AddRange(notCreatedTags.Select(item => new AdTags()
                    {
                        IdsId = ad.Id,
                        TagsId = item.Id
                    }));
                    await _adTagsRepository.CreateAsync(adTags);

                    await _adTagsRepository.SaveAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }

        public async Task<List<Ad>> GetAdsByTagsAsync(List<string> tags)
        {
            return await _adRepository.GetAdsByTagsAsync(tags);
        }
    }
}
