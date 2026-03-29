using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class Admin
{
    public int AdminId { get; set; }

    public virtual User AdminNavigation { get; set; } = null!;

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
