using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace brandiagaAPI2.Interfaces;

public interface ITrackingRepository
{
    Task<Order?> GetOrderTrackingAsync(string orderNumber, string email);
    Task<OrderShipping?> GetOrderShippingAsync(Guid orderId);
    Task<bool> CreateOrderShippingAsync(Guid orderId, Guid shippingMethodId, string? trackingNumber, DateTime? estimatedDelivery, List<CreateTrackingEventDto> trackingEvents);
    Task<bool> UpdateOrderShippingAsync(Guid orderId, string? trackingNumber, DateTime? estimatedDelivery, List<CreateTrackingEventDto> trackingEvents);
    Task<List<Order>> GetAllShippingsAsync();
}