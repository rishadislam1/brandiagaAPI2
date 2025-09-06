using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace brandiagaAPI2.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly DbAbdeaeDotnet1Context _context;

        public PromotionRepository(DbAbdeaeDotnet1Context context)
        {
            _context = context;
        }

        public async Task<Promotion> GetPromotionByIdAsync(Guid promotionId)
        {
            return await _context.Promotions
                .FirstOrDefaultAsync(p => p.PromotionId == promotionId);
        }

        public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
        {
            return await _context.Promotions
                .ToListAsync();
        }

        public async Task AddPromotionAsync(Promotion promotion)
        {
            await _context.Promotions.AddAsync(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePromotionAsync(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePromotionAsync(Guid promotionId)
        {
            var promotion = await _context.Promotions.FindAsync(promotionId);
            if (promotion != null)
            {
                _context.Promotions.Remove(promotion);
                await _context.SaveChangesAsync();
            }
        }
    }
}
