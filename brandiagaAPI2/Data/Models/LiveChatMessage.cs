using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class LiveChatMessage
{
    public Guid MessageId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? AdminId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime SentAt { get; set; }

    public bool IsRead { get; set; }

    public string ClientId { get; set; } = null!;

    public virtual User? Admin { get; set; }

    public virtual User? User { get; set; }
}
