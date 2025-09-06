using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class Notification
{
    public Guid NotificationId { get; set; }

    public Guid UserId { get; set; }

    public string Type { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime SentAt { get; set; }
}
