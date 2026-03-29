namespace software_API.Models
{
    public class Donation
    {
        public int DonationID { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Image { get; set; }
        public int DonorID { get; set; }
        public int? LocationID { get; set; }
    }
}