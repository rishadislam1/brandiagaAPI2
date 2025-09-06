using brandiagaAPI2.Dtos;

namespace brandiagaAPI2.Interfaces.ServiceInterfaces
{
    public interface IReportService
    {
        Task<ReportResponseDto> GenerateSalesReportAsync(DateTime startDate, DateTime endDate);
        Task<ReportResponseDto> GenerateOrderStatusReportAsync();
        Task<ReportResponseDto> GetReportByIdAsync(Guid reportId);
        Task<IEnumerable<ReportResponseDto>> GetAllReportsAsync();
    }
}
