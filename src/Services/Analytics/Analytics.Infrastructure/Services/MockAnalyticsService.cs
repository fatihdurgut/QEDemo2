using Analytics.Application.DTOs;
using Analytics.Application.Services;

namespace Analytics.Infrastructure.Services;

/// <summary>
/// Mock analytics service implementation with sample data
/// TODO: Replace with actual data aggregation from other services
/// </summary>
public class MockAnalyticsService : IAnalyticsService
{
    public Task<SalesAnalyticsDto> GetSalesAnalyticsAsync(DateTime startDate, DateTime endDate)
    {
        // Mock data - in real implementation, aggregate from Sales service
        var analytics = new SalesAnalyticsDto(
            TotalSales: 150000.00m,
            TotalOrders: 450,
            AverageOrderValue: 333.33m,
            StartDate: startDate,
            EndDate: endDate);

        return Task.FromResult(analytics);
    }

    public Task<IEnumerable<PublisherPerformanceDto>> GetPublisherPerformanceAsync()
    {
        // Mock data - in real implementation, aggregate from Publishers and Sales services
        var performance = new List<PublisherPerformanceDto>
        {
            new(Guid.NewGuid(), "New Moon Books", 15, 75000.00m, 250),
            new(Guid.NewGuid(), "Binnet & Hardley", 12, 60000.00m, 200),
            new(Guid.NewGuid(), "Algodata Infosystems", 8, 35000.00m, 120)
        };

        return Task.FromResult<IEnumerable<PublisherPerformanceDto>>(performance);
    }

    public Task<IEnumerable<AuthorRoyaltyDto>> GetAuthorRoyaltiesAsync()
    {
        // Mock data - in real implementation, aggregate from Authors and Sales services
        var royalties = new List<AuthorRoyaltyDto>
        {
            new(Guid.NewGuid(), "Anne Ringer", 3, 12500.00m, 0.10m),
            new(Guid.NewGuid(), "Abraham Bennet", 2, 8750.00m, 0.12m),
            new(Guid.NewGuid(), "Cheryl Carson", 2, 7500.00m, 0.08m)
        };

        return Task.FromResult<IEnumerable<AuthorRoyaltyDto>>(royalties);
    }

    public Task<IEnumerable<TopSellingTitleDto>> GetTopSellingTitlesAsync(int count = 10)
    {
        // Mock data - in real implementation, aggregate from Titles and Sales services
        var topTitles = new List<TopSellingTitleDto>
        {
            new(Guid.NewGuid(), "The Busy Executive's Database Guide", "Algodata Infosystems", 150, 30000.00m),
            new(Guid.NewGuid(), "Cooking with Computers", "Algodata Infosystems", 120, 24000.00m),
            new(Guid.NewGuid(), "You Can Combat Computer Stress!", "New Moon Books", 100, 20000.00m),
            new(Guid.NewGuid(), "Straight Talk About Computers", "Algodata Infosystems", 85, 17000.00m),
            new(Guid.NewGuid(), "Silicon Valley Gastronomic Treats", "Binnet & Hardley", 75, 15000.00m)
        };

        return Task.FromResult<IEnumerable<TopSellingTitleDto>>(topTitles.Take(count));
    }

    public Task<IEnumerable<SalesByPeriodDto>> GetSalesByPeriodAsync(DateTime startDate, DateTime endDate, string periodType = "month")
    {
        // Mock data - in real implementation, aggregate from Sales service
        var salesByPeriod = new List<SalesByPeriodDto>
        {
            new("2024-10", 45000.00m, 150),
            new("2024-09", 52000.00m, 175),
            new("2024-08", 48000.00m, 160),
            new("2024-07", 50000.00m, 165)
        };

        return Task.FromResult<IEnumerable<SalesByPeriodDto>>(salesByPeriod);
    }

    public Task<IEnumerable<InventoryStatusDto>> GetInventoryStatusAsync()
    {
        // Mock data - in real implementation, aggregate from Titles service
        var inventory = new List<InventoryStatusDto>
        {
            new(Guid.NewGuid(), "The Busy Executive's Database Guide", 150, "In Stock"),
            new(Guid.NewGuid(), "Cooking with Computers", 45, "In Stock"),
            new(Guid.NewGuid(), "You Can Combat Computer Stress!", 8, "Low Stock"),
            new(Guid.NewGuid(), "Straight Talk About Computers", 0, "Out of Stock"),
            new(Guid.NewGuid(), "Silicon Valley Gastronomic Treats", 120, "In Stock")
        };

        return Task.FromResult<IEnumerable<InventoryStatusDto>>(inventory);
    }
}
