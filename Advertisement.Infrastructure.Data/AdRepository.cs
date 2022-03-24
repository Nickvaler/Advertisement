using Advertisement.Domain.Core;
using Advertisement.Domain.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Advertisement.Infrastructure.Data
{
    public class AdRepository : IAdRepository
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IConfiguration _configuration;
        private readonly string _connectString;

        public AdRepository(ApplicationContext applicationContext, IConfiguration configuration)
        {
            _applicationContext = applicationContext;
            _configuration = configuration;
            _connectString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateAsync(Ad ad)
        {
            await _applicationContext.Ads.AddAsync(ad);
        }

        public async Task CreateAsync(List<Ad> ads)
        {
            await _applicationContext.Ads.AddRangeAsync(ads);
        }

        public async Task SaveAsync()
        {
            await _applicationContext.SaveChangesAsync();
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
