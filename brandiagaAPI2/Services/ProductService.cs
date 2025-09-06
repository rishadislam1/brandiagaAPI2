using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace brandiagaAPI2.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "upload");

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            // Ensure upload directory exists
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        private async Task<string> SaveImageAsync(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return null;

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(image.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                throw new Exception("Invalid file type. Only JPG and PNG are allowed.");

            // Validate file size (e.g., max 5MB)
            if (image.Length > 5 * 1024 * 1024)
                throw new Exception("File size exceeds 5MB.");

            var fileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/upload/{fileName}";
        }

        public async Task<ProductResponseDto> CreateProductAsync(ProductCreateDto productCreateDto)
        {
            var existingProduct = await _productRepository.GetProductBySkuAsync(productCreateDto.Sku);
            if (existingProduct != null)
            {
                throw new Exception("SKU Already Exists");
            }

            var product = new Product
            {
                ProductId = Guid.NewGuid(),
                Name = productCreateDto.Name,
                Sku = productCreateDto.Sku,
                Price = productCreateDto.Price,
                DiscountPrice = productCreateDto.DiscountPrice,
                CategoryId = productCreateDto.CategoryId,
                Description = productCreateDto.Description, // Map Description
                Specification = productCreateDto.Specification, // Map Specification
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _productRepository.AddProductAsync(product);

            // Handle image uploads
            if (productCreateDto.Images != null && productCreateDto.Images.Any())
            {
                for (int i = 0; i < productCreateDto.Images.Count; i++)
                {
                    var imageUrl = await SaveImageAsync(productCreateDto.Images[i]);
                    if (imageUrl != null)
                    {
                        var productImage = new ProductImage
                        {
                            ImageId = Guid.NewGuid(),
                            ProductId = product.ProductId,
                            ImageUrl = imageUrl,
                            IsPrimary = i == 0, // Set first image as primary
                            CreatedAt = DateTime.UtcNow
                        };
                        await _productRepository.AddProductImageAsync(productImage);
                    }
                }
            }

            var productImages = await _productRepository.GetProductImagesAsync(product.ProductId);
            return new ProductResponseDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Sku = product.Sku,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                Description = product.Description, // Map Description
                Specification = product.Specification, // Map Specification
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                ImageUrls = productImages.Select(pi => pi.ImageUrl).ToList()
            };
        }

        public async Task<ProductResponseDto> GetProductByIdAsync(Guid productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                throw new Exception("Product Not Found");
            }

            var productImages = await _productRepository.GetProductImagesAsync(productId);
            var averageRating = await _productRepository.GetAverageRatingAsync(productId);
            var reviewCount = await _productRepository.GetReviewCountAsync(productId);

            return new ProductResponseDto
            {
                ProductId = productId,
                Name = product.Name,
                Sku = product.Sku,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                Description = product.Description,
                Specification = product.Specification,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                ImageUrls = productImages.Select(pi => pi.ImageUrl).ToList(),
                Rating = averageRating,
                ReviewCount = reviewCount
            };
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            var result = new List<ProductResponseDto>();
            foreach (var product in products)
            {
                var averageRating = await _productRepository.GetAverageRatingAsync(product.ProductId);
                var reviewCount = await _productRepository.GetReviewCountAsync(product.ProductId);

                result.Add(new ProductResponseDto
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Sku = product.Sku,
                    Price = product.Price,
                    DiscountPrice = product.DiscountPrice,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category?.Name,
                    Description = product.Description,
                    Specification = product.Specification,
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt,
                    ImageUrls = product.ProductImages?.Select(pi => pi.ImageUrl).ToList() ?? new List<string>(),
                    Rating = averageRating,
                    ReviewCount = reviewCount
                });
            }
            return result;

        }

        public async Task<ProductResponseDto> UpdateProductAsync(Guid productId, ProductUpdateDto productUpdateDto)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                throw new Exception("Product Not Found");
            }

            if (!string.IsNullOrEmpty(productUpdateDto.Sku) && productUpdateDto.Sku != product.Sku)
            {
                var existingProduct = await _productRepository.GetProductBySkuAsync(productUpdateDto.Sku);
                if (existingProduct != null)
                {
                    throw new Exception("SKU Already Exists");
                }
                product.Sku = productUpdateDto.Sku;
            }

            if (!string.IsNullOrEmpty(productUpdateDto.Name))
                product.Name = productUpdateDto.Name;
            if (productUpdateDto.Price.HasValue)
                product.Price = productUpdateDto.Price.Value;
            if (productUpdateDto.DiscountPrice.HasValue)
                product.DiscountPrice = productUpdateDto.DiscountPrice;
            if (productUpdateDto.CategoryId.HasValue)
                product.CategoryId = productUpdateDto.CategoryId;
            if (!string.IsNullOrEmpty(productUpdateDto.Description))
                product.Description = productUpdateDto.Description; // Update Description
            if (productUpdateDto.Specification != null)
                product.Specification = productUpdateDto.Specification; // Update Specification
            product.UpdatedAt = DateTime.UtcNow;

            // Handle image uploads
            if (productUpdateDto.Images != null && productUpdateDto.Images.Any())
            {
                // Delete existing images from disk and database
                var existingImages = await _productRepository.GetProductImagesAsync(productId);
                foreach (var image in existingImages)
                {
                    var filePath = Path.Combine(_uploadPath, Path.GetFileName(image.ImageUrl));
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                await _productRepository.DeleteProductImagesAsync(productId);

                // Add new images
                for (int i = 0; i < productUpdateDto.Images.Count; i++)
                {
                    var imageUrl = await SaveImageAsync(productUpdateDto.Images[i]);
                    if (imageUrl != null)
                    {
                        var productImage = new ProductImage
                        {
                            ImageId = Guid.NewGuid(),
                            ProductId = product.ProductId,
                            ImageUrl = imageUrl,
                            IsPrimary = i == 0, // Set first image as primary
                            CreatedAt = DateTime.UtcNow
                        };
                        await _productRepository.AddProductImageAsync(productImage);
                    }
                }
            }

            await _productRepository.UpdateProductAsync(product);

            var productImages = await _productRepository.GetProductImagesAsync(productId);
            return new ProductResponseDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Sku = product.Sku,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                Description = product.Description, // Map Description
                Specification = product.Specification, // Map Specification
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                ImageUrls = productImages.Select(pi => pi.ImageUrl).ToList()
            };
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                throw new Exception("Product Not Found");
            }

            // Delete associated images from disk and database
            var productImages = await _productRepository.GetProductImagesAsync(productId);
            foreach (var image in productImages)
            {
                var filePath = Path.Combine(_uploadPath, Path.GetFileName(image.ImageUrl));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            await _productRepository.DeleteProductImagesAsync(productId);

            await _productRepository.DeleteProductAsync(productId);
        }
    }
}