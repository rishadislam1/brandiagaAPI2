using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Data.Models;

public partial class OrderShipping
{
    public Guid OrderShippingId { get; set; }

    public Guid OrderId { get; set; }

    public Guid ShippingMethodId { get; set; }

    public string? TrackingNumber { get; set; }

    public DateTime? EstimatedDelivery { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual ShippingMethod ShippingMethod { get; set; } = null!;

    public virtual ICollection<TrackingEvent> TrackingEvents { get; set; } = new List<TrackingEvent>();
}
