using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using software_API.Data;

namespace software_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly YadElawnContext _context;

        public LocationsController(YadElawnContext context)
        {
            _context = context;
        }

        // GET: api/locations
        [HttpGet]
        public async Task<IActionResult> GetAllLocations()
        {
            var locations = await _context.Locations.ToListAsync();
            return Ok(new { success = true, count = locations.Count, data = locations });
        }

        // GET: api/locations/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(l => l.LocationId == id);

            if (location == null)
                return NotFound(new { success = false, message = "Location not found" });

            return Ok(new { success = true, data = location });
        }

        // POST: api/locations
        [HttpPost]
        public async Task<IActionResult> CreateLocation([FromBody] CreateLocationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid request data" });

            var location = new Location
            {
                CityArea = request.CityArea,
                GpsCoordinates = request.GpsCoordinates
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLocationById), new { id = location.LocationId }, new
            {
                success = true,
                message = "Location created successfully",
                locationId = location.LocationId
            });
        }

        // PUT: api/locations/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, [FromBody] CreateLocationRequest request)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(l => l.LocationId == id);

            if (location == null)
                return NotFound(new { success = false, message = "Location not found" });

            location.CityArea = request.CityArea;
            location.GpsCoordinates = request.GpsCoordinates;

            _context.Locations.Update(location);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Location updated successfully", locationId = location.LocationId });
        }

        // DELETE: api/locations/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _context.Locations.FirstOrDefaultAsync(l => l.LocationId == id);

            if (location == null)
                return NotFound(new { success = false, message = "Location not found" });

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Location deleted successfully" });
        }
    }

    public class CreateLocationRequest
    {
        public string CityArea { get; set; }
        public string? GpsCoordinates { get; set; }
    }
}
