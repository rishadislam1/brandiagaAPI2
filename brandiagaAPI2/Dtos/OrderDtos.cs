using System.ComponentModel.DataAnnotations;

namespace brandiagaAPI2.Dtos
{
    public class OrderItemDto
    {
        [Required(ErrorMessage = "Product ID is required")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }
    }

    public class OrderCreateDto
    {
        [Required(ErrorMessage = "User ID is required")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Order items are required")]
        [MinLength(1, ErrorMessage = "At least one order item is required")]
        public List<OrderItemDto> OrderItems { get; set; }
    }

    public class OrderUpdateDto
    {
        public string Status { get; set; } // e.g., "Pending", "Processing", "Shipped", "Delivered", "Cancelled"
    }

    public class OrderItemResponseDto
    {
        public Guid OrderItemId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderItemResponseDto> OrderItems { get; set; }
    }

    public class WishlistDto
    {
        public Guid WishlistId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ProductName { get; set; }
        public string? UserName { get; set; }
    }

    public class AddToWishlistDTO
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
    }
}
