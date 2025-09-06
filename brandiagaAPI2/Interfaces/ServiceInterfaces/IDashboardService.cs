using brandiagaAPI2.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace brandiagaAPI2.Interfaces.ServiceInterfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetDashboardSummaryAsync();
        Task<List<SalesDataDto>> GetSalesOverTimeAsync(DateTime startDate, DateTime endDate);
        Task<List<OrderDto>> GetRecentOrdersAsync(int page, int pageSize);
        Task<DashboardReportResponseDto> GenerateSalesReportAsync();
        Task<DashboardReportResponseDto> GetReportByIdAsync(Guid reportId);
    }
}