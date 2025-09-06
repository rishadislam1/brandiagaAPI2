using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace brandiagaAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannersController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannersController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{bannerId:guid}")]
        public async Task<IActionResult> GetBannerById(Guid bannerId)
        {
            try
            {
                var banner = await _bannerService.GetBannerByIdAsync(bannerId);
                return Ok(ResponseDTO<BannerResponseDto>.Success(banner, "Banner retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

       
        [HttpGet]
        public async Task<IActionResult> GetAllBanners()
        {
            try
            {
                var banners = await _bannerService.GetAllBannersAsync();
                return Ok(ResponseDTO<IEnumerable<BannerResponseDto>>.Success(banners, "Banners retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateBanner([FromForm] BannerRequestDto bannerDto, IFormFile? imageFile)
        {
            try
            {
                if (bannerDto == null)
                {
                    return BadRequest(ResponseDTO<object>.Error("Banner data is required."));
                }
                if (string.IsNullOrWhiteSpace(bannerDto.Title))
                {
                    return BadRequest(ResponseDTO<object>.Error("Title is required."));
                }
                if (bannerDto.DisplayOrder < 0)
                {
                    return BadRequest(ResponseDTO<object>.Error("DisplayOrder cannot be negative."));
                }
                if (imageFile == null && string.IsNullOrWhiteSpace(bannerDto.ImageUrl))
                {
                    return BadRequest(ResponseDTO<object>.Error("An image file or ImageUrl is required."));
                }

                if (imageFile != null && imageFile.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                    {
                        return BadRequest(ResponseDTO<object>.Error("Invalid image format. Only JPG, JPEG, PNG, and GIF are allowed."));
                    }
                    if (imageFile.Length > 5 * 1024 * 1024)
                    {
                        return BadRequest(ResponseDTO<object>.Error("Image file size exceeds 5MB."));
                    }
                }

                var banner = await _bannerService.CreateBannerAsync(bannerDto, imageFile);
                return Ok(ResponseDTO<BannerResponseDto>.Success(banner, "Banner created successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{bannerId:guid}")]
        public async Task<IActionResult> UpdateBanner(Guid bannerId, [FromForm] BannerRequestDto bannerDto)
        {
            try
            {
                if (bannerDto == null)
                {
                    return BadRequest(ResponseDTO<object>.Error("Banner data is required."));
                }
                if (string.IsNullOrWhiteSpace(bannerDto.Title))
                {
                    return BadRequest(ResponseDTO<object>.Error("Title is required."));
                }
                if (bannerDto.DisplayOrder < 0)
                {
                    return BadRequest(ResponseDTO<object>.Error("DisplayOrder cannot be negative."));
                }
                if (string.IsNullOrWhiteSpace(bannerDto.ImageBase64) && string.IsNullOrWhiteSpace(bannerDto.ImageUrl))
                {
                    var existingBanner = await _bannerService.GetBannerByIdAsync(bannerId);
                    if (existingBanner == null)
                    {
                        return BadRequest(ResponseDTO<object>.Error("Banner not found."));
                    }
                    if (string.IsNullOrWhiteSpace(existingBanner.ImageUrl))
                    {
                        return BadRequest(ResponseDTO<object>.Error("An image file (ImageBase64) or ImageUrl is required."));
                    }
                }

                if (!string.IsNullOrWhiteSpace(bannerDto.ImageBase64))
                {
                    try
                    {
                        var base64Data = bannerDto.ImageBase64.Contains(",") ? bannerDto.ImageBase64.Split(',')[1] : bannerDto.ImageBase64;
                        var imageBytes = Convert.FromBase64String(base64Data);
                        if (imageBytes.Length > 5 * 1024 * 1024)
                        {
                            return BadRequest(ResponseDTO<object>.Error("Image file size exceeds 5MB."));
                        }
                    }
                    catch (FormatException)
                    {
                        return BadRequest(ResponseDTO<object>.Error("Invalid ImageBase64 format."));
                    }
                }

                await _bannerService.UpdateBannerAsync(bannerId, bannerDto);
                return Ok(ResponseDTO<object>.Success(null, "Banner updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{bannerId:guid}")]
        public async Task<IActionResult> DeleteBanner(Guid bannerId)
        {
            try
            {
                await _bannerService.DeleteBannerAsync(bannerId);
                return Ok(ResponseDTO<object>.Success(null, "Banner deleted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }
    }
}