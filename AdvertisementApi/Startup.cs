using Advertisement.Domain.Interfaces;
using Advertisement.Infrastructure.Business;
using Advertisement.Infrastructure.Data;
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
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
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
