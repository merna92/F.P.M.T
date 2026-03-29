using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class Match
{
    public int MatchId { get; set; }

    public int? DonationId { get; set; }

    public int? CharityId { get; set; }

    public int? BeneficiaryId { get; set; }

    public decimal? Distance { get; set; }

    public string? UrgencyLevel { get; set; }

    public DateTime? MatchDate { get; set; }

    public virtual Beneficiary? Beneficiary { get; set; }

    public virtual Charity? Charity { get; set; }

    public virtual Donation? Donation { get; set; }
}
