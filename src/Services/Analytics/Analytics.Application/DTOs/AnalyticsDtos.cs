namespace Analytics.Application.DTOs;

/// <summary>
/// Sales analytics summary
/// </summary>
public record SalesAnalyticsDto(
    decimal TotalSales,
    int TotalOrders,
    decimal AverageOrderValue,
    DateTime StartDate,
    DateTime EndDate);

/// <summary>
/// Publisher performance metrics
/// </summary>
public record PublisherPerformanceDto(
    Guid PublisherId,
    string PublisherName,
    int TitlesPublished,
    decimal TotalRevenue,
    int TotalSales);

/// <summary>
/// Author royalties summary
/// </summary>
public record AuthorRoyaltyDto(
    Guid AuthorId,
    string AuthorName,
    int TitlesPublished,
    decimal TotalRoyalties,
    decimal AverageRoyaltyPercentage);

/// <summary>
/// Top selling titles
/// </summary>
public record TopSellingTitleDto(
    Guid TitleId,
    string TitleName,
    string PublisherName,
    int UnitsSold,
    decimal TotalRevenue);

/// <summary>
/// Sales by period
/// </summary>
public record SalesByPeriodDto(
    string Period, // e.g., "2024-01", "2024-Q1"
    decimal TotalSales,
    int TotalOrders);

/// <summary>
/// Inventory status
/// </summary>
public record InventoryStatusDto(
    Guid TitleId,
    string TitleName,
    int CurrentStock,
    string Status); // "In Stock", "Low Stock", "Out of Stock"
