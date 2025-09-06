using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace brandiagaAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SEOController : ControllerBase
    {
        private readonly ISEOService _seoService;

        public SEOController(ISEOService seoService)
        {
            _seoService = seoService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{seoid:guid}")]
        public async Task<IActionResult> GetSEOById(Guid seoid)
        {
            try
            {
                var seo = await _seoService.GetSEOByIdAsync(seoid);
                return Ok(ResponseDTO<SEOResponseDto>.Success(seo, "SEO setting retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("page")]
        public async Task<IActionResult> GetSEOByPage([FromQuery] string pageType, [FromQuery] Guid pageId)
        {
            try
            {
                var seo = await _seoService.GetSEOByPageAsync(pageType, pageId);
                return Ok(ResponseDTO<SEOResponseDto>.Success(seo, "SEO setting retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllSEOSettings()
        {
            try
            {
                var seoSettings = await _seoService.GetAllSEOSettingsAsync();
                return Ok(ResponseDTO<IEnumerable<SEOResponseDto>>.Success(seoSettings, "SEO settings retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateSEO([FromBody] SEORequestDto seoDto)
        {
            try
            {
                if (seoDto == null)
                {
                    return BadRequest(ResponseDTO<object>.Error("SEO data is required."));
                }
                var seo = await _seoService.CreateSEOAsync(seoDto);
                return Ok(ResponseDTO<SEOResponseDto>.Success(seo, "SEO setting created successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{seoid:guid}")]
        public async Task<IActionResult> UpdateSEO(Guid seoid, [FromBody] SEORequestDto seoDto)
        {
            try
            {
                if (seoDto == null)
                {
                    return BadRequest(ResponseDTO<object>.Error("SEO data is required."));
                }
                await _seoService.UpdateSEOAsync(seoid, seoDto);
                return Ok(ResponseDTO<object>.Success(null, "SEO setting updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{seoid:guid}")]
        public async Task<IActionResult> DeleteSEO(Guid seoid)
        {
            try
            {
                await _seoService.DeleteSEOAsync(seoid);
                return Ok(ResponseDTO<object>.Success(null, "SEO setting deleted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }
    }
}