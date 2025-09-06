using System.ComponentModel.DataAnnotations;

namespace brandiagaAPI2.Dtos
{
    public class InventoryCreateDto
    {
        [Required(ErrorMessage = "Product ID is required")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative number")]
        public int Quantity { get; set; }
    }

    public class InventoryUpdateDto
    {
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative number")]
        public int? Quantity { get; set; }
    }

    public class InventoryResponseDto
    {
        public Guid InventoryId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
