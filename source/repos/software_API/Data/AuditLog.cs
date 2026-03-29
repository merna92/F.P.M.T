using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class AuditLog
{
    public int LogId { get; set; }

    public int? AdminId { get; set; }

    public string? ActionTaken { get; set; }

    public DateTime? ActionDate { get; set; }

    public virtual Admin? Admin { get; set; }
}
