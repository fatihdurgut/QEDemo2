using Analytics.Application.DTOs;
using Analytics.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Analytics.API.Controllers;

/// <summary>
/// Controller for analytics and reporting
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly ILogger<AnalyticsController> _logger;

    public AnalyticsController(
        IAnalyticsService analyticsService,
        ILogger<AnalyticsController> logger)
    {
        _analyticsService = analyticsService;
        _logger = logger;
    }

    /// <summary>
    /// Get sales analytics for a date range
    /// </summary>
    [HttpGet("sales")]
    [ProducesResponseType(typeof(SalesAnalyticsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSalesAnalytics(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var start = startDate ?? DateTime.UtcNow.AddMonths(-1);
            var end = endDate ?? DateTime.UtcNow;

            var analytics = await _analyticsService.GetSalesAnalyticsAsync(start, end);
            return Ok(analytics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sales analytics");
            return StatusCode(500, new { message = "An error occurred while retrieving sales analytics" });
        }
    }

    /// <summary>
    /// Get publisher performance metrics
    /// </summary>
    [HttpGet("publishers/performance")]
    [ProducesResponseType(typeof(IEnumerable<PublisherPerformanceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPublisherPerformance()
    {
        try
        {
            var performance = await _analyticsService.GetPublisherPerformanceAsync();
            return Ok(performance);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting publisher performance");
            return StatusCode(500, new { message = "An error occurred while retrieving publisher performance" });
        }
    }

    /// <summary>
    /// Get author royalties
    /// </summary>
    [HttpGet("authors/royalties")]
    [ProducesResponseType(typeof(IEnumerable<AuthorRoyaltyDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAuthorRoyalties()
    {
        try
        {
            var royalties = await _analyticsService.GetAuthorRoyaltiesAsync();
            return Ok(royalties);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting author royalties");
            return StatusCode(500, new { message = "An error occurred while retrieving author royalties" });
        }
    }

    /// <summary>
    /// Get top selling titles
    /// </summary>
    [HttpGet("titles/top-selling")]
    [ProducesResponseType(typeof(IEnumerable<TopSellingTitleDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopSellingTitles([FromQuery] int count = 10)
    {
        try
        {
            if (count <= 0 || count > 100)
                return BadRequest(new { message = "Count must be between 1 and 100" });

            var titles = await _analyticsService.GetTopSellingTitlesAsync(count);
            return Ok(titles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting top selling titles");
            return StatusCode(500, new { message = "An error occurred while retrieving top selling titles" });
        }
    }

    /// <summary>
    /// Get sales by period (month/quarter/year)
    /// </summary>
    [HttpGet("sales/by-period")]
    [ProducesResponseType(typeof(IEnumerable<SalesByPeriodDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSalesByPeriod(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] string periodType = "month")
    {
        try
        {
            var start = startDate ?? DateTime.UtcNow.AddMonths(-6);
            var end = endDate ?? DateTime.UtcNow;

            if (!new[] { "month", "quarter", "year" }.Contains(periodType.ToLower()))
                return BadRequest(new { message = "Period type must be 'month', 'quarter', or 'year'" });

            var salesByPeriod = await _analyticsService.GetSalesByPeriodAsync(start, end, periodType);
            return Ok(salesByPeriod);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting sales by period");
            return StatusCode(500, new { message = "An error occurred while retrieving sales by period" });
        }
    }

    /// <summary>
    /// Get inventory status
    /// </summary>
    [HttpGet("inventory/status")]
    [ProducesResponseType(typeof(IEnumerable<InventoryStatusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetInventoryStatus()
    {
        try
        {
            var inventory = await _analyticsService.GetInventoryStatusAsync();
            return Ok(inventory);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting inventory status");
            return StatusCode(500, new { message = "An error occurred while retrieving inventory status" });
        }
    }
}
