using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class Notification
{
    public int NotifId { get; set; }

    public int? UserId { get; set; }

    public string? Content { get; set; }

    public DateTime? Timestamp { get; set; }

    public bool? IsRead { get; set; }

    public virtual User? User { get; set; }
}
