using brandiagaAPI2.Dtos;
using System;
using System.Threading.Tasks;

namespace brandiagaAPI2.Interfaces;

public interface ITrackingService
{
    Task<OrderTrackingDto?> GetOrderTrackingAsync(string orderNumber, string email);
    Task<bool> CreateOrderTrackingAsync(CreateTrackingDto trackingDto);
    Task<bool> UpdateOrderTrackingAsync(Guid orderId, UpdateTrackingDto trackingDto);
    Task<List<OrderTrackingDto>> GetAllShippingsAsync();
}