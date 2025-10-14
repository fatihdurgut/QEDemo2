# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Pubs.Microservices.sln", "./"]
COPY ["src/Services/Employees/Employees.API/Employees.API.csproj", "src/Services/Employees/Employees.API/"]
COPY ["src/Services/Employees/Employees.Application/Employees.Application.csproj", "src/Services/Employees/Employees.Application/"]
COPY ["src/Services/Employees/Employees.Domain/Employees.Domain.csproj", "src/Services/Employees/Employees.Domain/"]
COPY ["src/Services/Employees/Employees.Infrastructure/Employees.Infrastructure.csproj", "src/Services/Employees/Employees.Infrastructure/"]
COPY ["src/BuildingBlocks/Common/Common.csproj", "src/BuildingBlocks/Common/"]
COPY ["src/BuildingBlocks/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/"]
COPY ["src/BuildingBlocks/IntegrationEventLogEF/IntegrationEventLogEF.csproj", "src/BuildingBlocks/IntegrationEventLogEF/"]
COPY ["src/BuildingBlocks/WebHost.Customization/WebHost.Customization.csproj", "src/BuildingBlocks/WebHost.Customization/"]

# Restore dependencies
RUN dotnet restore "src/Services/Employees/Employees.API/Employees.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/Services/Employees/Employees.API"
RUN dotnet build "Employees.API.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Employees.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

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
ENTRYPOINT ["dotnet", "Employees.API.dll"]
