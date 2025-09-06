using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brandiagaAPI2.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DbAbdeaeDotnet1Context _context;

        public DashboardRepository(DbAbdeaeDotnet1Context context)
        {
            _context = context;
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync(u => u.IsActive);
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            return await _context.Orders.CountAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _context.Orders
                .Where(o => o.Status == "Completed")
                .SumAsync(o => o.TotalAmount);
        }

        public async Task<int> GetTotalProductsAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<int> GetPendingOrdersAsync()
        {
            return await _context.Orders
                .CountAsync(o => o.Status == "Pending");
        }

        public async Task<decimal> GetAverageOrderValueAsync()
        {
            var completedOrders = await _context.Orders
                .Where(o => o.Status == "Completed")
                .ToListAsync();
            return completedOrders.Any() ? completedOrders.Average(o => o.TotalAmount) : 0;
        }

        public async Task<double> GetConversionRateAsync()
        {
            var completedOrders = await _context.Orders.CountAsync(o => o.Status == "Completed");
            var totalOrders = await _context.Orders.CountAsync();
            return totalOrders > 0 ? (completedOrders / (double)totalOrders) * 100 : 0;
        }

        public async Task<double> GetCartAbandonmentRateAsync()
        {
            var pendingOrders = await _context.Orders.CountAsync(o => o.Status == "Pending");
            var totalOrders = await _context.Orders.CountAsync();
            return totalOrders > 0 ? (pendingOrders / (double)totalOrders) * 100 : 0;
        }

        public async Task<int> GetNewCustomersAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Users
                .CountAsync(u => u.IsActive && u.CreatedAt >= startDate && u.CreatedAt <= endDate);
        }

        public async Task<int> GetReturningCustomersAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .GroupBy(o => o.UserId)
                .CountAsync(g => g.Count() > 1);
        }

        public async Task<List<(string MonthYear, decimal Sales)>> GetSalesOverTimeAsync(DateTime startDate, DateTime endDate)
        {
            var query = _context.Orders
                .Where(o => o.Status == "Completed" && o.OrderDate >= startDate && o.OrderDate <= endDate)
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Sales = g.Sum(o => o.TotalAmount)
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month);

            var results = await query.ToListAsync();

            return results.Select(g => (
                MonthYear: $"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Month)} {g.Year}",
                Sales: g.Sales
            )).ToList();
        }

        public async Task<List<Order>> GetRecentOrdersAsync(int page, int pageSize)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task AddReportAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task<Report> GetReportByIdAsync(Guid reportId)
        {
            return await _context.Reports
                .FirstOrDefaultAsync(r => r.ReportId == reportId);
        }
    }
}