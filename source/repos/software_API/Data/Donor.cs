using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class Donor
{
    public int DonorId { get; set; }

    public int? DonationCount { get; set; }

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();

    public virtual User DonorNavigation { get; set; } = null!;

    public virtual ICollection<Evaluate> Evaluates { get; set; } = new List<Evaluate>();
}
