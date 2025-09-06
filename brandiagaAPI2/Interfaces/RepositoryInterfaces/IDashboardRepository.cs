using brandiagaAPI2.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace brandiagaAPI2.Interfaces.RepositoryInterfaces
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalUsersAsync();
        Task<int> GetTotalOrdersAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<int> GetTotalProductsAsync();
        Task<int> GetPendingOrdersAsync();
        Task<decimal> GetAverageOrderValueAsync();
        Task<double> GetConversionRateAsync();
        Task<double> GetCartAbandonmentRateAsync();
        Task<int> GetNewCustomersAsync(DateTime startDate, DateTime endDate);
        Task<int> GetReturningCustomersAsync(DateTime startDate, DateTime endDate);
        Task<List<(string MonthYear, decimal Sales)>> GetSalesOverTimeAsync(DateTime startDate, DateTime endDate);
        Task<List<Order>> GetRecentOrdersAsync(int page, int pageSize);
        Task AddReportAsync(Report report);
        Task<Report> GetReportByIdAsync(Guid reportId);
    }
}