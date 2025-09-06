using brandiagaAPI2.Dtos;

namespace brandiagaAPI2.Interfaces.ServiceInterfaces
{
    public interface ITransactionService
    {
        Task<TransactionResponseDto> ProcessPaymentAsync(PaymentRequestDto paymentRequestDto);
        Task<TransactionResponseDto> GetTransactionByIdAsync(Guid transactionId);
        Task<TransactionResponseDto> GetTransactionByOrderIdAsync(Guid orderId);
        Task<IEnumerable<TransactionResponseDto>> GetAllTransactionsAsync();
    }
}
