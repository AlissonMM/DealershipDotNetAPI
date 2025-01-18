using DealershipDotNetAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DealershipDotNetAPI.Infrastructure.Db
{
    /// <summary>
    /// Represents the database context for the application.
    /// This class is responsible for configuring the connection to the database
    /// and defining the DbSet properties, which map to database tables.
    /// </summary>
   
    public class ContextDb : DbContext
    {
        /// Represents the "Administrators" table in the database.
        /// Maps the Administrator entity to this table.
        public DbSet<Administrator> Administrators { get; set; } = default!;
        public DbSet<Vehicle> Vehicles { get; set; } = default!;

        /// Holds the application's configuration settings, specifically for accessing connection strings.
        public readonly IConfiguration _configurationAppSettings;

        /// Constructor for the ContextDb class.
        /// It initializes the configuration settings passed via dependency injection.
        public ContextDb(IConfiguration configurationAppSettings)
        {
            _configurationAppSettings = configurationAppSettings;
        }

        /// Configures the database connection for the DbContext.
        /// This method is called automatically by the Entity Framework Core.
        /// It checks if the options builder is already configured, retrieves the MySQL connection string from app settings,
        /// and sets up the database connection using the MySQL provider.
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
