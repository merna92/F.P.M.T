using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class Location
{
    public int LocationId { get; set; }

    public string CityArea { get; set; } = null!;

    public string? GpsCoordinates { get; set; }

    public virtual ICollection<Beneficiary> Beneficiaries { get; set; } = new List<Beneficiary>();

    public virtual ICollection<Charity> Charities { get; set; } = new List<Charity>();

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();
}
