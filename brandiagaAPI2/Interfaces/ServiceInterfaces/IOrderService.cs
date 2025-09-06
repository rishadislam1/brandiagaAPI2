using brandiagaAPI2.Dtos;
using System.Collections.Generic;

namespace brandiagaAPI2.Interfaces.ServiceInterfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(OrderCreateDto orderCreateDto);
        Task<OrderResponseDto> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
        Task<IEnumerable<OrderResponseDto>> GetOrdersByUserIdAsync(Guid userId);
        Task<OrderResponseDto> UpdateOrderAsync(Guid orderId, OrderUpdateDto orderUpdateDto);
        Task DeleteOrderAsync(Guid orderId);
        Task AddToWishlistAsync(Guid userId, Guid productId); 
        Task DeleteFromWishlistAsync(Guid wishlistId); 
        Task<List<WishlistDto>> GetWishlistAsync(Guid userId);
        Task<List<WishlistDto>> GetAllWishlistAsync();

    }
}
