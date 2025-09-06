using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace brandiagaAPI2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TrackingController : ControllerBase
{
    private readonly ITrackingService _trackingService;

    public TrackingController(ITrackingService trackingService)
    {
        _trackingService = trackingService;
    }

    [HttpGet("track")]
    public async Task<IActionResult> TrackOrder([FromQuery] string orderNumber, [FromQuery] string email)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(orderNumber) || string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(ResponseDTO<object>.Error("Order number and email are required."));
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^\S+@\S+\.\S+$"))
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid email format."));
            }

            var trackingData = await _trackingService.GetOrderTrackingAsync(orderNumber, email);
            if (trackingData == null)
            {
                return NotFound(ResponseDTO<object>.Error("Order not found or email does not match."));
            }

            return Ok(ResponseDTO<OrderTrackingDto>.Success(trackingData, "Tracking information retrieved successfully."));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseDTO<object>.Error($"Failed to retrieve tracking: {ex.Message}"));
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllShippings()
    {
        try
        {
            var shippings = await _trackingService.GetAllShippingsAsync();
            return Ok(ResponseDTO<List<OrderTrackingDto>>.Success(shippings, "Shipping records retrieved successfully."));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseDTO<object>.Error($"Failed to retrieve shippings: {ex.Message}"));
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateOrderTracking([FromBody] CreateTrackingDto trackingDto)
    {
        try
        {
            if (trackingDto == null || trackingDto.OrderId == Guid.Empty)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid tracking data or OrderId."));
            }

            var existingShipping = await _trackingService.GetOrderTrackingAsync($"B-{trackingDto.OrderId}", "");
            if (existingShipping != null)
            {
                return BadRequest(ResponseDTO<object>.Error("Shipping record already exists for this order."));
            }

            var success = await _trackingService.CreateOrderTrackingAsync(trackingDto);
            if (!success)
            {
                return NotFound(ResponseDTO<object>.Error("Order or shipping method not found."));
            }

            return CreatedAtAction(
                nameof(TrackOrder),
                new { orderNumber = $"B-{trackingDto.OrderId}", email = "" },
                ResponseDTO<object>.Success(null, "Tracking information created successfully.")
            );
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseDTO<object>.Error($"Failed to create tracking: {ex.Message}"));
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{orderId}")]
    public async Task<IActionResult> UpdateOrderTracking(Guid orderId, [FromBody] UpdateTrackingDto trackingDto)
    {
        try
        {
            if (trackingDto == null || orderId == Guid.Empty)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid tracking data or OrderId."));
            }

            var success = await _trackingService.UpdateOrderTrackingAsync(orderId, trackingDto);
            if (!success)
            {
                return NotFound(ResponseDTO<object>.Error("Shipping record not found or was modified/deleted. Please refresh and try again."));
            }

            return Ok(ResponseDTO<object>.Success(null, "Tracking information updated successfully."));
        }
        catch (Exception ex)
        {
            return BadRequest(ResponseDTO<object>.Error($"Failed to update shipping record: {ex.Message}"));
        }
    }

  
}