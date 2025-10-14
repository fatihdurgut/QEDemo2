# Developer Quick Reference Guide

## Common Commands

### Building & Testing
```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build Pubs.Microservices.sln

# Build in Release mode
dotnet build Pubs.Microservices.sln --configuration Release

# Run tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Clean solution
dotnet clean
```

### Docker Commands
```bash
# Build all containers
docker-compose build

# Start all services
docker-compose up

# Start in detached mode
docker-compose up -d

# View logs
docker-compose logs -f

# View logs for specific service
docker-compose logs -f authors-api

# Stop all services
docker-compose down

# Remove volumes
docker-compose down -v

# Rebuild specific service
docker-compose up --build authors-api

# Scale a service
docker-compose up --scale authors-api=3
```

### Kubernetes Commands
```bash
# Apply all manifests
kubectl apply -f deploy/k8s/

# Get all resources
kubectl get all -n pubs-microservices

# Get pods
kubectl get pods -n pubs-microservices

# View pod logs
kubectl logs <pod-name> -n pubs-microservices

# Follow logs
kubectl logs -f <pod-name> -n pubs-microservices

# Describe pod
kubectl describe pod <pod-name> -n pubs-microservices

# Delete all resources
kubectl delete -f deploy/k8s/

# Port forward to service
kubectl port-forward svc/authors-api 5001:80 -n pubs-microservices
```

### Helm Commands
```bash
# Install chart
helm install pubs-microservices ./deploy/helm/pubs-microservices \
  --namespace pubs-microservices --create-namespace

# Upgrade release
helm upgrade pubs-microservices ./deploy/helm/pubs-microservices \
  --namespace pubs-microservices

# Uninstall release
helm uninstall pubs-microservices -n pubs-microservices

# List releases
helm list -n pubs-microservices

# Get values
helm get values pubs-microservices -n pubs-microservices

# Dry run
helm install pubs-microservices ./deploy/helm/pubs-microservices \
  --dry-run --debug
```

### Terraform Commands
```bash
# Initialize
cd deploy/terraform
terraform init

# Format
terraform fmt

# Validate
terraform validate

# Plan
terraform plan

# Apply
terraform apply

# Apply with auto-approve
terraform apply -auto-approve

# Destroy
terraform destroy

# Show state
terraform show

# List resources
terraform state list
```

## Service Endpoints (Local)

| Service | Port | URL |
|---------|------|-----|
| API Gateway | 5000 | http://localhost:5000 |
| Authors API | 5001 | http://localhost:5001 |
| Publishers API | 5002 | http://localhost:5002 |
| Titles API | 5003 | http://localhost:5003 |
| Sales API | 5004 | http://localhost:5004 |
| Employees API | 5005 | http://localhost:5005 |
| Stores API | 5006 | http://localhost:5006 |
| Identity API | 5007 | http://localhost:5007 |
| Notifications API | 5008 | http://localhost:5008 |
| Analytics API | 5009 | http://localhost:5009 |

## Infrastructure Endpoints (Local)

| Service | Port | URL | Credentials |
|---------|------|-----|-------------|
| Grafana | 3000 | http://localhost:3000 | admin/admin |
| Prometheus | 9090 | http://localhost:9090 | - |
| Jaeger UI | 16686 | http://localhost:16686 | - |
| RabbitMQ Management | 15672 | http://localhost:15672 | guest/guest |
| SQL Server | 1433 | localhost:1433 | sa/YourStrong@Passw0rd |
| Redis | 6379 | localhost:6379 | - |

## Project Structure Reference

```
src/
├── Services/
│   └── [ServiceName]/
│       ├── [ServiceName].API/          # Controllers, Middleware
│       ├── [ServiceName].Application/  # Commands, Queries, DTOs
│       ├── [ServiceName].Domain/       # Entities, Aggregates, Events
│       └── [ServiceName].Infrastructure/ # Repositories, DbContext
├── BuildingBlocks/
│   ├── Common/                         # Shared utilities
│   ├── EventBus/                       # Event abstractions
│   ├── IntegrationEventLogEF/          # Event sourcing
│   └── WebHost.Customization/          # Web host extensions
└── ApiGateways/
    └── ApiGateway/                     # API Gateway
```

## Environment Variables

### Database
- `ConnectionStrings__DefaultConnection`: SQL Server connection string
- `ConnectionStrings__Redis`: Redis connection string

### Message Broker
- `MessageBroker__Host`: RabbitMQ host
- `MessageBroker__Username`: RabbitMQ username
- `MessageBroker__Password`: RabbitMQ password

### Application
- `ASPNETCORE_ENVIRONMENT`: Environment (Development/Staging/Production)
- `ASPNETCORE_URLS`: Binding URLs

### Logging
- `Logging__LogLevel__Default`: Default log level
- `Logging__LogLevel__Microsoft.AspNetCore`: ASP.NET Core log level

## Troubleshooting

### Container won't start
```bash
# Check logs
docker-compose logs [service-name]

# Check container status
docker ps -a

# Restart service
docker-compose restart [service-name]

# Remove and recreate
docker-compose up -d --force-recreate [service-name]
```

### Database connection issues
```bash
# Check SQL Server is running
docker-compose ps sqlserver

# Check SQL Server logs
docker-compose logs sqlserver

# Connect to SQL Server
docker exec -it pubs-sqlserver /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P 'YourStrong@Passw0rd'
```

### Port conflicts
```bash
# Check what's using a port
lsof -i :5000

# Kill process on port
kill -9 $(lsof -t -i:5000)

# Change ports in docker-compose.override.yml
```

### Build failures
```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build

# Clear NuGet cache
dotnet nuget locals all --clear
```

## Git Workflow

```bash
# Create feature branch
git checkout -b feature/my-feature

# Stage changes
git add .

# Commit changes
git commit -m "Description of changes"

# Push to remote
git push origin feature/my-feature

# Pull latest changes
git pull origin main

# Merge main into feature branch
git merge main
```

## Testing

### Unit Tests
```bash
# Run all tests
dotnet test

# Run tests for specific project
dotnet test src/Services/Authors/Authors.Tests/

# Run with verbose output
dotnet test --logger "console;verbosity=detailed"
```

### Integration Tests
```bash
# Run integration tests (requires Docker)
docker-compose up -d sqlserver redis rabbitmq
dotnet test --filter Category=Integration
```

## Monitoring

### Viewing Metrics
1. Open Grafana: http://localhost:3000
2. Login with admin/admin
3. Navigate to dashboards
4. Select service metrics

### Viewing Traces
1. Open Jaeger: http://localhost:16686
2. Select service from dropdown
3. Find traces by operation or time range

### Viewing Logs
```bash
# View all service logs
docker-compose logs -f

# View specific service
docker-compose logs -f authors-api

# Search logs
docker-compose logs | grep "ERROR"
```

## Additional Resources

- [Architecture Documentation](docs/architecture/README.md)
- [Phase 1 Summary](docs/PHASE1-SUMMARY.md)
- [Main README](README.md)
- [.NET 8 Documentation](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [Docker Documentation](https://docs.docker.com/)
- [Kubernetes Documentation](https://kubernetes.io/docs/)
