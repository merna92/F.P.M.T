using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using software_API.Data;

namespace software_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationsController : ControllerBase
    {
        private readonly YadElawnContext _context;

        public DonationsController(YadElawnContext context)
        {
            _context = context;
        }

        // GET: api/donations
        [HttpGet]
        public async Task<IActionResult> GetAllDonations([FromQuery] string? status = null)
        {
            var query = _context.Donations.Include(d => d.Donor).Include(d => d.Location).AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(d => d.Status == status);

            var donations = await query.Select(d => new
            {
                d.DonationId,
                d.Status,
                d.Image,
                DonorName = $"{d.Donor.DonorNavigation.Fname} {d.Donor.DonorNavigation.Lname}",
                DonorPhone = d.Donor.DonorNavigation.Phone,
                Location = d.Location != null ? d.Location.CityArea : null,
                d.LocationId,
                d.DonorId
            }).ToListAsync();

            return Ok(new { success = true, count = donations.Count, data = donations });
        }

        // GET: api/donations/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDonationById(int id)
        {
            var donation = await _context.Donations
                .Include(d => d.Donor)
                .Include(d => d.Location)
                .Include(d => d.Food)
                .Include(d => d.Medicine)
                .Include(d => d.Clothe)
                .Include(d => d.MedicalSupply)
                .FirstOrDefaultAsync(d => d.DonationId == id);

            if (donation == null)
                return NotFound(new { success = false, message = "Donation not found" });

            var response = new
            {
                donation.DonationId,
                donation.Status,
                donation.Image,
                DonorInfo = new
                {
                    DonorId = donation.Donor.DonorId,
                    Name = $"{donation.Donor.DonorNavigation.Fname} {donation.Donor.DonorNavigation.Lname}",
                    Phone = donation.Donor.DonorNavigation.Phone,
                    Email = donation.Donor.DonorNavigation.Email
                },
                Location = donation.Location != null ? new
                {
                    donation.Location.LocationId,
                    donation.Location.CityArea,
                    donation.Location.GpsCoordinates
                } : null,
                Food = donation.Food,
                Medicine = donation.Medicine,
                Clothe = donation.Clothe,
                MedicalSupply = donation.MedicalSupply
            };

            return Ok(new { success = true, data = response });
        }

        // POST: api/donations/food
        [HttpPost("food")]
        public async Task<IActionResult> CreateFoodDonation([FromBody] CreateFoodDonationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid request data" });

            var donor = await _context.Donors.FirstOrDefaultAsync(d => d.DonorId == request.DonorId);
            if (donor == null)
                return NotFound(new { success = false, message = "Donor not found" });

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var donation = new Donation
                    {
                        Status = "Pending",
                        Image = request.Image,
                        DonorId = request.DonorId,
                        LocationId = request.LocationId
                    };

                    _context.Donations.Add(donation);
                    await _context.SaveChangesAsync();

                    var food = new Food
                    {
                        DonationId = donation.DonationId,
                        ProductName = request.ProductName,
                        FoodType = request.FoodType,
                        ExpiryDate = DateOnly.FromDateTime(request.ExpiryDate),
                        Quantity = request.Quantity,
                        Category = request.Category,
                        StorageCondition = request.StorageCondition,
                        ShelfLife = request.ShelfLife
                    };

                    _context.Foods.Add(food);
                    donor.DonationCount = (donor.DonationCount ?? 0) + 1;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return CreatedAtAction(nameof(GetDonationById), new { id = donation.DonationId }, new
                    {
                        success = true,
                        message = "Food donation created successfully",
                        donationId = donation.DonationId
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new { success = false, message = "Error creating donation", error = ex.Message });
                }
            }
        }

        // POST: api/donations/medicine
        [HttpPost("medicine")]
        public async Task<IActionResult> CreateMedicineDonation([FromBody] CreateMedicineDonationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid request data" });

            var donor = await _context.Donors.FirstOrDefaultAsync(d => d.DonorId == request.DonorId);
            if (donor == null)
                return NotFound(new { success = false, message = "Donor not found" });

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var donation = new Donation
                    {
                        Status = "Pending",
                        Image = request.Image,
                        DonorId = request.DonorId,
                        LocationId = request.LocationId
                    };

                    _context.Donations.Add(donation);
                    await _context.SaveChangesAsync();

                    var medicine = new Data.Medicine
                    {
                        DonationId = donation.DonationId,
                        MedicineName = request.MedicineName,
                        MedicineType = request.MedicineType,
                        ExpiryDate = DateOnly.FromDateTime(request.ExpiryDate),
                        Quantity = request.Quantity,
                        SafetyConditions = request.SafetyConditions
                    };

                    _context.Medicines.Add(medicine);
                    donor.DonationCount = (donor.DonationCount ?? 0) + 1;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return CreatedAtAction(nameof(GetDonationById), new { id = donation.DonationId }, new
                    {
                        success = true,
                        message = "Medicine donation created successfully",
                        donationId = donation.DonationId
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new { success = false, message = "Error creating donation", error = ex.Message });
                }
            }
        }

        // POST: api/donations/clothes
        [HttpPost("clothes")]
        public async Task<IActionResult> CreateClothesDonation([FromBody] CreateClothesDonationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid request data" });

            var donor = await _context.Donors.FirstOrDefaultAsync(d => d.DonorId == request.DonorId);
            if (donor == null)
                return NotFound(new { success = false, message = "Donor not found" });

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var donation = new Donation
                    {
                        Status = "Pending",
                        Image = request.Image,
                        DonorId = request.DonorId,
                        LocationId = request.LocationId
                    };

                    _context.Donations.Add(donation);
                    await _context.SaveChangesAsync();

                    var clothe = new Clothe
                    {
                        DonationId = donation.DonationId,
                        Gender = request.Gender,
                        Size = request.Size,
                        Season = request.Season,
                        Condition = request.Condition,
                        CleaningStatus = request.CleaningStatus
                    };

                    _context.Clothes.Add(clothe);
                    donor.DonationCount = (donor.DonationCount ?? 0) + 1;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return CreatedAtAction(nameof(GetDonationById), new { id = donation.DonationId }, new
                    {
                        success = true,
                        message = "Clothes donation created successfully",
                        donationId = donation.DonationId
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new { success = false, message = "Error creating donation", error = ex.Message });
                }
            }
        }

        // PUT: api/donations/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateDonationStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            var donation = await _context.Donations.FirstOrDefaultAsync(d => d.DonationId == id);

            if (donation == null)
                return NotFound(new { success = false, message = "Donation not found" });

            var oldStatus = donation.Status;
            donation.Status = request.Status;

            _context.Donations.Update(donation);

            var statusHistory = new StatusHistory
            {
                DonationId = id,
                OldStatus = oldStatus,
                NewStatus = request.Status,
                ChangeDate = DateTime.UtcNow
            };

            _context.StatusHistories.Add(statusHistory);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Donation status updated successfully", status = donation.Status });
        }

        // DELETE: api/donations/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonation(int id)
        {
            var donation = await _context.Donations
                .Include(d => d.Food)
                .Include(d => d.Medicine)
                .Include(d => d.Clothe)
                .FirstOrDefaultAsync(d => d.DonationId == id);

            if (donation == null)
                return NotFound(new { success = false, message = "Donation not found" });

            _context.Donations.Remove(donation);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Donation deleted successfully" });
        }
    }

    // Request Models
    public class CreateFoodDonationRequest
    {
        public int DonorId { get; set; }
        public int? LocationId { get; set; }
        public string? Image { get; set; }
        public string ProductName { get; set; }
        public string FoodType { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Quantity { get; set; }
        public string? Category { get; set; }
        public string? StorageCondition { get; set; }
        public string? ShelfLife { get; set; }
    }

    public class CreateMedicineDonationRequest
    {
        public int DonorId { get; set; }
        public int? LocationId { get; set; }
        public string? Image { get; set; }
        public string MedicineName { get; set; }
        public string MedicineType { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Quantity { get; set; }
        public string? SafetyConditions { get; set; }
    }

    public class CreateClothesDonationRequest
    {
        public int DonorId { get; set; }
        public int? LocationId { get; set; }
        public string? Image { get; set; }
        public string? Gender { get; set; }
        public string? Size { get; set; }
        public string? Season { get; set; }
        public string? Condition { get; set; }
        public string? CleaningStatus { get; set; }
    }

    public class UpdateStatusRequest
    {
        public string Status { get; set; }
    }
}
