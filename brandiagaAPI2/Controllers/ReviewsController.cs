using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace brandiagaAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewCreateDto reviewCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid input data."));
            }

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(ResponseDTO<object>.Error("Invalid user token."));
                }

                var review = await _reviewService.CreateReviewAsync(reviewCreateDto, userId);
                return Ok(ResponseDTO<ReviewResponseDto>.Success(review, "Review created successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpGet("{reviewId:guid}")]
        public async Task<IActionResult> GetReviewById(Guid reviewId)
        {
            try
            {
                var review = await _reviewService.GetReviewByIdAsync(reviewId);
                return Ok(ResponseDTO<ReviewResponseDto>.Success(review, "Review retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

      
        [HttpGet("product/{productId:guid}")]
        public async Task<IActionResult> GetReviewsByProductId(Guid productId)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsByProductIdAsync(productId);
                return Ok(ResponseDTO<IEnumerable<ReviewResponseDto>>.Success(reviews, "Reviews retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpGet("user")]
        public async Task<IActionResult> GetReviewsByUserId()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(ResponseDTO<object>.Error("Invalid user token."));
                }

                var reviews = await _reviewService.GetReviewsByUserIdAsync(userId);
                return Ok(ResponseDTO<IEnumerable<ReviewResponseDto>>.Success(reviews, "Reviews retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpPut("{reviewId:guid}")]
        public async Task<IActionResult> UpdateReview(Guid reviewId, [FromBody] ReviewUpdateDto reviewUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid input data."));
            }

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(ResponseDTO<object>.Error("Invalid user token."));
                }

                var isAdmin = User.IsInRole("Admin");
                var review = await _reviewService.UpdateReviewAsync(reviewId, userId, isAdmin, reviewUpdateDto);
                return Ok(ResponseDTO<ReviewResponseDto>.Success(review, "Review updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpDelete("{reviewId:guid}")]
        public async Task<IActionResult> DeleteReview(Guid reviewId)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(ResponseDTO<object>.Error("Invalid user token."));
                }

                var isAdmin = User.IsInRole("Admin");
                await _reviewService.DeleteReviewAsync(reviewId, userId, isAdmin);
                return Ok(ResponseDTO<object>.Success(null, "Review deleted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }
    }
}