using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace brandiagaAPI2.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DbAbdeaeDotnet1Context _context;

        public TransactionRepository(DbAbdeaeDotnet1Context context)
        {
            _context = context;
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid transactionId)
        {
            return await _context.Transactions
                .Include(t => t.Order)
                .Include(t => t.Gateway)
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
        }

        public async Task<Transaction> GetTransactionByOrderIdAsync(Guid orderId)
        {
            return await _context.Transactions
                .Include(t => t.Order)
                .Include(t => t.Gateway)
                .FirstOrDefaultAsync(t => t.OrderId == orderId);
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .Include(t => t.Order)
                .Include(t => t.Gateway)
                .ToListAsync();
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }
    }
}
