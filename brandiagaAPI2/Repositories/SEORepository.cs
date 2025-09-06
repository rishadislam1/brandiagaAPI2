using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace brandiagaAPI2.Repositories
{
    public class SEORepository : ISEORepository
    {
        private readonly DbAbdeaeDotnet1Context _context;

        public SEORepository(DbAbdeaeDotnet1Context context)
        {
            _context = context;
        }

        public async Task<Seosetting> GetSEOByIdAsync(Guid seoid)
        {
            return await _context.Seosettings
                .FirstOrDefaultAsync(s => s.Seoid == seoid);
        }

        public async Task<Seosetting> GetSEOByPageAsync(string pageType, Guid pageId)
        {
            return await _context.Seosettings
                .FirstOrDefaultAsync(s => s.PageType == pageType && s.PageId == pageId);
        }

        public async Task<IEnumerable<Seosetting>> GetAllSEOSettingsAsync()
        {
            return await _context.Seosettings
                .ToListAsync();
        }

        public async Task AddSEOAsync(Seosetting seo)
        {
            await _context.Seosettings.AddAsync(seo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSEOAsync(Seosetting seo)
        {
            _context.Seosettings.Update(seo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSEOAsync(Guid seoid)
        {
            var seo = await GetSEOByIdAsync(seoid);
            if (seo != null)
            {
                _context.Seosettings.Remove(seo);
                await _context.SaveChangesAsync();
            }
        }
    }
}