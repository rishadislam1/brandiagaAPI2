using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace brandiagaAPI2.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
        {
            var startDate = DateTime.UtcNow.AddMonths(-1); // Last 30 days for customer metrics
            var endDate = DateTime.UtcNow;

            return new DashboardSummaryDto
            {
                TotalUsers = await _dashboardRepository.GetTotalUsersAsync(),
                TotalOrders = await _dashboardRepository.GetTotalOrdersAsync(),
                TotalRevenue = await _dashboardRepository.GetTotalRevenueAsync(),
                TotalProducts = await _dashboardRepository.GetTotalProductsAsync(),
                PendingOrders = await _dashboardRepository.GetPendingOrdersAsync(),
                AverageOrderValue = await _dashboardRepository.GetAverageOrderValueAsync(),
                ConversionRate = await _dashboardRepository.GetConversionRateAsync(),
                CartAbandonmentRate = await _dashboardRepository.GetCartAbandonmentRateAsync(),
                NewCustomers = await _dashboardRepository.GetNewCustomersAsync(startDate, endDate),
                ReturningCustomers = await _dashboardRepository.GetReturningCustomersAsync(startDate, endDate)
            };
        }

        public async Task<List<SalesDataDto>> GetSalesOverTimeAsync(DateTime startDate, DateTime endDate)
        {
            var salesData = await _dashboardRepository.GetSalesOverTimeAsync(startDate, endDate);
            return salesData.Select(s => new SalesDataDto
            {
                Name = s.MonthYear,
                Sales = s.Sales
            }).ToList();
        }

        public async Task<List<OrderDto>> GetRecentOrdersAsync(int page, int pageSize)
        {
            var orders = await _dashboardRepository.GetRecentOrdersAsync(page, pageSize);
            return orders.Select(o => new OrderDto
            {
                Id = o.OrderId,
                CustomerName = $"{o.User.FirstName} {o.User.LastName}",
                Product = o.OrderItems.FirstOrDefault()?.Product.Name ?? "N/A", // Use first OrderItem's Product
                Amount = o.TotalAmount,
                Status = o.Status
            }).ToList();
        }

        public async Task<DashboardReportResponseDto> GenerateSalesReportAsync()
        {
            var totalSales = await _dashboardRepository.GetTotalRevenueAsync();
            var reportData = new
            {
                totalSales,
                GeneratedOn = DateTime.UtcNow
            };

            var report = new Report
            {
                ReportId = Guid.NewGuid(),
                ReportType = "Sales",
                DataJson = JsonSerializer.Serialize(reportData),
                GeneratedAt = DateTime.UtcNow
            };

            await _dashboardRepository.AddReportAsync(report);

            return new DashboardReportResponseDto
            {
                ReportId = report.ReportId,
                ReportType = report.ReportType,
                ReportData = report.DataJson,
                GeneratedAt = report.GeneratedAt
            };
        }

        public async Task<DashboardReportResponseDto> GetReportByIdAsync(Guid reportId)
        {
            var report = await _dashboardRepository.GetReportByIdAsync(reportId);
            if (report == null)
            {
                throw new Exception("Report not found");
            }

            return new DashboardReportResponseDto
            {
                ReportId = report.ReportId,
                ReportType = report.ReportType,
                ReportData = report.DataJson,
                GeneratedAt = report.GeneratedAt
            };
        }
    }
}