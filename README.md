# Pubs Microservices Application

A modern, cloud-native microservices application built with .NET 8, following Domain-Driven Design (DDD) principles and microservices architecture patterns.

## 🏗️ Architecture

This application implements a complete microservices architecture for the Pubs database domain, including:

- **9 Microservices**: Authors, Publishers, Titles, Sales, Employees, Stores, Identity, Notifications, Analytics
- **API Gateway**: Centralized entry point using Ocelot
- **Shared Building Blocks**: Common libraries for cross-cutting concerns
- **Infrastructure**: Complete containerization, orchestration, and monitoring setup

## 🚀 Quick Start

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [kubectl](https://kubernetes.io/docs/tasks/tools/) (for Kubernetes deployment)
- [Helm](https://helm.sh/docs/intro/install/) (for Kubernetes deployment)
- [Terraform](https://www.terraform.io/downloads) (for infrastructure provisioning)

### Local Development with Docker Compose

1. Clone the repository:
```bash
git clone https://github.com/fatihdurgut/QEDemo2.git
cd QEDemo2
```

2. Build and run all services:
```bash
docker-compose up --build
```

3. Access the services:
- API Gateway: http://localhost:5000
- Authors Service: http://localhost:5001
- Publishers Service: http://localhost:5002
- Titles Service: http://localhost:5003
- Sales Service: http://localhost:5004
- Employees Service: http://localhost:5005
- Stores Service: http://localhost:5006
- Identity Service: http://localhost:5007
- Notifications Service: http://localhost:5008
- Analytics Service: http://localhost:5009
- Grafana: http://localhost:3000 (admin/admin)
- Prometheus: http://localhost:9090
- Jaeger: http://localhost:16686
- RabbitMQ Management: http://localhost:15672 (guest/guest)

### Building the Solution

```bash
dotnet restore Pubs.Microservices.sln
dotnet build Pubs.Microservices.sln --configuration Release
dotnet test Pubs.Microservices.sln --configuration Release
```

## 📁 Project Structure

```
.
├── src/
│   ├── Services/                    # Microservices
│   │   ├── Authors/                 # Authors service
│   │   │   ├── Authors.API/         # API layer
│   │   │   ├── Authors.Application/ # Application layer (CQRS)
│   │   │   ├── Authors.Domain/      # Domain layer (DDD)
│   │   │   └── Authors.Infrastructure/ # Infrastructure layer
│   │   ├── Publishers/              # Publishers service
│   │   ├── Titles/                  # Titles service
│   │   ├── Sales/                   # Sales service
│   │   ├── Employees/               # Employees service
│   │   ├── Stores/                  # Stores service
│   │   ├── Identity/                # Identity & authentication
│   │   ├── Notifications/           # Notification service
│   │   └── Analytics/               # Analytics & reporting
│   ├── BuildingBlocks/              # Shared libraries
│   │   ├── Common/                  # Common utilities
│   │   ├── EventBus/                # Event bus abstractions
│   │   ├── IntegrationEventLogEF/   # Event sourcing
│   │   └── WebHost.Customization/   # Web host extensions
│   └── ApiGateways/
│       └── ApiGateway/              # API Gateway (Ocelot)
├── deploy/
│   ├── k8s/                         # Kubernetes manifests
│   ├── helm/                        # Helm charts
│   ├── terraform/                   # Infrastructure as Code
│   ├── monitoring/                  # Monitoring configuration
│   └── database/                    # Database scripts
├── docs/                            # Documentation
└── .github/
    └── workflows/                   # CI/CD pipelines
```

## 🐳 Docker & Kubernetes Deployment

### Docker Compose (Local Development)

```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop all services
docker-compose down
```

### Kubernetes Deployment

1. Apply Kubernetes manifests:
```bash
kubectl apply -f deploy/k8s/namespace.yaml
kubectl apply -f deploy/k8s/configmaps/
kubectl apply -f deploy/k8s/secrets/
kubectl apply -f deploy/k8s/deployments/
```

2. Or use Helm:
```bash
helm install pubs-microservices ./deploy/helm/pubs-microservices \
  --namespace pubs-microservices \
  --create-namespace
```

## ☁️ Infrastructure Provisioning

### Azure Infrastructure with Terraform

1. Initialize Terraform:
```bash
cd deploy/terraform
terraform init
```

2. Plan the deployment:
```bash
terraform plan -var-file="environments/dev/terraform.tfvars"
```

3. Apply the configuration:
```bash
terraform apply -var-file="environments/dev/terraform.tfvars"
```

## 🔍 Monitoring & Observability

### Prometheus & Grafana
- **Prometheus**: Metrics collection at http://localhost:9090
- **Grafana**: Visualization at http://localhost:3000

### Jaeger
- **Distributed Tracing**: http://localhost:16686

### Application Insights
- Configured for Azure deployments with comprehensive telemetry

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run with code coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test src/Services/Authors/Authors.Tests/Authors.Tests.csproj
```

## 🔐 Security

- **Authentication**: OAuth 2.0 / OpenID Connect
- **Authorization**: JWT tokens with role-based access control
- **Secrets Management**: Azure Key Vault integration
- **Container Security**: Non-root users, minimal base images
- **Network Security**: Network policies and encryption in transit

## 🛠️ Technologies

- **.NET 8**: Latest .NET platform
- **ASP.NET Core**: Web API framework
- **Entity Framework Core**: ORM for data access
- **MediatR**: CQRS pattern implementation
- **Docker**: Containerization
- **Kubernetes**: Container orchestration
- **Helm**: Kubernetes package manager
- **Terraform**: Infrastructure as Code
- **RabbitMQ**: Message broker
- **Redis**: Distributed caching
- **SQL Server**: Primary database
- **Prometheus**: Metrics collection
- **Grafana**: Metrics visualization
- **Jaeger**: Distributed tracing
- **GitHub Actions**: CI/CD

## 📝 Development Practices

- **Domain-Driven Design (DDD)**: Rich domain models and bounded contexts
- **CQRS**: Command Query Responsibility Segregation
- **Event Sourcing**: Complete audit trail
- **Microservices Patterns**: Circuit breaker, retry, bulkhead
- **12-Factor App**: Cloud-native application principles
- **GitOps**: Infrastructure and deployment automation

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 📧 Contact

Project Link: [https://github.com/fatihdurgut/QEDemo2](https://github.com/fatihdurgut/QEDemo2)

## 🙏 Acknowledgments

- [eShopOnContainers](https://github.com/dotnet-architecture/eShopOnContainers) - Reference architecture
- [Microsoft Architecture Guides](https://docs.microsoft.com/en-us/dotnet/architecture/)
- [Domain-Driven Design by Eric Evans](https://www.domainlanguage.com/ddd/)
