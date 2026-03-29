using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using software_API.Data;

namespace software_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiariesController : ControllerBase
    {
        private readonly YadElawnContext _context;

        public BeneficiariesController(YadElawnContext context)
        {
            _context = context;
        }

        // GET: api/beneficiaries
        [HttpGet]
        public async Task<IActionResult> GetAllBeneficiaries()
        {
            var beneficiaries = await _context.Beneficiaries
                .Include(b => b.BeneficiaryNavigation)
                .Include(b => b.Location)
                .Select(b => new
                {
                    b.BeneficiaryId,
                    Name = $"{b.BeneficiaryNavigation.Fname} {b.BeneficiaryNavigation.Lname}",
                    Email = b.BeneficiaryNavigation.Email,
                    Phone = b.BeneficiaryNavigation.Phone,
                    Address = b.BeneficiaryNavigation.Address,
                    Location = b.Location != null ? b.Location.CityArea : null,
                    IsVerified = b.BeneficiaryNavigation.IsVerified
                })
                .ToListAsync();

            return Ok(new { success = true, count = beneficiaries.Count, data = beneficiaries });
        }

        // GET: api/beneficiaries/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBeneficiaryById(int id)
        {
            var beneficiary = await _context.Beneficiaries
                .Include(b => b.BeneficiaryNavigation)
                .Include(b => b.Location)
                .Include(b => b.Matches)
                .FirstOrDefaultAsync(b => b.BeneficiaryId == id);

            if (beneficiary == null)
                return NotFound(new { success = false, message = "Beneficiary not found" });

            var response = new
            {
                beneficiary.BeneficiaryId,
                Name = $"{beneficiary.BeneficiaryNavigation.Fname} {beneficiary.BeneficiaryNavigation.Lname}",
                Email = beneficiary.BeneficiaryNavigation.Email,
                Phone = beneficiary.BeneficiaryNavigation.Phone,
                Address = beneficiary.BeneficiaryNavigation.Address,
                Location = beneficiary.Location != null ? new
                {
                    beneficiary.Location.LocationId,
                    beneficiary.Location.CityArea,
                    beneficiary.Location.GpsCoordinates
                } : null,
                IsVerified = beneficiary.BeneficiaryNavigation.IsVerified,
                MatchesCount = beneficiary.Matches.Count
            };

            return Ok(new { success = true, data = response });
        }

        // PUT: api/beneficiaries/{id}/verify
        [HttpPut("{id}/verify")]
        public async Task<IActionResult> VerifyBeneficiary(int id)
        {
            var beneficiary = await _context.Beneficiaries
                .Include(b => b.BeneficiaryNavigation)
                .FirstOrDefaultAsync(b => b.BeneficiaryId == id);

            if (beneficiary == null)
                return NotFound(new { success = false, message = "Beneficiary not found" });

            beneficiary.BeneficiaryNavigation.IsVerified = true;
            _context.Users.Update(beneficiary.BeneficiaryNavigation);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Beneficiary verified successfully" });
        }

        // GET: api/beneficiaries/{id}/matches
        [HttpGet("{id}/matches")]
        public async Task<IActionResult> GetBeneficiaryMatches(int id)
        {
            var beneficiary = await _context.Beneficiaries.FirstOrDefaultAsync(b => b.BeneficiaryId == id);

            if (beneficiary == null)
                return NotFound(new { success = false, message = "Beneficiary not found" });

            var matches = await _context.Matches
                .Where(m => m.BeneficiaryId == id)
                .Include(m => m.Donation)
                .Include(m => m.Charity)
                .Select(m => new
                {
                    m.MatchId,
                    m.DonationId,
                    CharityName = $"{m.Charity.CharityNavigation.Fname} {m.Charity.CharityNavigation.Lname}",
                    m.UrgencyLevel,
                    m.MatchDate,
                    DonationStatus = m.Donation.Status
                })
                .ToListAsync();

            return Ok(new { success = true, beneficiaryId = id, matchesCount = matches.Count, matches = matches });
        }
    }
}
