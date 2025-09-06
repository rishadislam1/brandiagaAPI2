using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace brandiagaAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            try
            {
                var summary = await _dashboardService.GetDashboardSummaryAsync();
                return Ok(ResponseDTO<DashboardSummaryDto>.Success(summary, "Dashboard summary retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [HttpGet("sales-over-time")]
        public async Task<IActionResult> GetSalesOverTime([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var salesData = await _dashboardService.GetSalesOverTimeAsync(startDate, endDate);
                return Ok(ResponseDTO<List<SalesDataDto>>.Success(salesData, "Sales data retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [HttpGet("recent-orders")]
        public async Task<IActionResult> GetRecentOrders([FromQuery] int page = 0, [FromQuery] int pageSize = 5)
        {
            try
            {
                var orders = await _dashboardService.GetRecentOrdersAsync(page, pageSize);
                return Ok(ResponseDTO<List<OrderDto>>.Success(orders, "Recent orders retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [HttpPost("sales-report")]
        public async Task<IActionResult> GenerateSalesReport()
        {
            try
            {
                var report = await _dashboardService.GenerateSalesReportAsync();
                return Ok(ResponseDTO<DashboardReportResponseDto>.Success(report, "Sales report generated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }

        [HttpGet("report/{reportId:guid}")]
        public async Task<IActionResult> GetReport(Guid reportId)
        {
            try
            {
                var report = await _dashboardService.GetReportByIdAsync(reportId);
                return Ok(ResponseDTO<DashboardReportResponseDto>.Success(report, "Report retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseDTO<object>.Error(ex.Message));
            }
        }
    }
}