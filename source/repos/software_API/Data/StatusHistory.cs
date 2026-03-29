using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class StatusHistory
{
    public int HistoryId { get; set; }

    public int? DonationId { get; set; }

    public string? OldStatus { get; set; }

    public string? NewStatus { get; set; }

    public DateTime? ChangeDate { get; set; }

    public virtual Donation? Donation { get; set; }
}
