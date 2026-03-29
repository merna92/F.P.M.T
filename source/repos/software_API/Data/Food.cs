using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class Food
{
    public int DonationId { get; set; }

    public string? ProductName { get; set; }

    public string? FoodType { get; set; }

    public string? Category { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public string? Quantity { get; set; }

    public string? StorageCondition { get; set; }

    public string? ShelfLife { get; set; }

    public virtual Donation Donation { get; set; } = null!;
}
