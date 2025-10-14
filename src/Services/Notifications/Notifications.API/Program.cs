var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Notifications API",
        Version = "v1",
        Description = "Notifications microservice with SignalR for real-time updates"
    });
});

// Add SignalR
builder.Services.AddSignalR();

// Add health checks
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection") 
            ?? "Server=localhost;Database=PubsNotifications;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True",
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
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Notifications API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
