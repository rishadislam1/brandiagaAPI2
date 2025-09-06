using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace brandiagaAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid input data."));
            }

            try
            {
                var order = await _orderService.CreateOrderAsync(orderCreateDto);
                return Ok(ResponseDTO<OrderResponseDto>.Success(order, "Order created successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                return Ok(ResponseDTO<OrderResponseDto>.Success(order, "Order retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(ResponseDTO<IEnumerable<OrderResponseDto>>.Success(orders, "Orders retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetOrdersByUserId(Guid userId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByUserIdAsync(userId);
                return Ok(ResponseDTO<IEnumerable<OrderResponseDto>>.Success(orders, "User orders retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{orderId:guid}")]
        public async Task<IActionResult> UpdateOrder(Guid orderId, [FromBody] OrderUpdateDto orderUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid input data."));
            }

            try
            {
                var order = await _orderService.UpdateOrderAsync(orderId, orderUpdateDto);
                return Ok(ResponseDTO<OrderResponseDto>.Success(order, "Order updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{orderId:guid}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            try
            {
                await _orderService.DeleteOrderAsync(orderId);
                return Ok(ResponseDTO<object>.Success(null, "Order deleted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [HttpPost("wishlist")]
        public async Task<IActionResult> AddToWishlist([FromBody] AddToWishlistDTO request)
        {
            try
            {
                await _orderService.AddToWishlistAsync(request.UserId, request.ProductId);
                
                return Ok(ResponseDTO<object>.Success(null, "Added to wishlist"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
          
        }
        [HttpDelete("wishlist/{wishlistId}")]
        public async Task<IActionResult> DeleteFromWishlistController(Guid wishlistId)
        {

            try
            {
                await _orderService.DeleteFromWishlistAsync(wishlistId);
      

                return Ok(ResponseDTO<object>.Success(null, "Wishlist Deleted Successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
            
        }
        [HttpGet("wishlist/{userId}")]
        public async Task<IActionResult> GetWishlistController(Guid userId)
        {
            try
            {
                var wishlist = await _orderService.GetWishlistAsync(userId);
            


                return Ok(ResponseDTO<object>.Success(wishlist, "Wishlist Deleted Successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
         
        }
        [HttpGet("wishlistAll")]
        public async Task<IActionResult> GetAllWishListController()
        {
            try
            {
                var wishlist = await _orderService.GetAllWishlistAsync();
           



                return Ok(ResponseDTO<object>.Success(wishlist, "Wishlist Deleted Successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
         
        }
    }
}
