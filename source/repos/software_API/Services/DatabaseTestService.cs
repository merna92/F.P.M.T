using Microsoft.EntityFrameworkCore;
using software_API.Data;

namespace software_API.Services
{
    public class DatabaseTestService
    {
        private readonly YadElawnContext _context;
        private readonly ILogger<DatabaseTestService> _logger;

        public DatabaseTestService(YadElawnContext context, ILogger<DatabaseTestService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> TestDatabaseConnectionAsync()
        {
            try
            {
                // ????? ??? Connection
                var result = await _context.Database.ExecuteSqlRawAsync("SELECT 1");
                _logger.LogInformation("? Database connection successful!");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Database connection failed: {ex.Message}");
                return false;
            }
        }

        public async Task<int> GetUsersCountAsync()
        {
            try
            {
                var count = await _context.Users.CountAsync();
                _logger.LogInformation($"?? Total users: {count}");
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error counting users: {ex.Message}");
                return 0;
            }
        }

        public async Task SeedDefaultDataAsync()
        {
            try
            {
                // ????? ??? ??? ???? ??????
                if (await _context.Users.AnyAsync())
                {
                    _logger.LogInformation("Database already has data");
                    return;
                }

                // ??? location ???????
                var location = new Location
                {
                    CityArea = "Cairo - Heliopolis",
                    GpsCoordinates = "30.0444,31.3619"
                };
                _context.Locations.Add(location);
                await _context.SaveChangesAsync();

                _logger.LogInformation("? Seed data added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error seeding data: {ex.Message}");
            }
        }
    }
}
