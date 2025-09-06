using System.ComponentModel.DataAnnotations;

namespace brandiagaAPI2.Dtos
{
    public class PaymentRequestDto
    {
        [Required(ErrorMessage = "Order ID is required")]
        public Guid OrderId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Gateway ID is required")]
        public Guid GatewayId { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be a 3-letter code (e.g., USD)")]
        public string Currency { get; set; }
    }

    public class TransactionResponseDto
    {
        public Guid TransactionId { get; set; }
        public Guid OrderId { get; set; }
        public Guid GatewayId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string TransactionStatus { get; set; }
        public DateTime TransactionDate { get; set; }
        public string GatewayName { get; set; }
    }
}
