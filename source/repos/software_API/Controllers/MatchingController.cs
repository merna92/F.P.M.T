using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using software_API.Data;

namespace software_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchingController : ControllerBase
    {
        private readonly YadElawnContext _context;

        public MatchingController(YadElawnContext context)
        {
            _context = context;
        }

        // GET: api/matching/available-donations/{beneficiaryId}
        [HttpGet("available-donations/{beneficiaryId}")]
        public async Task<IActionResult> GetAvailableDonationsForBeneficiary(int beneficiaryId)
        {
            var beneficiary = await _context.Beneficiaries
                .Include(b => b.Location)
                .FirstOrDefaultAsync(b => b.BeneficiaryId == beneficiaryId);

            if (beneficiary == null)
                return NotFound(new { success = false, message = "Beneficiary not found" });

            var availableDonations = await _context.Donations
                .Where(d => d.Status == "Available" && d.LocationId == beneficiary.LocationId)
                .Include(d => d.Donor)
                .Include(d => d.Food)
                .Include(d => d.Medicine)
                .Include(d => d.Clothe)
                .Include(d => d.Location)
                .Select(d => new
                {
                    d.DonationId,
                    d.Status,
                    DonorName = $"{d.Donor.DonorNavigation.Fname} {d.Donor.DonorNavigation.Lname}",
                    Location = d.Location.CityArea,
                    DonationType = d.Food != null ? "Food" : d.Medicine != null ? "Medicine" : "Clothes",
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

            return Ok(new
            {
                success = true,
                beneficiaryId = beneficiary.BeneficiaryId,
                location = beneficiary.Location?.CityArea,
                availableDonations = availableDonations,
                count = availableDonations.Count
            });
        }

        // POST: api/matching/create-match
        [HttpPost("create-match")]
        public async Task<IActionResult> CreateMatch([FromBody] CreateMatchRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid request data" });

            var donation = await _context.Donations
                .Include(d => d.Donor)
                .FirstOrDefaultAsync(d => d.DonationId == request.DonationId);

            if (donation == null)
                return NotFound(new { success = false, message = "Donation not found" });

            var beneficiary = await _context.Beneficiaries.FirstOrDefaultAsync(b => b.BeneficiaryId == request.BeneficiaryId);
            if (beneficiary == null)
                return NotFound(new { success = false, message = "Beneficiary not found" });

            var charity = await _context.Charities.FirstOrDefaultAsync(c => c.CharityId == request.CharityId);
            if (charity == null)
                return NotFound(new { success = false, message = "Charity not found" });

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var match = new Match
                    {
                        DonationId = request.DonationId,
                        BeneficiaryId = request.BeneficiaryId,
                        CharityId = request.CharityId,
                        UrgencyLevel = request.UrgencyLevel,
                        Distance = request.Distance,
                        MatchDate = DateTime.UtcNow
                    };

                    _context.Matches.Add(match);

                    donation.Status = "Matched";
                    _context.Donations.Update(donation);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return CreatedAtAction(nameof(GetMatchById), new { id = match.MatchId }, new
                    {
                        success = true,
                        message = "Match created successfully",
                        matchId = match.MatchId
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new { success = false, message = "Error creating match", error = ex.Message });
                }
            }
        }

        // GET: api/matching/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatchById(int id)
        {
            var match = await _context.Matches
                .Include(m => m.Donation)
                .Include(m => m.Beneficiary)
                .Include(m => m.Charity)
                .FirstOrDefaultAsync(m => m.MatchId == id);

            if (match == null)
                return NotFound(new { success = false, message = "Match not found" });

            return Ok(new
            {
                success = true,
                matchId = match.MatchId,
                donationId = match.DonationId,
                beneficiaryId = match.BeneficiaryId,
                charityId = match.CharityId,
                urgencyLevel = match.UrgencyLevel,
                distance = match.Distance,
                matchDate = match.MatchDate,
                donationStatus = match.Donation?.Status,
                beneficiaryName = match.Beneficiary?.BeneficiaryNavigation?.Fname,
                charityName = match.Charity?.CharityNavigation?.Fname
            });
        }

        // GET: api/matching/matches-for-charity/{charityId}
        [HttpGet("matches-for-charity/{charityId}")]
        public async Task<IActionResult> GetMatchesForCharity(int charityId)
        {
            var matches = await _context.Matches
                .Where(m => m.CharityId == charityId)
                .Include(m => m.Donation)
                .Include(m => m.Beneficiary)
                .Select(m => new
                {
                    m.MatchId,
                    m.DonationId,
                    BeneficiaryName = $"{m.Beneficiary.BeneficiaryNavigation.Fname} {m.Beneficiary.BeneficiaryNavigation.Lname}",
                    m.UrgencyLevel,
                    m.Distance,
                    m.MatchDate,
                    DonationStatus = m.Donation.Status
                })
                .ToListAsync();

            return Ok(new { success = true, charityId = charityId, count = matches.Count, data = matches });
        }

        // PUT: api/matching/{id}/complete
        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteMatch(int id)
        {
            var match = await _context.Matches.Include(m => m.Donation).FirstOrDefaultAsync(m => m.MatchId == id);

            if (match == null)
                return NotFound(new { success = false, message = "Match not found" });

            match.Donation.Status = "Delivered";
            _context.Matches.Update(match);
            _context.Donations.Update(match.Donation);

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Match completed successfully", matchId = match.MatchId });
        }
    }

    public class CreateMatchRequest
    {
        public int DonationId { get; set; }
        public int BeneficiaryId { get; set; }
        public int CharityId { get; set; }
        public string? UrgencyLevel { get; set; }
        public decimal? Distance { get; set; }
    }
}
