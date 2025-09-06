using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brandiagaAPI2.Services
{
    public class SEOService : ISEOService
    {
        private readonly ISEORepository _seoRepository;

        public SEOService(ISEORepository seoRepository)
        {
            _seoRepository = seoRepository;
        }

        public async Task<SEOResponseDto> GetSEOByIdAsync(Guid seoid)
        {
            var seo = await _seoRepository.GetSEOByIdAsync(seoid);
            if (seo == null) throw new Exception("SEO setting not found");

            return new SEOResponseDto
            {
                Seoid = seo.Seoid,
                PageType = seo.PageType,
                PageId = seo.PageId,
                MetaTitle = seo.MetaTitle,
                MetaDescription = seo.MetaDescription
            };
        }

        public async Task<SEOResponseDto> GetSEOByPageAsync(string pageType, Guid pageId)
        {
            var seo = await _seoRepository.GetSEOByPageAsync(pageType, pageId);
            if (seo == null) throw new Exception("SEO setting not found for the specified page");

            return new SEOResponseDto
            {
                Seoid = seo.Seoid,
                PageType = seo.PageType,
                PageId = seo.PageId,
                MetaTitle = seo.MetaTitle,
                MetaDescription = seo.MetaDescription
            };
        }

        public async Task<IEnumerable<SEOResponseDto>> GetAllSEOSettingsAsync()
        {
            var seoSettings = await _seoRepository.GetAllSEOSettingsAsync();
            return seoSettings.Select(s => new SEOResponseDto
            {
                Seoid = s.Seoid,
                PageType = s.PageType,
                PageId = s.PageId,
                MetaTitle = s.MetaTitle,
                MetaDescription = s.MetaDescription
            }).ToList();
        }

        public async Task<SEOResponseDto> CreateSEOAsync(SEORequestDto seoDto)
        {
            var seo = new Seosetting
            {
                Seoid = Guid.NewGuid(),
                PageType = seoDto.PageType,
                PageId = seoDto.PageId,
                MetaTitle = seoDto.MetaTitle,
                MetaDescription = seoDto.MetaDescription
            };

            await _seoRepository.AddSEOAsync(seo);

            return new SEOResponseDto
            {
                Seoid = seo.Seoid,
                PageType = seo.PageType,
                PageId = seo.PageId,
                MetaTitle = seo.MetaTitle,
                MetaDescription = seo.MetaDescription
            };
        }

        public async Task UpdateSEOAsync(Guid seoid, SEORequestDto seoDto)
        {
            var seo = await _seoRepository.GetSEOByIdAsync(seoid);
            if (seo == null) throw new Exception("SEO setting not found");

            seo.PageType = seoDto.PageType;
            seo.PageId = seoDto.PageId;
            seo.MetaTitle = seoDto.MetaTitle;
            seo.MetaDescription = seoDto.MetaDescription;

            await _seoRepository.UpdateSEOAsync(seo);
        }

        public async Task DeleteSEOAsync(Guid seoid)
        {
            await _seoRepository.DeleteSEOAsync(seoid);
        }
    }
}