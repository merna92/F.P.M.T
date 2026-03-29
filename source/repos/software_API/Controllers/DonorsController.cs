using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using software_API.Data;

namespace software_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorsController : ControllerBase
    {
        private readonly YadElawnContext _context;

        public DonorsController(YadElawnContext context)
        {
            _context = context;
        }

        // GET: api/donors
        [HttpGet]
        public async Task<IActionResult> GetAllDonors()
        {
            var donors = await _context.Donors
                .Include(d => d.DonorNavigation)
                .Select(d => new
                {
                    d.DonorId,
                    Name = $"{d.DonorNavigation.Fname} {d.DonorNavigation.Lname}",
                    Email = d.DonorNavigation.Email,
                    Phone = d.DonorNavigation.Phone,
                    d.DonationCount,
                    IsVerified = d.DonorNavigation.IsVerified
                })
                .ToListAsync();

            return Ok(new { success = true, count = donors.Count, data = donors });
        }

        // GET: api/donors/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDonorById(int id)
        {
            var donor = await _context.Donors
                .Include(d => d.DonorNavigation)
                .Include(d => d.Donations)
                .FirstOrDefaultAsync(d => d.DonorId == id);

            if (donor == null)
                return NotFound(new { success = false, message = "Donor not found" });

            var response = new
            {
                donor.DonorId,
                Name = $"{donor.DonorNavigation.Fname} {donor.DonorNavigation.Lname}",
                Email = donor.DonorNavigation.Email,
                Phone = donor.DonorNavigation.Phone,
                Address = donor.DonorNavigation.Address,
                donor.DonationCount,
                IsVerified = donor.DonorNavigation.IsVerified,
                DonationsCount = donor.Donations.Count
            };

            return Ok(new { success = true, data = response });
        }

        // GET: api/donors/{id}/donations
        [HttpGet("{id}/donations")]
        public async Task<IActionResult> GetDonorDonations(int id)
        {
            var donor = await _context.Donors.FirstOrDefaultAsync(d => d.DonorId == id);

            if (donor == null)
                return NotFound(new { success = false, message = "Donor not found" });

            var donations = await _context.Donations
                .Where(d => d.DonorId == id)
                .Include(d => d.Location)
                .Include(d => d.Food)
                .Include(d => d.Medicine)
                .Include(d => d.Clothe)
                .Select(d => new
                {
                    d.DonationId,
                    d.Status,
                    DonationType = d.Food != null ? "Food" : d.Medicine != null ? "Medicine" : "Clothes",
                    Location = d.Location != null ? d.Location.CityArea : null,
                    Details = d.Food != null ? (object)new
                    {
                        d.Food.ProductName,
                        d.Food.ExpiryDate,
                        d.Food.Quantity
                    } : d.Medicine != null ? (object)new
                    {
                        d.Medicine.MedicineName,
                        d.Medicine.ExpiryDate,
                        d.Medicine.Quantity
                    } : (object)new
                    {
                        d.Clothe.Gender,
                        d.Clothe.Size,
                        d.Clothe.Season
                    }
                })
                .ToListAsync();

            return Ok(new { success = true, donorId = id, donationsCount = donations.Count, donations = donations });
        }

        // PUT: api/donors/{id}/verify
        [HttpPut("{id}/verify")]
        public async Task<IActionResult> VerifyDonor(int id)
        {
            var donor = await _context.Donors
                .Include(d => d.DonorNavigation)
                .FirstOrDefaultAsync(d => d.DonorId == id);

            if (donor == null)
                return NotFound(new { success = false, message = "Donor not found" });

            donor.DonorNavigation.IsVerified = true;
            _context.Users.Update(donor.DonorNavigation);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Donor verified successfully" });
        }

        // GET: api/donors/statistics/top-donors
        [HttpGet("statistics/top-donors")]
        public async Task<IActionResult> GetTopDonors([FromQuery] int limit = 10)
        {
            var topDonors = await _context.Donors
                .Include(d => d.DonorNavigation)
                .OrderByDescending(d => d.DonationCount)
                .Take(limit)
                .Select(d => new
                {
                    d.DonorId,
                    Name = $"{d.DonorNavigation.Fname} {d.DonorNavigation.Lname}",
                    Email = d.DonorNavigation.Email,
                    d.DonationCount
                })
                .ToListAsync();

            return Ok(new { success = true, count = topDonors.Count, data = topDonors });
        }
    }
}
