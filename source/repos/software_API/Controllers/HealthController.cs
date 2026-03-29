using Microsoft.AspNetCore.Mvc;
using software_API.Services;

namespace software_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly DatabaseTestService _dbTestService;
        private readonly ILogger<HealthController> _logger;

        public HealthController(DatabaseTestService dbTestService, ILogger<HealthController> logger)
        {
            _dbTestService = dbTestService;
            _logger = logger;
        }

        /// <summary>
        /// Health check endpoint
        /// </summary>
        [HttpGet("check")]
        public async Task<IActionResult> HealthCheck()
        {
            try
            {
                var isConnected = await _dbTestService.TestDatabaseConnectionAsync();
                var userCount = await _dbTestService.GetUsersCountAsync();

                return Ok(new
                {
                    success = true,
                    status = "API is running",
                    database = isConnected ? "Connected" : "Disconnected",
                    totalUsers = userCount,
                    timestamp = DateTime.UtcNow,
                    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                    apiVersion = "1.0.0",
                    apiName = "Yad El-Awn API"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return StatusCode(500, new
                {
                    success = false,
                    status = "API error",
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Simple health check (no database)
        /// </summary>
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok(new
            {
                success = true,
                message = "Pong! API is alive",
                timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Seed database with test data
        /// </summary>
        [HttpPost("seed-data")]
        public async Task<IActionResult> SeedData()
        {
            try
            {
                await _dbTestService.SeedDefaultDataAsync();
                return Ok(new
                {
                    success = true,
                    message = "Data seeded successfully",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Seed data failed");
                return StatusCode(500, new
                {
                    success = false,
                    error = ex.Message,
                    timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Get API info
        /// </summary>
        [HttpGet("info")]
        public IActionResult GetInfo()
        {
            return Ok(new
            {
                success = true,
                apiName = "Yad El-Awn (?? ?????)",
                apiVersion = "1.0.0",
                description = "Donation Management Platform",
                environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                documentation = "/swagger",
                baseUrl = $"{Request.Scheme}://{Request.Host}",
                timestamp = DateTime.UtcNow
            });
        }
    }
}
