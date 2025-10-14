# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Pubs.Microservices.sln", "./"]
COPY ["src/Services/Sales/Sales.API/Sales.API.csproj", "src/Services/Sales/Sales.API/"]
COPY ["src/Services/Sales/Sales.Application/Sales.Application.csproj", "src/Services/Sales/Sales.Application/"]
COPY ["src/Services/Sales/Sales.Domain/Sales.Domain.csproj", "src/Services/Sales/Sales.Domain/"]
COPY ["src/Services/Sales/Sales.Infrastructure/Sales.Infrastructure.csproj", "src/Services/Sales/Sales.Infrastructure/"]
COPY ["src/BuildingBlocks/Common/Common.csproj", "src/BuildingBlocks/Common/"]
COPY ["src/BuildingBlocks/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/"]
COPY ["src/BuildingBlocks/IntegrationEventLogEF/IntegrationEventLogEF.csproj", "src/BuildingBlocks/IntegrationEventLogEF/"]
COPY ["src/BuildingBlocks/WebHost.Customization/WebHost.Customization.csproj", "src/BuildingBlocks/WebHost.Customization/"]

# Restore dependencies
RUN dotnet restore "src/Services/Sales/Sales.API/Sales.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/Services/Sales/Sales.API"
RUN dotnet build "Sales.API.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Sales.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create non-root user
RUN addgroup --system --gid 1000 appgroup && \
    adduser --system --uid 1000 --ingroup appgroup appuser && \
    chown -R appuser:appgroup /app

# Copy published application
COPY --from=publish /app/publish .

# Switch to non-root user
USER appuser

# Expose port
EXPOSE 8080
EXPOSE 8081

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl --fail http://localhost:8080/health || exit 1

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

# Start the application
ENTRYPOINT ["dotnet", "Sales.API.dll"]
