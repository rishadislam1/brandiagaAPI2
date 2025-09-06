using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace brandiagaAPI2.Repositories
{
    public class BannerRepository : IBannerRepository
    {
        private readonly DbAbdeaeDotnet1Context _context;

        public BannerRepository(DbAbdeaeDotnet1Context context)
        {
            _context = context;
        }

        public async Task<Banner> GetBannerByIdAsync(Guid bannerId)
        {
            return await _context.Banners
                .FirstOrDefaultAsync(b => b.BannerId == bannerId);
        }

        public async Task<IEnumerable<Banner>> GetAllBannersAsync()
        {
            return await _context.Banners
                .OrderBy(b => b.DisplayOrder)
                .ToListAsync();
        }

        public async Task AddBannerAsync(Banner banner)
        {
            banner.CreatedAt = DateTime.UtcNow;
            await _context.Banners.AddAsync(banner);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBannerAsync(Banner banner)
        {
            _context.Banners.Update(banner);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBannerAsync(Guid bannerId)
        {
            var banner = await GetBannerByIdAsync(bannerId);
            if (banner != null)
            {
                _context.Banners.Remove(banner);
                await _context.SaveChangesAsync();
            }
        }
    }
}
