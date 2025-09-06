using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace brandiagaAPI2.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbAbdeaeDotnet1Context _context;

        public UserRepository(DbAbdeaeDotnet1Context context)
        {
            _context = context;
        }
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task<Role> GetRoleByIdAsync(Guid roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
            
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTotalOrdersByUserIdAsync(Guid userId)
        {
            return await _context.Orders
                .CountAsync(o => o.UserId == userId);
        }

        public async Task<decimal> GetTotalSpentByUserIdAsync(Guid userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId && o.Status == "Completed")
                .SumAsync(o => o.TotalAmount);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllUserAsync()
        {
            return await _context.Users
           .Include(u => u.Role) 
           .ToListAsync();
        }

        public async Task AddVerificationTokenAsync(VerificationToken token)
        {
            await _context.VerificationTokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }
        public async Task<VerificationToken> GetVerificationTokenAsync(string token)
        {
            return await _context.VerificationTokens
                .Include(vt => vt.User)
                .FirstOrDefaultAsync(vt => vt.Token == token);
        }

        public async Task<Subscription> UserSubscription(string email)
        {
            var subscription = new Subscription
            {
                Id = Guid.NewGuid(),
                Email = email
            };

            await _context.Subscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();

            return subscription;
        }





    }
}
