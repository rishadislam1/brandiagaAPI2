using System.ComponentModel.DataAnnotations;

namespace brandiagaAPI2.Dtos
{
    public class ShippingMethodCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Cost is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Cost must be zero or positive")]
        public decimal Cost { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class ShippingMethodUpdateDto
    {
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Cost must be zero or positive")]
        public decimal? Cost { get; set; }

        public bool? IsActive { get; set; }
    }

    public class ShippingMethodResponseDto
    {
        public Guid ShippingMethodId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public bool IsActive { get; set; }
    }
}
