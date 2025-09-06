using brandiagaAPI2.Data.Models;

namespace brandiagaAPI2.Interfaces.RepositoryInterfaces
{
    public interface IReportRepository
    {
        Task<Report> GetReportByIdAsync(Guid reportId);
        Task<IEnumerable<Report>> GetAllReportsAsync();
        Task AddReportAsync(Report report);
    }
}
