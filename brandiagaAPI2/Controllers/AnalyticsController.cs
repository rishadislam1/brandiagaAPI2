using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace brandiagaAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public AnalyticsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("sales-report")]
        public async Task<IActionResult> GenerateSalesReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var report = await _reportService.GenerateSalesReportAsync(startDate, endDate);
                return Ok(ResponseDTO<ReportResponseDto>.Success(report, "Sales report generated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("order-status-report")]
        public async Task<IActionResult> GenerateOrderStatusReport()
        {
            try
            {
                var report = await _reportService.GenerateOrderStatusReportAsync();
                return Ok(ResponseDTO<ReportResponseDto>.Success(report, "Order status report generated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{reportId:guid}")]
        public async Task<IActionResult> GetReportById(Guid reportId)
        {
            try
            {
                var report = await _reportService.GetReportByIdAsync(reportId);
                return Ok(ResponseDTO<ReportResponseDto>.Success(report, "Report retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            try
            {
                var reports = await _reportService.GetAllReportsAsync();
                return Ok(ResponseDTO<IEnumerable<ReportResponseDto>>.Success(reports, "Reports retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }
    }
}