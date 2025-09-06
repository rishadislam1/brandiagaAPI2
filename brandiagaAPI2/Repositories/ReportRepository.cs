using brandiagaAPI2.Data.Models;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace brandiagaAPI2.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly DbAbdeaeDotnet1Context _context;

        public ReportRepository(DbAbdeaeDotnet1Context context)
        {
            _context = context;
        }

        public async Task<Report> GetReportByIdAsync(Guid reportId)
        {
            return await _context.Reports
                .FirstOrDefaultAsync(r => r.ReportId == reportId);
        }

        public async Task<IEnumerable<Report>> GetAllReportsAsync()
        {
            return await _context.Reports
                .ToListAsync();
        }

        public async Task AddReportAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
        }
    }
}
