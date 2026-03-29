using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using software_API.Data;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace software_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly YadElawnContext _context;

        public AuthController(YadElawnContext context)
        {
            _context = context;
        }

        // POST: api/auth/register-donor
        [HttpPost("register-donor")]
        public async Task<IActionResult> RegisterDonor([FromBody] UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(new { success = false, message = "Invalid data", errors = errors.Select(e => e.ErrorMessage) });
            }

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest(new { success = false, message = "Email already exists" });

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = new User
                    {
                        Fname = request.FirstName,
                        Lname = request.LastName,
                        Email = request.Email,
                        Password = HashPassword(request.Password),
                        Phone = request.Phone,
                        Address = request.Address,
                        IsVerified = false,
                        UserType = "Donor"
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    var donor = new Donor
                    {
                        DonorId = user.UserId,
                        DonationCount = 0
                    };

                    _context.Donors.Add(donor);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok(new
                    {
                        success = true,
                        message = "Registration successful",
                        userId = user.UserId,
                        userType = "Donor"
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new { success = false, message = "Registration error", error = ex.Message });
                }
            }
        }

        // POST: api/auth/register-beneficiary
        [HttpPost("register-beneficiary")]
        public async Task<IActionResult> RegisterBeneficiary([FromBody] BeneficiaryRegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(new { success = false, message = "Invalid data", errors = errors.Select(e => e.ErrorMessage) });
            }

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return BadRequest(new { success = false, message = "Email already exists" });

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var user = new User
                    {
                        Fname = request.FirstName,
                        Lname = request.LastName,
                        Email = request.Email,
                        Password = HashPassword(request.Password),
                        Phone = request.Phone,
                        Address = request.Address,
                        IsVerified = false,
                        UserType = "Beneficiary"
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    var beneficiary = new Beneficiary
                    {
                        BeneficiaryId = user.UserId,
                        LocationId = request.LocationId
                    };

                    _context.Beneficiaries.Add(beneficiary);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok(new
                    {
                        success = true,
                        message = "Registration successful",
                        userId = user.UserId,
                        userType = "Beneficiary"
                    });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new { success = false, message = "Registration error", error = ex.Message });
                }
            }
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(new { success = false, message = "Invalid data", errors = errors.Select(e => e.ErrorMessage) });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !VerifyPassword(request.Password, user.Password))
                return Unauthorized(new { success = false, message = "Invalid credentials" });

            return Ok(new
            {
                success = true,
                message = "Login successful",
                userId = user.UserId,
                fullName = $"{user.Fname} {user.Lname}",
                email = user.Email,
                userType = user.UserType
            });
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }
    }

    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }
    }

    public class BeneficiaryRegisterRequest
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        public int? LocationId { get; set; }
    }

    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;
    }
}
