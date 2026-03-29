using Microsoft.EntityFrameworkCore;
using software_API.Data;

namespace software_API.Services
{
    public class DatabaseInitializationService
    {
        private readonly YadElawnContext _context;
        private readonly ILogger<DatabaseInitializationService> _logger;

        public DatabaseInitializationService(YadElawnContext context, ILogger<DatabaseInitializationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Initialize database on application startup
        /// </summary>
        public async Task InitializeDatabaseAsync()
        {
            try
            {
                _logger.LogInformation("Initializing database...");

                // Apply migrations automatically
                var migrations = await _context.Database.GetPendingMigrationsAsync();
                if (migrations.Any())
                {
                    _logger.LogInformation($"Applying {migrations.Count()} pending migrations...");
                    await _context.Database.MigrateAsync();
                    _logger.LogInformation("? Migrations applied successfully");
                }
                else
                {
                    _logger.LogInformation("No pending migrations");
                }

                // Verify connection
                var canConnect = await _context.Database.CanConnectAsync();
                if (canConnect)
                {
                    _logger.LogInformation("? Database connection verified");
                }
                else
                {
                    _logger.LogWarning("?? Could not verify database connection");
                }

                // Seed initial data if needed
                await SeedDataAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error initializing database");
                throw;
            }
        }

        private async Task SeedDataAsync()
        {
            try
            {
                // Check if data already exists
                if (await _context.Locations.AnyAsync())
                {
                    _logger.LogInformation("Database already contains data, skipping seed");
                    return;
                }

                _logger.LogInformation("Seeding initial data...");

                // Add default locations
                var locations = new[]
                {
                    new Location { CityArea = "Cairo - Heliopolis", GpsCoordinates = "30.0444,31.3619" },
                    new Location { CityArea = "Cairo - Zamalek", GpsCoordinates = "30.0615,31.2535" },
                    new Location { CityArea = "Giza - Dokki", GpsCoordinates = "30.0196,31.2021" },
                    new Location { CityArea = "Alexandria - Downtown", GpsCoordinates = "31.2001,29.9187" }
                };

                await _context.Locations.AddRangeAsync(locations);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"? Seeded {locations.Length} locations");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Warning during data seeding, but this is not critical");
                // Don't throw - seeding is optional
            }
        }
    }
}
