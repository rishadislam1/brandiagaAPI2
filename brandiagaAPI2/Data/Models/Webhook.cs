using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class Webhook
{
    public Guid WebhookId { get; set; }

    public string Url { get; set; } = null!;

    public string EventType { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}
