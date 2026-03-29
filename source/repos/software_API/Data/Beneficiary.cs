using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class Beneficiary
{
    public int BeneficiaryId { get; set; }

    public int? LocationId { get; set; }

    public virtual User BeneficiaryNavigation { get; set; } = null!;

    public virtual Location? Location { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
}
