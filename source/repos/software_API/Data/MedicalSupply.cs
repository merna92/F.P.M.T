using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class MedicalSupply
{
    public int DonationId { get; set; }

    public string? SupplyName { get; set; }

    public string? Condition { get; set; }

    public string? Quantity { get; set; }

    public virtual Donation Donation { get; set; } = null!;
}
