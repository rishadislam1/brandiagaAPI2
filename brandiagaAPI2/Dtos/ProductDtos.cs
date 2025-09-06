using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace brandiagaAPI2.Dtos
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "SKU is required")]
        [StringLength(50, ErrorMessage = "SKU cannot exceed 50 characters")]
        public string Sku { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000000, ErrorMessage = "Price must be between 0.01 and 1,000,000")]
        public decimal Price { get; set; }

        [Range(0, 1000000, ErrorMessage = "Discount price must be between 0 and 1,000,000")]
        public decimal? DiscountPrice { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        public Guid CategoryId { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } // New Description property

        public string Specification { get; set; } // New Specification property as Dictionary

        // Add support for multiple image uploads
        public List<IFormFile> Images { get; set; }
    }

    public class ProductUpdateDto
    {
        [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "SKU cannot exceed 50 characters")]
        public string Sku { get; set; }

        [Range(0.01, 1000000, ErrorMessage = "Price must be between 0.01 and 1,000,000")]
        public decimal? Price { get; set; }

        [Range(0, 1000000, ErrorMessage = "Discount price must be between 0 and 1,000,000")]
        public decimal? DiscountPrice { get; set; }

        public Guid? CategoryId { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } // New Description property

        public string Specification { get; set; } // New Specification property as Dictionary

        // Add support for multiple image uploads
        public List<IFormFile> Images { get; set; }
    }

    public class ProductResponseDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> ImageUrls { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public decimal Rating { get; set; } // Average rating from reviews
        public int ReviewCount { get; set; } // Total number of reviews
    }
}