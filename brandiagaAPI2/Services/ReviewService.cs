using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using brandiagaAPI2.Interfaces.ServiceInterfaces;

namespace brandiagaAPI2.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public ReviewService(IReviewRepository reviewRepository, IUserRepository userRepository, IProductRepository productRepository)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<ReviewResponseDto> CreateReviewAsync(ReviewCreateDto reviewCreateDto, Guid userId)
        {
            // Validate user
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Validate product
            var product = await _productRepository.GetProductByIdAsync(reviewCreateDto.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            // Check if user already reviewed this product
            var existingReview = await _reviewRepository.GetReviewsByUserIdAsync(userId);
            if (existingReview.Any(r => r.ProductId == reviewCreateDto.ProductId))
            {
                throw new Exception("You have already reviewed this product");
            }

            var review = new Review
            {
                ReviewId = Guid.NewGuid(),
                UserId = userId,
                ProductId = reviewCreateDto.ProductId,
                Rating = reviewCreateDto.Rating,
                Comment = reviewCreateDto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.AddReviewAsync(review);

            return new ReviewResponseDto
            {
                ReviewId = review.ReviewId,
                UserId = review.UserId,
                Username = user.FirstName,
                ProductId = review.ProductId,
                ProductName = product.Name,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }

        public async Task<ReviewResponseDto> GetReviewByIdAsync(Guid reviewId)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null)
            {
                throw new Exception("Review not found");
            }

            return new ReviewResponseDto
            {
                ReviewId = review.ReviewId,
                UserId = review.UserId,
                Username = review.User.FirstName,
                ProductId = review.ProductId,
                ProductName = review.Product.Name,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByProductIdAsync(Guid productId)
        {
            var reviews = await _reviewRepository.GetReviewsByProductIdAsync(productId);
            return reviews.Select(r => new ReviewResponseDto
            {
                ReviewId = r.ReviewId,
                UserId = r.UserId,
                Username = r.User.FirstName,
                ProductId = r.ProductId,
                ProductName = r.Product.Name,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByUserIdAsync(Guid userId)
        {
            var reviews = await _reviewRepository.GetReviewsByUserIdAsync(userId);
            return reviews.Select(r => new ReviewResponseDto
            {
                ReviewId = r.ReviewId,
                UserId = r.UserId,
                Username = r.User.FirstName,
                ProductId = r.ProductId,
                ProductName = r.Product.Name,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<ReviewResponseDto> UpdateReviewAsync(Guid reviewId, Guid userId, bool isAdmin, ReviewUpdateDto reviewUpdateDto)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null)
            {
                throw new Exception("Review not found");
            }

            // Check if user is authorized to update (own review or admin)
            if (review.UserId != userId && !isAdmin)
            {
                throw new Exception("Unauthorized to update this review");
            }

            if (reviewUpdateDto.Rating.HasValue)
            {
                review.Rating = reviewUpdateDto.Rating.Value;
            }

            if (reviewUpdateDto.Comment != null)
            {
                review.Comment = reviewUpdateDto.Comment;
            }

            await _reviewRepository.UpdateReviewAsync(review);

            return new ReviewResponseDto
            {
                ReviewId = review.ReviewId,
                UserId = review.UserId,
                Username = review.User.FirstName,
                ProductId = review.ProductId,
                ProductName = review.Product.Name,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }

        public async Task DeleteReviewAsync(Guid reviewId, Guid userId, bool isAdmin)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null)
            {
                throw new Exception("Review not found");
            }

            // Check if user is authorized to delete (own review or admin)
            if (review.UserId != userId && !isAdmin)
            {
                throw new Exception("Unauthorized to delete this review");
            }

            await _reviewRepository.DeleteReviewAsync(reviewId);
        }
    }
}
