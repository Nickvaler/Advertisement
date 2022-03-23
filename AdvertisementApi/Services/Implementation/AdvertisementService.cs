using Advertisement.Models;
using Advertisement.Models.DbModels;
using AdvertisementApi.Context;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertisementApi.Services.Implementation
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly ApplicationContext _adContext;
        private readonly IConfiguration _configuration;
        private readonly string _connectString;
        public AdvertisementService(IConfiguration configuration, ApplicationContext adContext)
        {
            _adContext = adContext;
            _configuration = configuration;
            _connectString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateAdAsync(Advertising advertising)
        {
            var notCreatedTags = GetNotCreatedTags(advertising.Tags);
            var createdTags = await GetCreatedTagIdsAsync(advertising.Tags);

            using (var transaction = _adContext.Database.BeginTransaction())
            {
                try
                {
                    await _adContext.Tags.AddRangeAsync(notCreatedTags);

                    var ad = new Ad()
                    {
                        Text = advertising.Text
                    };

                    await _adContext.Ads.AddAsync(ad);
                    await _adContext.SaveChangesAsync();

                    var adTags = new List<AdTags>();
                    foreach (var tagId in createdTags)
                    {
                        adTags.Add(new AdTags()
                        {
                            IdsId = ad.Id,
                            TagsId = tagId
                        });
                    }
                    adTags.AddRange(notCreatedTags.Select(item => new AdTags()
                    {
                        IdsId = ad.Id,
                        TagsId = item.Id
                    }));
                    await _adContext.AdTags.AddRangeAsync(adTags);

                    await _adContext.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }

        private List<Tag> GetNotCreatedTags(List<string> tags)
        {
            return (tags.Where(tag => !_adContext.Tags.Any(x => x.Title == tag)).Select(tag => new Tag()
            {
                Title = tag
            })).ToList();
        }
        
        private async Task<List<int>> GetCreatedTagIdsAsync(List<string> tags)
        {
            using (var connection = new SqlConnection(_connectString))
            {
                var sql = @"SELECT Id 
                        FROM [dbo].[Tags] with(nolock)
                        WHERE [Title] in @tags";
                var results = await connection.QueryAsync<int>(sql, new { tags = tags });
                return results.ToList();
            }
        }

        public async Task<List<Ad>> GetAdsByTagsAsync(List<string> tags)
        {
            using (var connection = new SqlConnection(_connectString))
            {
                var sql = @"SELECT distinct a.Id, a.Text 
                        FROM [dbo].[Tags] t with(nolock)
                        INNER JOIN [dbo].[AdTags] adt with(nolock) on adt.[TagsId] = t.[Id]
                        INNER JOIN [dbo].[Ads] a with(nolock) on a.[Id] = adt.[IdsId]
                        WHERE t.[Title] in @tags";
                var results = await connection.QueryAsync<Ad>(sql, new { tags = tags });
                return results.ToList();
            }
        }
    }
}
