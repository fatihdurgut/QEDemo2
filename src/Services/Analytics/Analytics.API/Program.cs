using Analytics.Application.Services;
using Analytics.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Analytics API",
        Version = "v1",
        Description = "Analytics and reporting microservice for business intelligence"
    });
});

// Register application services
builder.Services.AddScoped<IAnalyticsService, MockAnalyticsService>();

// Add health checks
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection") 
            ?? "Server=localhost;Database=PubsAnalytics;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True",
        name: "database")
    .AddRedis(
        builder.Configuration.GetConnectionString("Redis") 
            ?? "localhost:6379",
        name: "redis");

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Analytics API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
