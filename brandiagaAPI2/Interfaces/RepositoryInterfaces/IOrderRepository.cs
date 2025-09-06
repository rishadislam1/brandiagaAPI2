using brandiagaAPI2.Data.Models;
using System.Collections.Generic;

namespace brandiagaAPI2.Interfaces.RepositoryInterfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(Guid orderId);
        Task AddToWishlistAsync(Wishlist wishlist); 
        Task DeleteFromWishlistAsync(Guid wishlistId); 
        Task<IEnumerable<Wishlist>> GetWishlistAsync(Guid userId);
        Task<IEnumerable<Wishlist>> GetAllWishListAsync();
    }
}
