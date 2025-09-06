using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using Newtonsoft.Json;

namespace brandiagaAPI2.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ITransactionRepository _transactionRepository;

        public ReportService(IReportRepository reportRepository, IOrderRepository orderRepository, ITransactionRepository transactionRepository)
        {
            _reportRepository = reportRepository;
            _orderRepository = orderRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<ReportResponseDto> GenerateSalesReportAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new Exception("Start date must be before end date");
            }

            var orders = await _orderRepository.GetAllOrdersAsync();
            var transactions = await _transactionRepository.GetAllTransactionsAsync();

            var relevantOrders = orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.Status == "Completed")
                .ToList();
            var relevantTransactions = transactions
                .Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate && t.TransactionStatus == "Completed")
                .ToList();

            var totalSales = relevantOrders.Sum(o => o.TotalAmount);
            var orderCount = relevantOrders.Count;
            var salesData = new SalesReportDto
            {
                TotalSales = totalSales,
                OrderCount = orderCount,
                StartDate = startDate,
                EndDate = endDate
            };

            var dataJson = JsonConvert.SerializeObject(salesData);
            var report = new Report
            {
                ReportId = Guid.NewGuid(),
                ReportType = "SalesReport",
                DataJson = dataJson,
                GeneratedAt = DateTime.UtcNow
            };

            await _reportRepository.AddReportAsync(report);

            return new ReportResponseDto
            {
                ReportId = report.ReportId,
                ReportType = report.ReportType,
                DataJson = report.DataJson,
                GeneratedAt = report.GeneratedAt
            };
        }

        public async Task<ReportResponseDto> GenerateOrderStatusReportAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();

            var statusCounts = orders
                .GroupBy(o => o.Status)
                .Select(g => new OrderStatusReportDto
                {
                    Status = g.Key,
                    Count = g.Count()
                })
                .ToList();

            var dataJson = JsonConvert.SerializeObject(statusCounts);
            var report = new Report
            {
                ReportId = Guid.NewGuid(),
                ReportType = "OrderStatusReport",
                DataJson = dataJson,
                GeneratedAt = DateTime.UtcNow
            };

            await _reportRepository.AddReportAsync(report);

            return new ReportResponseDto
            {
                ReportId = report.ReportId,
                ReportType = report.ReportType,
                DataJson = report.DataJson,
                GeneratedAt = report.GeneratedAt
            };
        }

        public async Task<ReportResponseDto> GetReportByIdAsync(Guid reportId)
        {
            var report = await _reportRepository.GetReportByIdAsync(reportId);
            if (report == null)
            {
                throw new Exception("Report not found");
            }

            return new ReportResponseDto
            {
                ReportId = report.ReportId,
                ReportType = report.ReportType,
                DataJson = report.DataJson,
                GeneratedAt = report.GeneratedAt
            };
        }

        public async Task<IEnumerable<ReportResponseDto>> GetAllReportsAsync()
        {
            var reports = await _reportRepository.GetAllReportsAsync();
            return reports.Select(r => new ReportResponseDto
            {
                ReportId = r.ReportId,
                ReportType = r.ReportType,
                DataJson = r.DataJson,
                GeneratedAt = r.GeneratedAt
            }).ToList();
        }
    }
}
