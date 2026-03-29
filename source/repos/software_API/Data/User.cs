using System;
using System.Collections.Generic;

namespace software_API.Data;

public partial class User
{
    public int UserId { get; set; }

    public string Fname { get; set; } = null!;

    public string Lname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public bool? IsVerified { get; set; }

    public string? UserType { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Beneficiary? Beneficiary { get; set; }

    public virtual Charity? Charity { get; set; }

    public virtual Donor? Donor { get; set; }

    public virtual ICollection<Message> MessageReceivers { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
