using brandiagaAPI2.Dtos;

namespace brandiagaAPI2.Interfaces.ServiceInterfaces
{
    public interface ISEOService
    {
        Task<SEOResponseDto> GetSEOByIdAsync(Guid seoid);
        Task<SEOResponseDto> GetSEOByPageAsync(string pageType, Guid pageId);
        Task<IEnumerable<SEOResponseDto>> GetAllSEOSettingsAsync();
        Task<SEOResponseDto> CreateSEOAsync(SEORequestDto seoDto);
        Task UpdateSEOAsync(Guid seoid, SEORequestDto seoDto);
        Task DeleteSEOAsync(Guid seoid);
    }
}
