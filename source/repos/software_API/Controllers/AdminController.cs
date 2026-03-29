using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using software_API.Data;

namespace software_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly YadElawnContext _context;

        public AdminController(YadElawnContext context)
        {
            _context = context;
        }

        // GET: api/admin/statistics
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalDonors = await _context.Donors.CountAsync();
            var totalBeneficiaries = await _context.Beneficiaries.CountAsync();
            var totalDonations = await _context.Donations.CountAsync();
            var totalMatches = await _context.Matches.CountAsync();
            var totalCharities = await _context.Charities.CountAsync();

            var pendingDonations = await _context.Donations.CountAsync(d => d.Status == "Pending");
            var deliveredDonations = await _context.Donations.CountAsync(d => d.Status == "Delivered");
            var matchedDonations = await _context.Donations.CountAsync(d => d.Status == "Matched");

            var verifiedUsers = await _context.Users.CountAsync(u => u.IsVerified == true);
            var unverifiedUsers = totalUsers - verifiedUsers;

            return Ok(new
            {
                success = true,
                totalUsers,
                verifiedUsers,
                unverifiedUsers,
                totalDonors,
                totalBeneficiaries,
                totalCharities,
                totalDonations,
                donationsByStatus = new
                {
                    pending = pendingDonations,
                    matched = matchedDonations,
                    delivered = deliveredDonations
                },
                totalMatches
            });
        }

        // GET: api/admin/unverified-users
        [HttpGet("unverified-users")]
        public async Task<IActionResult> GetUnverifiedUsers()
        {
            var unverifiedUsers = await _context.Users
                .Where(u => u.IsVerified == false)
                .Select(u => new
                {
                    u.UserId,
                    FullName = $"{u.Fname} {u.Lname}",
                    u.Email,
                    u.Phone,
                    u.UserType
                })
                .ToListAsync();

            return Ok(new { success = true, count = unverifiedUsers.Count, data = unverifiedUsers });
        }

        // PUT: api/admin/verify-user/{userId}
        [HttpPut("verify-user/{userId}")]
        public async Task<IActionResult> VerifyUser(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                return NotFound(new { success = false, message = "User not found" });

            user.IsVerified = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "User verified successfully" });
        }

        // GET: api/admin/pending-donations
        [HttpGet("pending-donations")]
        public async Task<IActionResult> GetPendingDonations()
        {
            var pendingDonations = await _context.Donations
                .Where(d => d.Status == "Pending")
                .Include(d => d.Donor)
                .Include(d => d.Location)
                .Select(d => new
                {
                    d.DonationId,
                    DonorName = $"{d.Donor.DonorNavigation.Fname} {d.Donor.DonorNavigation.Lname}",
                    Location = d.Location != null ? d.Location.CityArea : null,
                    d.Status
                })
                .ToListAsync();

            return Ok(new { success = true, count = pendingDonations.Count, data = pendingDonations });
        }

        // GET: api/admin/audit-logs
        [HttpGet("audit-logs")]
        public async Task<IActionResult> GetAuditLogs([FromQuery] int limit = 100)
        {
            var logs = await _context.AuditLogs
                .Include(a => a.Admin)
                .OrderByDescending(a => a.ActionDate)
                .Take(limit)
                .Select(a => new
                {
                    a.LogId,
                    AdminName = $"{a.Admin.AdminNavigation.Fname} {a.Admin.AdminNavigation.Lname}",
                    a.ActionTaken,
                    a.ActionDate
                })
                .ToListAsync();

            return Ok(new { success = true, count = logs.Count, data = logs });
        }

        // POST: api/admin/audit-log
        [HttpPost("audit-log")]
        public async Task<IActionResult> CreateAuditLog([FromBody] CreateAuditLogRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid request data" });

            var auditLog = new AuditLog
            {
                AdminId = request.AdminId,
                ActionTaken = request.ActionTaken,
                ActionDate = DateTime.UtcNow
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Audit log created successfully", logId = auditLog.LogId });
        }

        // GET: api/admin/donations-by-type
        [HttpGet("donations-by-type")]
        public async Task<IActionResult> GetDonationsByType()
        {
            var foodDonations = await _context.Foods.CountAsync();
            var medicineDonations = await _context.Medicines.CountAsync();
            var clothesDonations = await _context.Clothes.CountAsync();
            var medicalSupplies = await _context.MedicalSupplies.CountAsync();

            return Ok(new
            {
                success = true,
                foodDonations,
                medicineDonations,
                clothesDonations,
                medicalSupplies,
                total = foodDonations + medicineDonations + clothesDonations + medicalSupplies
            });
        }

        // GET: api/admin/top-locations
        [HttpGet("top-locations")]
        public async Task<IActionResult> GetTopLocations()
        {
            var topLocations = await _context.Donations
                .Where(d => d.LocationId != null)
                .GroupBy(d => d.LocationId)
                .Select(g => new
                {
                    LocationId = g.Key,
                    DonationsCount = g.Count()
                })
                .OrderByDescending(x => x.DonationsCount)
                .Take(10)
                .ToListAsync();

            var result = new List<object>();

            foreach (var item in topLocations)
            {
                var location = await _context.Locations.FirstOrDefaultAsync(l => l.LocationId == item.LocationId);
                result.Add(new
                {
                    LocationId = item.LocationId,
                    CityArea = location?.CityArea,
                    DonationsCount = item.DonationsCount
                });
            }

            return Ok(new { success = true, count = result.Count, data = result });
        }
    }

    public class CreateAuditLogRequest
    {
        public int AdminId { get; set; }
        public string ActionTaken { get; set; }
    }
}
