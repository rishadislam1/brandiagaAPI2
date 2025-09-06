using brandiagaAPI2.Data.Models;

namespace brandiagaAPI2.Interfaces.RepositoryInterfaces
{
    public interface IReviewRepository
    {
        Task<Review> GetReviewByIdAsync(Guid reviewId);
        Task<IEnumerable<Review>> GetReviewsByProductIdAsync(Guid productId);
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid userId);
        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(Guid reviewId);
    }
}
