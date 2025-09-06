using brandiagaAPI2.Data.Models;

namespace brandiagaAPI2.Interfaces.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByIdAsync(Guid userId);
        Task AddUserAsync(User user);
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<Role> GetRoleByIdAsync(Guid roleId);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid userId);
        Task<int> GetTotalOrdersByUserIdAsync(Guid userId);
        Task<decimal> GetTotalSpentByUserIdAsync(Guid userId);
        Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);
        Task<List<User>> GetAllUserAsync();
        Task<Subscription> UserSubscription(string email);
        Task AddVerificationTokenAsync(VerificationToken token);
        Task<VerificationToken> GetVerificationTokenAsync(string token);

    }
}
