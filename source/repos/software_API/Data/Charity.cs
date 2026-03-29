using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class Charity
{
    public int CharityId { get; set; }

    public int? Capacity { get; set; }

    public string? LicenseNumber { get; set; }

    public string? CoverageArea { get; set; }

    public string? Needs { get; set; }

    public int? LocationId { get; set; }

    public virtual User CharityNavigation { get; set; } = null!;

    public virtual ICollection<Evaluate> Evaluates { get; set; } = new List<Evaluate>();

    public virtual Location? Location { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
}
