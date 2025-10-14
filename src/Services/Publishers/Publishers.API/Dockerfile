# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Pubs.Microservices.sln", "./"]
COPY ["src/Services/Publishers/Publishers.API/Publishers.API.csproj", "src/Services/Publishers/Publishers.API/"]
COPY ["src/Services/Publishers/Publishers.Application/Publishers.Application.csproj", "src/Services/Publishers/Publishers.Application/"]
COPY ["src/Services/Publishers/Publishers.Domain/Publishers.Domain.csproj", "src/Services/Publishers/Publishers.Domain/"]
COPY ["src/Services/Publishers/Publishers.Infrastructure/Publishers.Infrastructure.csproj", "src/Services/Publishers/Publishers.Infrastructure/"]
COPY ["src/BuildingBlocks/Common/Common.csproj", "src/BuildingBlocks/Common/"]
COPY ["src/BuildingBlocks/EventBus/EventBus.csproj", "src/BuildingBlocks/EventBus/"]
COPY ["src/BuildingBlocks/IntegrationEventLogEF/IntegrationEventLogEF.csproj", "src/BuildingBlocks/IntegrationEventLogEF/"]
COPY ["src/BuildingBlocks/WebHost.Customization/WebHost.Customization.csproj", "src/BuildingBlocks/WebHost.Customization/"]

# Restore dependencies
RUN dotnet restore "src/Services/Publishers/Publishers.API/Publishers.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/src/Services/Publishers/Publishers.API"
RUN dotnet build "Publishers.API.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Publishers.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

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
ENTRYPOINT ["dotnet", "Publishers.API.dll"]
