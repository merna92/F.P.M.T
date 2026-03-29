using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class Evaluate
{
    public int CharityId { get; set; }

    public int DonorId { get; set; }

    public int? Score { get; set; }

    public string? Comment { get; set; }

    public virtual Charity Charity { get; set; } = null!;

    public virtual Donor Donor { get; set; } = null!;
}
