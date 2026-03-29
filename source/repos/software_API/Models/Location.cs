namespace software_API.Models
{
    public class Location
    {
        public int LocationID { get; set; }
        public string City_Area { get; set; } = string.Empty;
        public string? GPS_Coordinates { get; set; }
    }
}