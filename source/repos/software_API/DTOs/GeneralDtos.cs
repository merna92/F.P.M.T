namespace software_API.DTOs
{
    // User DTOs
    public class UserDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public bool? IsVerified { get; set; }
        public string? UserType { get; set; }
    }

    public class DonorDto
    {
        public int DonorId { get; set; }
        public UserDto UserInfo { get; set; } = null!;
        public int? DonationCount { get; set; }
    }

    public class BeneficiaryDto
    {
        public int BeneficiaryId { get; set; }
        public UserDto UserInfo { get; set; } = null!;
        public int? LocationId { get; set; }
        public string? LocationName { get; set; }
    }

    public class CharityDto
    {
        public int CharityId { get; set; }
        public UserDto UserInfo { get; set; } = null!;
        public string? LicenseNumber { get; set; }
        public string? CoverageArea { get; set; }
        public int? LocationId { get; set; }
    }

    // Donation DTOs
    public class DonationDto
    {
        public int DonationId { get; set; }
        public string? Status { get; set; }
        public string? Image { get; set; }
        public int DonorId { get; set; }
        public string? DonorName { get; set; }
        public int? LocationId { get; set; }
        public string? LocationName { get; set; }
    }

    public class FoodDonationDto : DonationDto
    {
        public string? ProductName { get; set; }
        public string? FoodType { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public string? Quantity { get; set; }
        public string? Category { get; set; }
        public string? StorageCondition { get; set; }
    }

    public class MedicineDonationDto : DonationDto
    {
        public string? MedicineName { get; set; }
        public string? MedicineType { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public string? Quantity { get; set; }
        public string? SafetyConditions { get; set; }
    }

    public class ClothesDonationDto : DonationDto
    {
        public string? Gender { get; set; }
        public string? Size { get; set; }
        public string? Season { get; set; }
        public string? Condition { get; set; }
        public string? CleaningStatus { get; set; }
    }

    // Location DTOs
    public class LocationDto
    {
        public int LocationId { get; set; }
        public string CityArea { get; set; } = string.Empty;
        public string? GpsCoordinates { get; set; }
    }

    public class CreateLocationDto
    {
        public string CityArea { get; set; } = string.Empty;
        public string? GpsCoordinates { get; set; }
    }

    // Match DTOs
    public class MatchDto
    {
        public int MatchId { get; set; }
        public int DonationId { get; set; }
        public int BeneficiaryId { get; set; }
        public string? BeneficiaryName { get; set; }
        public int CharityId { get; set; }
        public string? CharityName { get; set; }
        public string? UrgencyLevel { get; set; }
        public decimal? Distance { get; set; }
        public DateTime MatchDate { get; set; }
        public string? DonationStatus { get; set; }
    }

    public class CreateMatchDto
    {
        public int DonationId { get; set; }
        public int BeneficiaryId { get; set; }
        public int CharityId { get; set; }
        public string? UrgencyLevel { get; set; }
        public decimal? Distance { get; set; }
    }

    // Statistics DTOs
    public class StatisticsDto
    {
        public int TotalUsers { get; set; }
        public int VerifiedUsers { get; set; }
        public int UnverifiedUsers { get; set; }
        public int TotalDonors { get; set; }
        public int TotalBeneficiaries { get; set; }
        public int TotalCharities { get; set; }
        public int TotalDonations { get; set; }
        public DonationStatusCountDto DonationsByStatus { get; set; } = null!;
        public int TotalMatches { get; set; }
    }

    public class DonationStatusCountDto
    {
        public int Pending { get; set; }
        public int Matched { get; set; }
        public int Delivered { get; set; }
    }

    // Pagination DTOs
    public class PaginationDto<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public List<T> Data { get; set; } = new List<T>();
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
