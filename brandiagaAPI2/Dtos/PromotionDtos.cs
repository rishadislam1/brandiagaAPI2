using System.ComponentModel.DataAnnotations;

namespace brandiagaAPI2.Dtos
{
    public class PromotionCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Discount type is required")]
        [StringLength(50, ErrorMessage = "Discount type cannot exceed 50 characters")]
        public string DiscountType { get; set; } // e.g., "Percentage", "Fixed"

        [Required(ErrorMessage = "Discount value is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Discount value must be zero or positive")]
        public decimal DiscountValue { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class PromotionUpdateDto
    {
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Discount type cannot exceed 50 characters")]
        public string DiscountType { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discount value must be zero or positive")]
        public decimal? DiscountValue { get; set; }

        public bool? IsActive { get; set; }
    }

    public class PromotionResponseDto
    {
        public Guid PromotionId { get; set; }
        public string Name { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public bool IsActive { get; set; }
    }
}
