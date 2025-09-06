using brandiagaAPI2.Data.Models;

namespace brandiagaAPI2.Interfaces.RepositoryInterfaces
{
    public interface IBannerRepository
    {
        Task<Banner> GetBannerByIdAsync(Guid bannerId);
        Task<IEnumerable<Banner>> GetAllBannersAsync();
        Task AddBannerAsync(Banner banner);
        Task UpdateBannerAsync(Banner banner);
        Task DeleteBannerAsync(Guid bannerId);
    }
}
