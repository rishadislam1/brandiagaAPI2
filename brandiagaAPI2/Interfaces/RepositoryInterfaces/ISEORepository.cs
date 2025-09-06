using brandiagaAPI2.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace brandiagaAPI2.Interfaces.RepositoryInterfaces
{
    public interface ISEORepository
    {
        Task<Seosetting> GetSEOByIdAsync(Guid seoid);
        Task<Seosetting> GetSEOByPageAsync(string pageType, Guid pageId);
        Task<IEnumerable<Seosetting>> GetAllSEOSettingsAsync();
        Task AddSEOAsync(Seosetting seo);
        Task UpdateSEOAsync(Seosetting seo);
        Task DeleteSEOAsync(Guid seoid);
    }
}