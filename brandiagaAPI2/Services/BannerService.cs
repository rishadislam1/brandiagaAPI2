using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace brandiagaAPI2.Services
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "upload", "bannerimage");

        public BannerService(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
            Directory.CreateDirectory(_uploadPath);
        }

        public async Task<BannerResponseDto> GetBannerByIdAsync(Guid bannerId)
        {
            var banner = await _bannerRepository.GetBannerByIdAsync(bannerId);
            if (banner == null) throw new Exception("Banner not found");

            return new BannerResponseDto
            {
                BannerId = banner.BannerId,
                Title = banner.Title,
                ImageUrl = banner.ImageUrl,
                LinkUrl = banner.LinkUrl,
                DisplayOrder = banner.DisplayOrder,
                IsActive = banner.IsActive,
                CreatedAt = banner.CreatedAt
            };
        }

        public async Task<IEnumerable<BannerResponseDto>> GetAllBannersAsync()
        {
            var banners = await _bannerRepository.GetAllBannersAsync();
            return banners.Select(b => new BannerResponseDto
            {
                BannerId = b.BannerId,
                Title = b.Title,
                ImageUrl = b.ImageUrl,
                LinkUrl = b.LinkUrl,
                DisplayOrder = b.DisplayOrder,
                IsActive = b.IsActive,
                CreatedAt = b.CreatedAt
            }).ToList();
        }

        public async Task<BannerResponseDto> CreateBannerAsync(BannerRequestDto bannerDto, IFormFile? imageFile)
        {
            if (bannerDto == null)
                throw new ArgumentNullException(nameof(bannerDto), "Banner data is required.");
            if (string.IsNullOrWhiteSpace(bannerDto.Title))
                throw new ArgumentException("Title is required.", nameof(bannerDto.Title));
            if (bannerDto.DisplayOrder < 0)
                throw new ArgumentException("DisplayOrder cannot be negative.", nameof(bannerDto.DisplayOrder));

            string? imageUrl = bannerDto.ImageUrl;

            if (imageFile != null && imageFile.Length > 0)
            {
                try
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                        throw new Exception("Invalid image format. Only JPG, JPEG, PNG, and GIF are allowed.");
                    if (imageFile.Length > 5 * 1024 * 1024)
                        throw new Exception("Image file size exceeds 5MB.");

                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(_uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    imageUrl = $"/upload/bannerimage/{fileName}";
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to upload image", ex);
                }
            }
            else if (string.IsNullOrWhiteSpace(imageUrl))
            {
                throw new ArgumentException("An image file or ImageUrl is required.", nameof(bannerDto.ImageUrl));
            }

            var banner = new Banner
            {
                BannerId = Guid.NewGuid(),
                Title = bannerDto.Title,
                ImageUrl = imageUrl,
                LinkUrl = bannerDto.LinkUrl,
                DisplayOrder = bannerDto.DisplayOrder,
                IsActive = bannerDto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            await _bannerRepository.AddBannerAsync(banner);

            return new BannerResponseDto
            {
                BannerId = banner.BannerId,
                Title = banner.Title,
                ImageUrl = banner.ImageUrl,
                LinkUrl = banner.LinkUrl,
                DisplayOrder = banner.DisplayOrder,
                IsActive = banner.IsActive,
                CreatedAt = banner.CreatedAt
            };
        }

        public async Task UpdateBannerAsync(Guid bannerId, BannerRequestDto bannerDto)
        {
            if (bannerDto == null)
                throw new ArgumentNullException(nameof(bannerDto), "Banner data is required.");
            if (string.IsNullOrWhiteSpace(bannerDto.Title))
                throw new ArgumentException("Title is required.", nameof(bannerDto.Title));
            if (bannerDto.DisplayOrder < 0)
                throw new ArgumentException("DisplayOrder cannot be negative.", nameof(bannerDto.DisplayOrder));

            var banner = await _bannerRepository.GetBannerByIdAsync(bannerId);
            if (banner == null) throw new Exception("Banner not found");

            string? imageUrl = bannerDto.ImageUrl ?? banner.ImageUrl;

            if (!string.IsNullOrWhiteSpace(bannerDto.ImageBase64))
            {
                try
                {
                    var base64Data = bannerDto.ImageBase64.Contains(",") ? bannerDto.ImageBase64.Split(',')[1] : bannerDto.ImageBase64;
                    var imageBytes = Convert.FromBase64String(base64Data);

                    var fileExtension = ".png";
                    if (bannerDto.ImageBase64.Contains("image/jpeg")) fileExtension = ".jpg";
                    else if (bannerDto.ImageBase64.Contains("image/gif")) fileExtension = ".gif";

                    if (imageBytes.Length > 5 * 1024 * 1024)
                        throw new Exception("Image file size exceeds 5MB.");

                    var fileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(_uploadPath, fileName);

                    await File.WriteAllBytesAsync(filePath, imageBytes);

                    imageUrl = $"/upload/bannerimage/{fileName}";

                    if (!string.IsNullOrEmpty(banner.ImageUrl) && banner.ImageUrl.StartsWith("/upload/bannerimage/"))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), banner.ImageUrl.TrimStart('/'));
                        if (File.Exists(oldImagePath))
                        {
                            File.Delete(oldImagePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to upload image", ex);
                }
            }
            else if (string.IsNullOrWhiteSpace(imageUrl))
            {
                throw new ArgumentException("An image file or ImageUrl is required.", nameof(bannerDto.ImageUrl));
            }

            banner.Title = bannerDto.Title;
            banner.ImageUrl = imageUrl;
            banner.LinkUrl = bannerDto.LinkUrl;
            banner.DisplayOrder = bannerDto.DisplayOrder;
            banner.IsActive = bannerDto.IsActive;

            await _bannerRepository.UpdateBannerAsync(banner);
        }

        public async Task DeleteBannerAsync(Guid bannerId)
        {
            var banner = await _bannerRepository.GetBannerByIdAsync(bannerId);
            if (banner == null) throw new Exception("Banner not found");

            if (!string.IsNullOrEmpty(banner.ImageUrl) && banner.ImageUrl.StartsWith("/upload/bannerimage/"))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), banner.ImageUrl.TrimStart('/'));
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            await _bannerRepository.DeleteBannerAsync(bannerId);
        }
    }
}