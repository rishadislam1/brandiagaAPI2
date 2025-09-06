namespace brandiagaAPI2.Dtos
{
    public class SalesDataDto
    {
        public string Name { get; set; } // e.g., "Jan"
        public decimal Sales { get; set; }
    }

    public class OrderDto
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string Product { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
    }

    public class DashboardSummaryDto
    {
        public int TotalUsers { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalProducts { get; set; }
        public int PendingOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public double ConversionRate { get; set; }
        public double CartAbandonmentRate { get; set; }
        public int NewCustomers { get; set; }
        public int ReturningCustomers { get; set; }
    }

    public class DashboardReportResponseDto
    {
        public Guid ReportId { get; set; }
        public string ReportType { get; set; }
        public string ReportData { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}