using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class VwAvailableFoodDonation
{
    public int DonationId { get; set; }

    public string? ProductName { get; set; }

    public string? Quantity { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public string DonorName { get; set; } = null!;

    public string CityArea { get; set; } = null!;
}
