using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class Medicine
{
    public int DonationId { get; set; }

    public string? MedicineName { get; set; }

    public string? MedicineType { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public string? Quantity { get; set; }

    public string? SafetyConditions { get; set; }

    public virtual Donation Donation { get; set; } = null!;
}
