using Analytics.Application.DTOs;

namespace Analytics.Application.Services;

/// <summary>
/// Service for analytics and reporting
/// </summary>
public interface IAnalyticsService
{
    Task<SalesAnalyticsDto> GetSalesAnalyticsAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<PublisherPerformanceDto>> GetPublisherPerformanceAsync();
    Task<IEnumerable<AuthorRoyaltyDto>> GetAuthorRoyaltiesAsync();
    Task<IEnumerable<TopSellingTitleDto>> GetTopSellingTitlesAsync(int count = 10);
    Task<IEnumerable<SalesByPeriodDto>> GetSalesByPeriodAsync(DateTime startDate, DateTime endDate, string periodType = "month");
    Task<IEnumerable<InventoryStatusDto>> GetInventoryStatusAsync();
}
