using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class TrackingEvent
{
    public Guid TrackingEventId { get; set; }

    public Guid OrderShippingId { get; set; }

    public DateTime EventDate { get; set; }

    public string Status { get; set; } = null!;

    public string? Location { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual OrderShipping OrderShipping { get; set; } = null!;
}
