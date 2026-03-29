using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using software_API.Data;
using System.Security.Cryptography;
using System.Text;

namespace software_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharitiesController : ControllerBase
    {
        private readonly YadElawnContext _context;

        public CharitiesController(YadElawnContext context)
        {
            _context = context;
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

        // GET: api/charities
        [HttpGet]
        public async Task<IActionResult> GetAllCharities()
        {
            var charities = await _context.Charities
                .Include(c => c.CharityNavigation)
                .Include(c => c.Location)
                .Select(c => new
                {
                    c.CharityId,
                    Name = c.CharityNavigation.Fname,
                    Email = c.CharityNavigation.Email,
                    Phone = c.CharityNavigation.Phone,
                    LicenseNumber = c.LicenseNumber,
                    CoverageArea = c.CoverageArea,
                    Location = c.Location != null ? c.Location.CityArea : null
                })
                .ToListAsync();

            return Ok(new { success = true, count = charities.Count, data = charities });
        }

        // GET: api/charities/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharityById(int id)
        {
            var charity = await _context.Charities
                .Include(c => c.CharityNavigation)
                .Include(c => c.Location)
                .Include(c => c.Matches)
                .FirstOrDefaultAsync(c => c.CharityId == id);

            if (charity == null)
                return NotFound(new { success = false, message = "Charity not found" });

            var response = new
            {
                charity.CharityId,
                Name = charity.CharityNavigation.Fname,
                Email = charity.CharityNavigation.Email,
                Phone = charity.CharityNavigation.Phone,
                Address = charity.CharityNavigation.Address,
                LicenseNumber = charity.LicenseNumber,
                CoverageArea = charity.CoverageArea,
                Location = charity.Location != null ? new
                {
                    charity.Location.LocationId,
                    charity.Location.CityArea
                } : null,
                IsVerified = charity.CharityNavigation.IsVerified,
                MatchesCount = charity.Matches.Count
            };

            return Ok(new { success = true, data = response });
        }

        // POST: api/charities
        [HttpPost]
        public async Task<IActionResult> CreateCharity([FromBody] CreateCharityRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid request data" });

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user != null)
                return BadRequest(new { success = false, message = "Email already exists" });

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var newUser = new User
                    {
                        Fname = request.Name,
                        Lname = "",
                        Email = request.Email,
                        Password = HashPassword(request.Password),
                        Phone = request.Phone,
                        Address = request.Address,
                        IsVerified = false,
                        UserType = "Charity"
                    };

                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();

                    var charity = new Charity
                    {
                        CharityId = newUser.UserId,
                        LicenseNumber = request.LicenseNumber,
                        CoverageArea = request.CoverageArea,
                        LocationId = request.LocationId
                    };

                    _context.Charities.Add(charity);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return CreatedAtAction(nameof(GetCharityById), new { id = charity.CharityId }, new
                    {
                        success = true,
                        message = "Charity created successfully",
                        charityId = charity.CharityId
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new { success = false, message = "Error creating charity", error = ex.Message });
                }
            }
        }

        // PUT: api/charities/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCharity(int id, [FromBody] UpdateCharityRequest request)
        {
            var charity = await _context.Charities
                .Include(c => c.CharityNavigation)
                .FirstOrDefaultAsync(c => c.CharityId == id);

            if (charity == null)
                return NotFound(new { success = false, message = "Charity not found" });

            charity.CoverageArea = request.CoverageArea;
            charity.LocationId = request.LocationId;
            charity.CharityNavigation.Phone = request.Phone;
            charity.CharityNavigation.Address = request.Address;

            _context.Charities.Update(charity);
            _context.Users.Update(charity.CharityNavigation);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Charity updated successfully" });
        }

        // PUT: api/charities/{id}/verify
        [HttpPut("{id}/verify")]
        public async Task<IActionResult> VerifyCharity(int id)
        {
            var charity = await _context.Charities
                .Include(c => c.CharityNavigation)
                .FirstOrDefaultAsync(c => c.CharityId == id);

            if (charity == null)
                return NotFound(new { success = false, message = "Charity not found" });

            charity.CharityNavigation.IsVerified = true;
            _context.Users.Update(charity.CharityNavigation);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Charity verified successfully" });
        }

        // GET: api/charities/{id}/matches
        [HttpGet("{id}/matches")]
        public async Task<IActionResult> GetCharityMatches(int id)
        {
            var charity = await _context.Charities.FirstOrDefaultAsync(c => c.CharityId == id);

            if (charity == null)
                return NotFound(new { success = false, message = "Charity not found" });

            var matches = await _context.Matches
                .Where(m => m.CharityId == id)
                .Include(m => m.Donation)
                .Include(m => m.Beneficiary)
                .Select(m => new
                {
                    m.MatchId,
                    m.DonationId,
                    BeneficiaryName = $"{m.Beneficiary.BeneficiaryNavigation.Fname} {m.Beneficiary.BeneficiaryNavigation.Lname}",
                    m.UrgencyLevel,
                    m.MatchDate,
                    DonationStatus = m.Donation.Status
                })
                .ToListAsync();

            return Ok(new { success = true, charityId = id, matchesCount = matches.Count, matches = matches });
        }
    }

    public class CreateCharityRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string LicenseNumber { get; set; }
        public string? CoverageArea { get; set; }
        public int? LocationId { get; set; }
    }

    public class UpdateCharityRequest
    {
        public string? CoverageArea { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? LocationId { get; set; }
    }
}
