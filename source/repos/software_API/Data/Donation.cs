using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class Donation
{
    public int DonationId { get; set; }

    public string? Status { get; set; }

    public string? Image { get; set; }

    public int DonorId { get; set; }

    public int? LocationId { get; set; }

    public virtual Clothe? Clothe { get; set; }

    public virtual Donor Donor { get; set; } = null!;

    public virtual Food? Food { get; set; }

    public virtual Location? Location { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    public virtual MedicalSupply? MedicalSupply { get; set; }

    public virtual Medicine? Medicine { get; set; }

    public virtual ICollection<StatusHistory> StatusHistories { get; set; } = new List<StatusHistory>();
}
