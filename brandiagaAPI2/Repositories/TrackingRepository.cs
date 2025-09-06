using brandiagaAPI2.Data;
using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brandiagaAPI2.Repositories;

public class TrackingRepository : ITrackingRepository
{
    private readonly DbAbdeaeDotnet1Context _context;

    public TrackingRepository(DbAbdeaeDotnet1Context context)
    {
        _context = context;
    }

    public async Task<Order?> GetOrderTrackingAsync(string orderNumber, string email)
    {
       // orderNumber = orderNumber.Replace("B-", "");
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.OrderShippings)
                .ThenInclude(os => os.ShippingMethod)
            .Include(o => o.OrderShippings)
                .ThenInclude(os => os.TrackingEvents)
            .FirstOrDefaultAsync(o =>
                o.OrderId.ToString() == orderNumber &&
                o.User.Email == email);
    }

    public async Task<List<Order>> GetAllShippingsAsync()
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.OrderShippings)
                .ThenInclude(os => os.ShippingMethod)
            .Include(o => o.OrderShippings)
                .ThenInclude(os => os.TrackingEvents)
            .Where(o => o.OrderShippings.Any())
            .ToListAsync();
    }

    public async Task<OrderShipping?> GetOrderShippingAsync(Guid orderId)
    {
        return await _context.OrderShippings
            .Include(os => os.ShippingMethod)
            .Include(os => os.TrackingEvents)
            .FirstOrDefaultAsync(os => os.OrderId == orderId);
    }

    public async Task<bool> CreateOrderShippingAsync(Guid orderId, Guid shippingMethodId, string? trackingNumber, DateTime? estimatedDelivery, List<CreateTrackingEventDto> trackingEvents)
    {
        var orderExists = await _context.Orders.AnyAsync(o => o.OrderId == orderId);
        if (!orderExists)
        {
            return false;
        }

        var shippingMethodExists = await _context.ShippingMethods.AnyAsync(sm => sm.ShippingMethodId == shippingMethodId);
        if (!shippingMethodExists)
        {
            return false;
        }

        var orderShipping = new OrderShipping
        {
            OrderShippingId = Guid.NewGuid(),
            OrderId = orderId,
            ShippingMethodId = shippingMethodId,
            TrackingNumber = trackingNumber,
            EstimatedDelivery = estimatedDelivery,
            TrackingEvents = trackingEvents.Select(te => new TrackingEvent
            {
                TrackingEventId = Guid.NewGuid(),
                EventDate = te.EventDate,
                Status = te.Status,
                Location = te.Location,
                CreatedAt = DateTime.UtcNow
            }).ToList()
        };

        await _context.OrderShippings.AddAsync(orderShipping);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateOrderShippingAsync(Guid orderId, string? trackingNumber, DateTime? estimatedDelivery, List<CreateTrackingEventDto> trackingEvents)
    {
        var orderShipping = await _context.OrderShippings
            .Include(os => os.TrackingEvents)
            .FirstOrDefaultAsync(os => os.OrderId == orderId);

        if (orderShipping == null)
        {
            return false; // No record found, return false as expected
        }

        try
        {
            orderShipping.TrackingNumber = trackingNumber ?? orderShipping.TrackingNumber;
            orderShipping.EstimatedDelivery = estimatedDelivery ?? orderShipping.EstimatedDelivery;



            //// Remove existing tracking events
            //_context.TrackingEvents.RemoveRange(orderShipping.TrackingEvents);
            //orderShipping.TrackingEvents.Clear();

            //// Add new tracking events
            var newTrackingEvent = trackingEvents.Select(te => new TrackingEvent
            {
                TrackingEventId = Guid.NewGuid(),
                OrderShippingId = orderShipping.OrderShippingId,
                EventDate = te.EventDate,
                Status = te.Status,
                Location = te.Location,
                CreatedAt = DateTime.UtcNow,
            });




            _context.TrackingEvents.AddRange(newTrackingEvent);

            _context.OrderShippings.Update(orderShipping);
            var rowsAffected = await _context.SaveChangesAsync();
            return rowsAffected > 0;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Log the concurrency exception for debugging
            Console.WriteLine($"Concurrency error: {ex.Message}");
            return false; // Handle concurrency conflict
        }
        catch (Exception ex)
        {
            // Log other exceptions
            Console.WriteLine($"Update error: {ex.Message}");
            throw; // Rethrow to let the controller handle the error
        }
    }
}