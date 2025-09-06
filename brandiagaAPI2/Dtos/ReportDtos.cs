namespace brandiagaAPI2.Dtos
{
    public class SalesReportDto
    {
        public decimal TotalSales { get; set; }
        public int OrderCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class OrderStatusReportDto
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }

    public class ReportResponseDto
    {
        public Guid ReportId { get; set; }
        public string ReportType { get; set; }
        public string DataJson { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
