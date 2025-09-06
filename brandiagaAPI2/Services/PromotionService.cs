using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using brandiagaAPI2.Interfaces.ServiceInterfaces;

namespace brandiagaAPI2.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;

        public PromotionService(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }

        public async Task<PromotionResponseDto> CreatePromotionAsync(PromotionCreateDto promotionCreateDto)
        {
            // Validate discount type
            var validDiscountTypes = new[] { "Percentage", "Fixed" };
            if (!validDiscountTypes.Contains(promotionCreateDto.DiscountType))
            {
                throw new Exception("Invalid discount type. Allowed values: Percentage, Fixed");
            }

            // Check if a promotion with the same name already exists
            var existingPromotions = await _promotionRepository.GetAllPromotionsAsync();
            if (existingPromotions.Any(p => p.Name.Equals(promotionCreateDto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Promotion with this name already exists");
            }

            // Validate discount value based on type
            if (promotionCreateDto.DiscountType == "Percentage" && promotionCreateDto.DiscountValue > 100)
            {
                throw new Exception("Discount value cannot exceed 100% for percentage discounts");
            }

            var promotion = new Promotion
            {
                PromotionId = Guid.NewGuid(),
                Name = promotionCreateDto.Name,
                DiscountType = promotionCreateDto.DiscountType,
                DiscountValue = promotionCreateDto.DiscountValue,
                IsActive = promotionCreateDto.IsActive
            };

            await _promotionRepository.AddPromotionAsync(promotion);

            return new PromotionResponseDto
            {
                PromotionId = promotion.PromotionId,
                Name = promotion.Name,
                DiscountType = promotion.DiscountType,
                DiscountValue = promotion.DiscountValue,
                IsActive = promotion.IsActive
            };
        }

        public async Task<PromotionResponseDto> GetPromotionByIdAsync(Guid promotionId)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            if (promotion == null)
            {
                throw new Exception("Promotion not found");
            }

            return new PromotionResponseDto
            {
                PromotionId = promotion.PromotionId,
                Name = promotion.Name,
                DiscountType = promotion.DiscountType,
                DiscountValue = promotion.DiscountValue,
                IsActive = promotion.IsActive
            };
        }

        public async Task<IEnumerable<PromotionResponseDto>> GetAllPromotionsAsync()
        {
            var promotions = await _promotionRepository.GetAllPromotionsAsync();
            return promotions.Select(p => new PromotionResponseDto
            {
                PromotionId = p.PromotionId,
                Name = p.Name,
                DiscountType = p.DiscountType,
                DiscountValue = p.DiscountValue,
                IsActive = p.IsActive
            }).ToList();
        }

        public async Task<PromotionResponseDto> UpdatePromotionAsync(Guid promotionId, PromotionUpdateDto promotionUpdateDto)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            if (promotion == null)
            {
                throw new Exception("Promotion not found");
            }

            if (!string.IsNullOrEmpty(promotionUpdateDto.Name))
            {
                var existingPromotions = await _promotionRepository.GetAllPromotionsAsync();
                if (existingPromotions.Any(p => p.Name.Equals(promotionUpdateDto.Name, StringComparison.OrdinalIgnoreCase) && p.PromotionId != promotionId))
                {
                    throw new Exception("Promotion with this name already exists");
                }
                promotion.Name = promotionUpdateDto.Name;
            }

            if (!string.IsNullOrEmpty(promotionUpdateDto.DiscountType))
            {
                var validDiscountTypes = new[] { "Percentage", "Fixed" };
                if (!validDiscountTypes.Contains(promotionUpdateDto.DiscountType))
                {
                    throw new Exception("Invalid discount type. Allowed values: Percentage, Fixed");
                }
                promotion.DiscountType = promotionUpdateDto.DiscountType;
            }

            if (promotionUpdateDto.DiscountValue.HasValue)
            {
                promotion.DiscountValue = promotionUpdateDto.DiscountValue.Value;

                // Validate discount value based on type
                if ((promotion.DiscountType == "Percentage" || (promotionUpdateDto.DiscountType == "Percentage" && !string.IsNullOrEmpty(promotionUpdateDto.DiscountType)))
                    && promotion.DiscountValue > 100)
                {
                    throw new Exception("Discount value cannot exceed 100% for percentage discounts");
                }
            }

            if (promotionUpdateDto.IsActive.HasValue)
            {
                promotion.IsActive = promotionUpdateDto.IsActive.Value;
            }

            await _promotionRepository.UpdatePromotionAsync(promotion);

            return new PromotionResponseDto
            {
                PromotionId = promotion.PromotionId,
                Name = promotion.Name,
                DiscountType = promotion.DiscountType,
                DiscountValue = promotion.DiscountValue,
                IsActive = promotion.IsActive
            };
        }

        public async Task DeletePromotionAsync(Guid promotionId)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            if (promotion == null)
            {
                throw new Exception("Promotion not found");
            }

            // Note: Not checking PromotionProducts dependency since we're not using other models
            await _promotionRepository.DeletePromotionAsync(promotionId);
        }
    }
}
