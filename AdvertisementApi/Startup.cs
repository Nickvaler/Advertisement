using Advertisement.Domain.Interfaces;
using Advertisement.Infrastructure.Business;
using Advertisement.Infrastructure.Data;
using Advertisement.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdvertisementApi
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private const string DefaultConnection = "DefaultConnection";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString(DefaultConnection);
            services.AddScoped<IAdRepository, AdRepository>();
            services.AddScoped<ITagsRepository, TagsRepository>();
            services.AddScoped<IAdTagsRepository, AdTagsRepository>();
            services.AddScoped<IAdvertisement, AdvertisementService>();
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
