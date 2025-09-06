using brandiagaAPI2.Data.Models;

namespace brandiagaAPI2.Interfaces.RepositoryInterfaces
{
    public interface IPromotionRepository
    {
        Task<Promotion> GetPromotionByIdAsync(Guid promotionId);
        Task<IEnumerable<Promotion>> GetAllPromotionsAsync();
        Task AddPromotionAsync(Promotion promotion);
        Task UpdatePromotionAsync(Promotion promotion);
        Task DeletePromotionAsync(Guid promotionId);
    }
}
