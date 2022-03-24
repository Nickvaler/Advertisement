using Advertisement.Domain.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace Advertisement.Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public IConfiguration Configuration { get; }

        public DbSet<Ad> Ads { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<AdTags> AdTags { get; set; }

        public ApplicationContext(IConfiguration configuration)
        {
            Configuration = configuration;
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ad>(AdConfigure);
            modelBuilder.Entity<AdTags>(AdTagsConfigure);
            modelBuilder.Entity<Tag>(TagConfigure);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        public void AdConfigure(EntityTypeBuilder<Ad> builder)
        {
            builder.ToTable("Ads").HasKey(x => x.Id);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Text);
        }

        public void AdTagsConfigure(EntityTypeBuilder<AdTags> builder)
        {
            builder.ToTable("AdTags").HasKey(x => x.Id);
        }

        public void TagConfigure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags").HasKey(x => x.Id);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title);
        }
    }
}
