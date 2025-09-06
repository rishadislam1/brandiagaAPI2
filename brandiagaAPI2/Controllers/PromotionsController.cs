using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace brandiagaAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionsController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreatePromotion([FromBody] PromotionCreateDto promotionCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid input data."));
            }

            try
            {
                var promotion = await _promotionService.CreatePromotionAsync(promotionCreateDto);
                return Ok(ResponseDTO<PromotionResponseDto>.Success(promotion, "Promotion created successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpGet("{promotionId:guid}")]
        public async Task<IActionResult> GetPromotionById(Guid promotionId)
        {
            try
            {
                var promotion = await _promotionService.GetPromotionByIdAsync(promotionId);
                return Ok(ResponseDTO<PromotionResponseDto>.Success(promotion, "Promotion retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllPromotions()
        {
            try
            {
                var promotions = await _promotionService.GetAllPromotionsAsync();
                return Ok(ResponseDTO<IEnumerable<PromotionResponseDto>>.Success(promotions, "Promotions retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{promotionId:guid}")]
        public async Task<IActionResult> UpdatePromotion(Guid promotionId, [FromBody] PromotionUpdateDto promotionUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid input data."));
            }

            try
            {
                var promotion = await _promotionService.UpdatePromotionAsync(promotionId, promotionUpdateDto);
                return Ok(ResponseDTO<PromotionResponseDto>.Success(promotion, "Promotion updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{promotionId:guid}")]
        public async Task<IActionResult> DeletePromotion(Guid promotionId)
        {
            try
            {
                await _promotionService.DeletePromotionAsync(promotionId);
                return Ok(ResponseDTO<object>.Success(null, "Promotion deleted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }
    }
}
