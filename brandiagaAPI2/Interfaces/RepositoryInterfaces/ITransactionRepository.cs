using brandiagaAPI2.Data.Models;

namespace brandiagaAPI2.Interfaces.RepositoryInterfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> GetTransactionByIdAsync(Guid transactionId);
        Task<Transaction> GetTransactionByOrderIdAsync(Guid orderId);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task AddTransactionAsync(Transaction transaction);
        Task UpdateTransactionAsync(Transaction transaction);
    }
}
