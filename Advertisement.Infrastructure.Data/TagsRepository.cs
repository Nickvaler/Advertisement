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
    public class TagsRepository : ITagsRepository
    {
        private readonly ApplicationContext _applicationContext;
        private readonly string _connectString;

        public TagsRepository(ApplicationContext applicationContext, IConfiguration configuration)
        {
            _applicationContext = applicationContext;
            _connectString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateAsync(Tag tag)
        {
           await _applicationContext.Tags.AddAsync(tag);
        }
        
        public async Task CreateAsync(List<Tag> tags)
        {
            await _applicationContext.Tags.AddRangeAsync(tags);
        }

        public async Task SaveAsync()
        {
            await _applicationContext.SaveChangesAsync();
        }

        public async Task<List<Tag>> GetIdsByTagsAsync(List<string> tags)
        {
            using (var connection = new SqlConnection(_connectString))
            {
                var sql = @"SELECT [Id], [Title]
                        FROM [dbo].[Tags] with(nolock)
                        WHERE [Title] in @tags";
                var results = await connection.QueryAsync<Tag>(sql, new { tags });
                return results.ToList();
            }
        }
    }
}
