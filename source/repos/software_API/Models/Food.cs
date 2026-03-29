namespace software_API.Models
{
    public class Food
    {
        public int DonationID { get; set; }
        public string? Product_Name { get; set; }
        public string? Food_Type { get; set; }
        public DateTime Expiry_Date { get; set; }
        public string? Quantity { get; set; }
    }
}