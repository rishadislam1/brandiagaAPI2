using brandiagaAPI2.Dtos;

namespace brandiagaAPI2.Interfaces.ServiceInterfaces
{
    public interface IPromotionService
    {
        Task<PromotionResponseDto> CreatePromotionAsync(PromotionCreateDto promotionCreateDto);
        Task<PromotionResponseDto> GetPromotionByIdAsync(Guid promotionId);
        Task<IEnumerable<PromotionResponseDto>> GetAllPromotionsAsync();
        Task<PromotionResponseDto> UpdatePromotionAsync(Guid promotionId, PromotionUpdateDto promotionUpdateDto);
        Task DeletePromotionAsync(Guid promotionId);
    }
}
