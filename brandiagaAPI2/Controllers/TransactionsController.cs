using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace brandiagaAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [Authorize]
        [HttpPost("pay")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequestDto paymentRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ResponseDTO<object>.Error("Invalid input data."));
            }

            try
            {
                var transaction = await _transactionService.ProcessPaymentAsync(paymentRequestDto);
                return Ok(ResponseDTO<TransactionResponseDto>.Success(transaction, "Payment processed successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpGet("{transactionId:guid}")]
        public async Task<IActionResult> GetTransactionById(Guid transactionId)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(transactionId);
                return Ok(ResponseDTO<TransactionResponseDto>.Success(transaction, "Transaction retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize]
        [HttpGet("order/{orderId:guid}")]
        public async Task<IActionResult> GetTransactionByOrderId(Guid orderId)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByOrderIdAsync(orderId);
                return Ok(ResponseDTO<TransactionResponseDto>.Success(transaction, "Transaction retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllTransactions()
        {
            try
            {
                var transactions = await _transactionService.GetAllTransactionsAsync();
                return Ok(ResponseDTO<IEnumerable<TransactionResponseDto>>.Success(transactions, "Transactions retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }
    }
}
