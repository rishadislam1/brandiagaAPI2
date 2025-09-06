using System;
using System.Collections.Generic;

namespace brandiagaAPI2.Dtos;

public class OrderTrackingDto
{
    public string OrderNumber { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime? EstimatedDelivery { get; set; }
    public string ShippingAddress { get; set; } = null!;
    public string Carrier { get; set; } = null!;
    public string? TrackingNumber { get; set; }
    public List<OrderItemDto1> Items { get; set; } = new List<OrderItemDto1>();
    public List<TrackingEventDto> TrackingHistory { get; set; } = new List<TrackingEventDto>();
}

public class OrderItemDto1
{
    public string Name { get; set; } = null!;
    public int Quantity { get; set; }
    public string Status { get; set; } = null!;
}

public class TrackingEventDto
{
    public string Date { get; set; } = null!;
    public string Time { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? Location { get; set; }
    public bool IsCompleted { get; set; }
}

public class CreateTrackingDto
{
    public Guid OrderId { get; set; }
    public Guid ShippingMethodId { get; set; }
    public string? TrackingNumber { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
    public List<CreateTrackingEventDto> TrackingEvents { get; set; } = new List<CreateTrackingEventDto>();
}

public class CreateTrackingEventDto
{
    public DateTime EventDate { get; set; }
    public string Status { get; set; } = null!;
    public string? Location { get; set; }
}

public class UpdateTrackingDto
{
    public string? TrackingNumber { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
    public List<CreateTrackingEventDto> TrackingEvents { get; set; } = new List<CreateTrackingEventDto>();
}