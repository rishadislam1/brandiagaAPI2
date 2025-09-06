using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using brandiagaAPI2.Interfaces.ServiceInterfaces;

namespace brandiagaAPI2.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly DbAbdeaeDotnet1Context _context;

        public TransactionService(ITransactionRepository transactionRepository, IOrderRepository orderRepository, DbAbdeaeDotnet1Context context)
        {
            _transactionRepository = transactionRepository;
            _orderRepository = orderRepository;
            _context = context;
        }

        public async Task<TransactionResponseDto> ProcessPaymentAsync(PaymentRequestDto paymentRequestDto)
        {
            // Validate order
            var order = await _orderRepository.GetOrderByIdAsync(paymentRequestDto.OrderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            if (order.TotalAmount != paymentRequestDto.Amount)
            {
                throw new Exception("Payment amount does not match order total");
            }
            if (order.Status != "Pending")
            {
                throw new Exception("Order is not in a pending state");
            }

            // Validate gateway
            var gateway = await _context.PaymentGateways.FindAsync(paymentRequestDto.GatewayId);
            if (gateway == null)
            {
                throw new Exception("Payment gateway not found");
            }

            // Placeholder payment gateway logic (simulated success)
            bool paymentSuccess = new Random().NextDouble() >= 0.2; // 80% success rate
            string transactionStatus = paymentSuccess ? "Completed" : "Failed";

            var transaction = new Transaction
            {
                TransactionId = Guid.NewGuid(),
                OrderId = paymentRequestDto.OrderId,
                GatewayId = paymentRequestDto.GatewayId,
                Amount = paymentRequestDto.Amount,
                Currency = paymentRequestDto.Currency,
                TransactionStatus = transactionStatus,
                TransactionDate = DateTime.UtcNow
            };

            await _transactionRepository.AddTransactionAsync(transaction);

            // Update order status if payment is successful
            if (transactionStatus == "Completed")
            {
                order.Status = "Processing";
                order.UpdatedAt = DateTime.UtcNow;
                await _orderRepository.UpdateOrderAsync(order);
            }

            return new TransactionResponseDto
            {
                TransactionId = transaction.TransactionId,
                OrderId = transaction.OrderId,
                GatewayId = transaction.GatewayId,
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                TransactionStatus = transaction.TransactionStatus,
                TransactionDate = transaction.TransactionDate,
                GatewayName = gateway.Name
            };
        }

        public async Task<TransactionResponseDto> GetTransactionByIdAsync(Guid transactionId)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(transactionId);
            if (transaction == null)
            {
                throw new Exception("Transaction not found");
            }

            return new TransactionResponseDto
            {
                TransactionId = transaction.TransactionId,
                OrderId = transaction.OrderId,
                GatewayId = transaction.GatewayId,
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                TransactionStatus = transaction.TransactionStatus,
                TransactionDate = transaction.TransactionDate,
                GatewayName = transaction.Gateway?.Name
            };
        }

        public async Task<TransactionResponseDto> GetTransactionByOrderIdAsync(Guid orderId)
        {
            var transaction = await _transactionRepository.GetTransactionByOrderIdAsync(orderId);
            if (transaction == null)
            {
                throw new Exception("Transaction not found for this order");
            }

            return new TransactionResponseDto
            {
                TransactionId = transaction.TransactionId,
                OrderId = transaction.OrderId,
                GatewayId = transaction.GatewayId,
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                TransactionStatus = transaction.TransactionStatus,
                TransactionDate = transaction.TransactionDate,
                GatewayName = transaction.Gateway?.Name
            };
        }

        public async Task<IEnumerable<TransactionResponseDto>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetAllTransactionsAsync();

            return transactions.Select(transaction => new TransactionResponseDto
            {
                TransactionId = transaction.TransactionId,
                OrderId = transaction.OrderId,
                GatewayId = transaction.GatewayId,
                Amount = transaction.Amount,
                Currency = transaction.Currency,
                TransactionStatus = transaction.TransactionStatus,
                TransactionDate = transaction.TransactionDate,
                GatewayName = transaction.Gateway?.Name
            }).ToList();
        }
    }
}
