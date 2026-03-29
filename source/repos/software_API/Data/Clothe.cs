using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class Clothe
{
    public int DonationId { get; set; }

    public string? Season { get; set; }

    public string? Gender { get; set; }

    public string? Size { get; set; }

    public string? Condition { get; set; }

    public string? CleaningStatus { get; set; }

    public virtual Donation Donation { get; set; } = null!;
}
