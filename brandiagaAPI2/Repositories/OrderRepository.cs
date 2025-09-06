using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace brandiagaAPI2.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbAbdeaeDotnet1Context _context;

        public OrderRepository(DbAbdeaeDotnet1Context context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order != null)
            {
                _context.OrderItems.RemoveRange(order.OrderItems);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddToWishlistAsync(Wishlist wishlist)
        {
            await _context.Wishlists.AddAsync(wishlist);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFromWishlistAsync(Guid wishlistId)
        {
            var wishlist = await _context.Wishlists.FindAsync(wishlistId);
            if(wishlist != null)
            {
                _context.Wishlists.Remove(wishlist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Wishlist>> GetWishlistAsync(Guid userId)
        {
            return await _context.Wishlists
                .Include(w=> w.Product)
                .Include(w=>w.User)
                .Where(w=>w.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Wishlist>> GetAllWishListAsync()
        {
            return await _context.Wishlists
                .Include(w => w.Product)
                .Include(w => w.User)
                .ToListAsync();
        }
    }
}
