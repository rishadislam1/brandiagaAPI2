using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace brandiagaAPI2.Services;

public class TrackingService : ITrackingService
{
    private readonly ITrackingRepository _trackingRepository;

    public TrackingService(ITrackingRepository trackingRepository)
    {
        _trackingRepository = trackingRepository;
    }

    public async Task<OrderTrackingDto?> GetOrderTrackingAsync(string orderNumber, string email)
    {
        var order = await _trackingRepository.GetOrderTrackingAsync(orderNumber, email);
        if (order == null || !order.OrderShippings.Any())
        {
            return null;
        }

        var orderShipping = order.OrderShippings.First();

        return new OrderTrackingDto
        {
            OrderNumber = $"B-{order.OrderId}",
            Status = order.Status,
            EstimatedDelivery = orderShipping.EstimatedDelivery,
            ShippingAddress = order.User.UserAddress ?? "Unknown",
            Carrier = orderShipping.ShippingMethod.Name,
            TrackingNumber = orderShipping.TrackingNumber,
            Items = order.OrderItems.Select(oi => new OrderItemDto1
            {
                Name = oi.Product.Name ?? "Unknown",
                Quantity = oi.Quantity,
                Status = "Shipped"
            }).ToList(),
            TrackingHistory = orderShipping.TrackingEvents
                .OrderByDescending(te => te.EventDate)
                .Select(te => new TrackingEventDto
                {
                    Date = te.EventDate.ToString("MMMM dd, yyyy"),
                    Time = te.EventDate.ToString("hh:mm tt"),
                    Status = te.Status,
                    Location = te.Location,
                    IsCompleted = true
                }).ToList()
        };
    }

    public async Task<List<OrderTrackingDto>> GetAllShippingsAsync()
    {
        var orders = await _trackingRepository.GetAllShippingsAsync();
        return orders.Select(order =>
        {
            var orderShipping = order.OrderShippings.FirstOrDefault();
            return new OrderTrackingDto
            {
                OrderNumber = $"B-{order.OrderId}",
                Status = order.Status,
                EstimatedDelivery = orderShipping?.EstimatedDelivery,
                ShippingAddress = order.User.UserAddress ?? "Unknown",
                Carrier = orderShipping?.ShippingMethod.Name ?? "Unknown",
                TrackingNumber = orderShipping?.TrackingNumber,
                Items = order.OrderItems.Select(oi => new OrderItemDto1
                {
                    Name = oi.Product.Name ?? "Unknown",
                    Quantity = oi.Quantity,
                    Status = "Shipped"
                }).ToList(),
                TrackingHistory = orderShipping?.TrackingEvents
                    .OrderByDescending(te => te.EventDate)
                    .Select(te => new TrackingEventDto
                    {
                        Date = te.EventDate.ToString("MMMM dd, yyyy"),
                        Time = te.EventDate.ToString("hh:mm tt"),
                        Status = te.Status,
                        Location = te.Location,
                        IsCompleted = true
                    }).ToList() ?? new List<TrackingEventDto>()
            };
        }).ToList();
    }

    public async Task<bool> CreateOrderTrackingAsync(CreateTrackingDto trackingDto)
    {
        if (trackingDto.OrderId == Guid.Empty || trackingDto.ShippingMethodId == Guid.Empty)
        {
            return false;
        }

        return await _trackingRepository.CreateOrderShippingAsync(
            trackingDto.OrderId,
            trackingDto.ShippingMethodId,
            trackingDto.TrackingNumber,
            trackingDto.EstimatedDelivery,
            trackingDto.TrackingEvents
        );
    }

    public async Task<bool> UpdateOrderTrackingAsync(Guid orderId, UpdateTrackingDto trackingDto)
    {
        if (orderId == Guid.Empty)
        {
            return false;
        }

        return await _trackingRepository.UpdateOrderShippingAsync(
            orderId,
            trackingDto.TrackingNumber,
            trackingDto.EstimatedDelivery,
            trackingDto.TrackingEvents
        );
    }
}