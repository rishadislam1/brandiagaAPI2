using brandiagaAPI2.Dtos;

namespace brandiagaAPI2.Interfaces.ServiceInterfaces
{
    public interface IBannerService
    {
        Task<BannerResponseDto> GetBannerByIdAsync(Guid bannerId);
        Task<IEnumerable<BannerResponseDto>> GetAllBannersAsync();
        Task<BannerResponseDto> CreateBannerAsync(BannerRequestDto bannerDto, IFormFile? imageFile);
        Task UpdateBannerAsync(Guid bannerId, BannerRequestDto bannerDto);
        Task DeleteBannerAsync(Guid bannerId);
    }
}