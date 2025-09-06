using brandiagaAPI2.Dtos;

namespace brandiagaAPI2.Interfaces.ServiceInterfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDto> CreateReviewAsync(ReviewCreateDto reviewCreateDto, Guid userId);
        Task<ReviewResponseDto> GetReviewByIdAsync(Guid reviewId);
        Task<IEnumerable<ReviewResponseDto>> GetReviewsByProductIdAsync(Guid productId);
        Task<IEnumerable<ReviewResponseDto>> GetReviewsByUserIdAsync(Guid userId);
        Task<ReviewResponseDto> UpdateReviewAsync(Guid reviewId, Guid userId, bool isAdmin, ReviewUpdateDto reviewUpdateDto);
        Task DeleteReviewAsync(Guid reviewId, Guid userId, bool isAdmin);
    }
}
