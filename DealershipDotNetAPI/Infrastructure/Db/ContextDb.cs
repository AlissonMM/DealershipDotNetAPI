using DealershipDotNetAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DealershipDotNetAPI.Infrastructure.Db
{
    public class ContextDb : DbContext
    {
        public DbSet<Administrator> Administrators { get; set; } = default!;

        public readonly IConfiguration _configurationAppSettings;

        public ContextDb(IConfiguration configurationAppSettings)
        {
            _configurationAppSettings = configurationAppSettings;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configurationAppSettings.GetConnectionString("mysql").ToString();

                if (!string.IsNullOrEmpty(connectionString))
                {
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                }
            }
            
            
        }
    }
}
