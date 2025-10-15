# Testing Strategy & Guidelines

## Overview

This document outlines the comprehensive testing strategy for the Pubs Microservices Application, covering unit tests, integration tests, end-to-end tests, and specialized testing approaches.

## Testing Philosophy

- **Test Early, Test Often**: Tests are written alongside production code
- **85% Minimum Coverage**: Domain and Application layers must maintain at least 85% code coverage
- **Test Naming Convention**: All tests follow the `MethodName_Condition_ExpectedResult()` pattern
- **Quality Gates**: Automated tests run in CI/CD pipelines with quality gates preventing deployment of failing builds

## Test Structure

```
tests/
├── Services/
│   └── [ServiceName]/
│       ├── [ServiceName].UnitTests/          # Unit tests for domain logic
│       │   ├── Domain/                       # Domain entity tests
│       │   ├── Application/                  # Application service tests
│       │   └── API/                          # Controller tests
│       ├── [ServiceName].IntegrationTests/   # Integration tests
│       │   ├── API/                          # API endpoint tests
│       │   ├── Infrastructure/               # Repository tests
│       │   └── EventBus/                     # Message bus tests
│       └── [ServiceName].FunctionalTests/    # End-to-end tests
├── WebApps/
│   └── PubsWebApp/
│       ├── unit/                             # Angular unit tests
│       ├── integration/                      # Component integration tests
│       └── e2e/                              # Cypress E2E tests
└── LoadTests/                                # Performance testing scripts
```

## Backend Testing

### Unit Tests

Unit tests focus on testing individual components in isolation, with all dependencies mocked.

#### Technologies
- **xUnit**: Test framework
- **Moq**: Mocking framework
- **FluentAssertions**: Assertion library for readable test assertions
- **Coverlet**: Code coverage tool

#### Running Unit Tests

```bash
# Run all unit tests
dotnet test --filter "FullyQualifiedName~UnitTests"

# Run unit tests with coverage
dotnet test tests/Services/Employees/Employees.UnitTests \
  /p:CollectCoverage=true \
  /p:CoverletOutputFormat=cobertura \
  /p:Threshold=85

# Run tests for a specific service
dotnet test tests/Services/Employees/Employees.UnitTests
```

#### Example Unit Test

```csharp
[Fact(DisplayName = "Constructor creates valid employee with all properties")]
public void Constructor_ValidParameters_CreatesEmployeeSuccessfully()
{
    // Arrange - Setup test data and dependencies
    var employeeId = "EMP001";
    var firstName = "John";
    var lastName = "Doe";
    var hireDate = DateTime.UtcNow.AddYears(-5);
    var jobLevel = "Senior Developer";

    // Act - Execute the method being tested
    var employee = new Employee(employeeId, firstName, lastName, null, hireDate, jobLevel);

    // Assert - Verify the expected outcome
    employee.EmployeeId.Should().Be(employeeId);
    employee.FirstName.Should().Be(firstName);
    employee.IsActive.Should().BeTrue();
}
```

#### Coverage Requirements

- **Domain Layer**: Minimum 90% line coverage
- **Application Layer**: Minimum 85% line coverage
- **Controllers**: Minimum 80% line coverage

### Integration Tests

Integration tests verify that components work correctly together, using real dependencies where appropriate.

#### Technologies
- **Testcontainers**: For spinning up real database instances
- **WebApplicationFactory**: For testing API endpoints
- **Microsoft.AspNetCore.Mvc.Testing**: API testing infrastructure

#### Running Integration Tests

```bash
# Run all integration tests
dotnet test --filter "FullyQualifiedName~IntegrationTests"

# Run integration tests for a specific service
dotnet test tests/Services/Employees/Employees.IntegrationTests
```

#### Example Integration Test Setup

```csharp
public class EmployeeApiTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;

    public EmployeeApiTests()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    // Replace database with test container
                    services.RemoveAll<DbContextOptions<EmployeeContext>>();
                    services.AddDbContext<EmployeeContext>(options =>
                        options.UseNpgsql(_dbContainer.GetConnectionString()));
                });
            });
        
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetEmployees_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/employees");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
```

### Contract Testing

Contract tests ensure that services can communicate correctly with each other.

#### Technologies
- **Pact.NET**: Consumer-driven contract testing

#### Running Contract Tests

```bash
dotnet test --filter "Category=ContractTest"
```

## Frontend Testing

### Unit Tests (Angular)

Frontend unit tests use Jasmine and Karma to test components, services, and utilities in isolation.

#### Running Frontend Unit Tests

```bash
cd src/WebApps/PubsWebApp
npm test

# With coverage
npm run test:coverage
```

#### Example Angular Unit Test

```typescript
describe('EmployeeListComponent', () => {
  let component: EmployeeListComponent;
  let fixture: ComponentFixture<EmployeeListComponent>;
  let employeeService: jasmine.SpyObj<EmployeeService>;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('EmployeeService', ['getEmployees']);
    
    TestBed.configureTestingModule({
      declarations: [EmployeeListComponent],
      providers: [
        { provide: EmployeeService, useValue: spy }
      ]
    });
    
    fixture = TestBed.createComponent(EmployeeListComponent);
    component = fixture.componentInstance;
    employeeService = TestBed.inject(EmployeeService) as jasmine.SpyObj<EmployeeService>;
  });

  it('should load employees on init', () => {
    const mockEmployees = [/* mock data */];
    employeeService.getEmployees.and.returnValue(of(mockEmployees));
    
    component.ngOnInit();
    
    expect(component.employees).toEqual(mockEmployees);
    expect(employeeService.getEmployees).toHaveBeenCalled();
  });
});
```

### End-to-End Tests

E2E tests verify complete user workflows across the entire application.

#### Technologies
- **Cypress**: Modern E2E testing framework
- **Playwright**: Alternative E2E testing framework

#### Running E2E Tests

```bash
cd src/WebApps/PubsWebApp

# Cypress
npm run e2e
npm run e2e:open  # Interactive mode

# Playwright
npm run test:e2e
```

#### Example Cypress Test

```typescript
describe('Employee Management', () => {
  beforeEach(() => {
    cy.visit('/employees');
    cy.login('admin@example.com', 'password123');
  });

  it('should create a new employee', () => {
    cy.get('[data-testid="add-employee-btn"]').click();
    cy.get('[data-testid="first-name-input"]').type('John');
    cy.get('[data-testid="last-name-input"]').type('Doe');
    cy.get('[data-testid="job-level-select"]').select('Developer');
    cy.get('[data-testid="save-btn"]').click();
    
    cy.contains('Employee created successfully').should('be.visible');
    cy.contains('John Doe').should('be.visible');
  });
});
```

### Accessibility Testing

Ensure the application meets WCAG 2.2 Level AA compliance.

#### Technologies
- **axe-core**: Automated accessibility testing
- **pa11y**: Accessibility testing tool

#### Running Accessibility Tests

```bash
cd src/WebApps/PubsWebApp
npm run test:a11y
```

#### Example Accessibility Test

```typescript
import { axe, toHaveNoViolations } from 'jest-axe';

expect.extend(toHaveNoViolations);

describe('Employee List Accessibility', () => {
  it('should have no accessibility violations', async () => {
    const { container } = render(<EmployeeList />);
    const results = await axe(container);
    expect(results).toHaveNoViolations();
  });
});
```

## Performance Testing

### Load Testing

Verify the application can handle expected load.

#### Technologies
- **k6**: Modern load testing tool
- **NBomber**: .NET load testing framework

#### Running Load Tests

```bash
# k6
k6 run tests/LoadTests/employee-api-load-test.js

# NBomber
dotnet run --project tests/LoadTests/EmployeeApi.LoadTests
```

#### Example k6 Test

```javascript
import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  stages: [
    { duration: '30s', target: 20 },  // Ramp up
    { duration: '1m', target: 100 },  // Stay at peak
    { duration: '30s', target: 0 },   // Ramp down
  ],
  thresholds: {
    http_req_duration: ['p(95)<200'], // 95% of requests under 200ms
  },
};

export default function () {
  const res = http.get('http://localhost:5000/api/employees');
  check(res, {
    'status is 200': (r) => r.status === 200,
    'response time < 200ms': (r) => r.timings.duration < 200,
  });
  sleep(1);
}
```

## Security Testing

### SAST (Static Application Security Testing)

Analyze source code for security vulnerabilities.

#### Tools
- **SonarQube**: Code quality and security analysis
- **Snyk**: Dependency vulnerability scanning

#### Running Security Scans

```bash
# SonarQube
dotnet sonarscanner begin /k:"pubs-microservices" /d:sonar.host.url="http://localhost:9000"
dotnet build
dotnet sonarscanner end

# Snyk
snyk test
snyk monitor
```

### DAST (Dynamic Application Security Testing)

Test running applications for security vulnerabilities.

#### Tools
- **OWASP ZAP**: Automated security scanning

#### Running DAST

```bash
# OWASP ZAP
zap-cli quick-scan --self-contained http://localhost:5000

# Or use Docker
docker run -v $(pwd):/zap/wrk/:rw -t owasp/zap2docker-stable zap-baseline.py \
  -t http://localhost:5000 -r testreport.html
```

## Chaos Engineering

Test system resilience by introducing failures.

#### Technologies
- **Chaos Toolkit**: Chaos engineering experiments
- **Simmy**: .NET chaos engineering library

#### Example Chaos Test

```yaml
# chaos-experiment.yaml
version: 1.0.0
title: Database connection failure
description: Verify system handles database failures gracefully

steady-state-hypothesis:
  title: System is healthy
  probes:
    - name: api-responds
      type: probe
      provider:
        type: http
        url: http://localhost:5000/health
        expect: 200

method:
  - type: action
    name: kill-database-connection
    provider:
      type: process
      path: docker
      arguments: ["stop", "postgres-container"]
    pauses:
      after: 10

rollbacks:
  - type: action
    name: restore-database
    provider:
      type: process
      path: docker
      arguments: ["start", "postgres-container"]
```

## CI/CD Integration

### GitHub Actions Workflow

```yaml
name: Test Pipeline

on: [push, pull_request]

jobs:
  unit-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'
      
      - name: Run Unit Tests
        run: |
          dotnet test --filter "FullyQualifiedName~UnitTests" \
            /p:CollectCoverage=true \
            /p:CoverletOutputFormat=opencover \
            /p:Threshold=85

      - name: Upload Coverage
        uses: codecov/codecov-action@v3
        with:
          files: ./coverage.opencover.xml

  integration-tests:
    runs-on: ubuntu-latest
    services:
      postgres:
        image: postgres:15-alpine
        env:
          POSTGRES_PASSWORD: postgres
        options: >-
          --health-cmd pg_isready
          --health-interval 10s
          --health-timeout 5s
          --health-retries 5
    
    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-dotnet@v3
      
      - name: Run Integration Tests
        run: dotnet test --filter "FullyQualifiedName~IntegrationTests"

  e2e-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: actions/setup-node@v3
      
      - name: Install Dependencies
        run: |
          cd src/WebApps/PubsWebApp
          npm ci
      
      - name: Run E2E Tests
        run: |
          cd src/WebApps/PubsWebApp
          npm run e2e:ci

  security-scan:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Run Snyk Security Scan
        uses: snyk/actions/dotnet@master
        env:
          SNYK_TOKEN: ${{ secrets.SNYK_TOKEN }}
```

## Quality Gates

### Code Coverage Thresholds

- Domain Layer: **≥ 90%**
- Application Layer: **≥ 85%**
- Infrastructure Layer: **≥ 70%**
- API Layer: **≥ 80%**
- Overall Project: **≥ 85%**

### Performance Thresholds

- API Response Time: **< 200ms** for 95th percentile
- Database Query Time: **< 100ms** for 95th percentile
- Frontend Bundle Size: **< 500KB** gzipped
- Lighthouse Performance Score: **≥ 90**

### Security Requirements

- **Zero critical vulnerabilities**
- **Zero high-severity vulnerabilities**
- Must pass OWASP Top 10 checks

### Accessibility Requirements

- **WCAG 2.2 Level AA compliance**
- Lighthouse Accessibility Score: **≥ 95**
- Zero critical axe-core violations

## Best Practices

### Test Writing

1. **Follow AAA Pattern**: Arrange, Act, Assert
2. **One Assertion Per Test**: Each test should verify one thing
3. **Descriptive Test Names**: Use `MethodName_Condition_ExpectedResult` pattern
4. **Test Edge Cases**: Don't just test the happy path
5. **Keep Tests Fast**: Unit tests should complete in milliseconds
6. **Avoid Test Interdependence**: Tests should be able to run in any order
7. **Use Test Fixtures**: Share setup code across related tests

### Mocking Guidelines

1. **Mock External Dependencies**: Databases, APIs, file systems
2. **Don't Mock What You Don't Own**: Test against real implementations when possible
3. **Verify Interactions**: Use mocks to verify behavior, not just state
4. **Keep Mocks Simple**: Complex mocks indicate design problems

### Code Coverage

1. **Aim for High Coverage**: But don't sacrifice test quality for coverage percentage
2. **Focus on Critical Paths**: Ensure high coverage for business-critical code
3. **Ignore Generated Code**: Exclude auto-generated code from coverage metrics
4. **Test Behavior, Not Implementation**: Coverage is a tool, not the goal

## Troubleshooting

### Common Issues

#### Tests Fail on CI but Pass Locally

- Check for timezone differences
- Verify database state is consistent
- Check for file path dependencies
- Review environment variable configuration

#### Flaky Tests

- Identify timing issues (use proper waits, not sleeps)
- Check for shared state between tests
- Review test data setup and teardown
- Use test retry strategies for genuine flakiness

#### Low Code Coverage

- Identify uncovered code paths
- Add tests for edge cases
- Review exception handling paths
- Consider if code is actually needed

## Resources

### Documentation

- [xUnit Documentation](https://xunit.net/)
- [Moq Quickstart](https://github.com/moq/moq4/wiki/Quickstart)
- [FluentAssertions Documentation](https://fluentassertions.com/)
- [Testcontainers Documentation](https://dotnet.testcontainers.org/)
- [Cypress Best Practices](https://docs.cypress.io/guides/references/best-practices)
- [k6 Documentation](https://k6.io/docs/)

### Training

- [Test-Driven Development Course](https://www.pluralsight.com/courses/play-by-play-tdd)
- [Integration Testing in .NET](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests)
- [E2E Testing with Cypress](https://learn.cypress.io/)

## Maintenance

### Regular Tasks

- **Weekly**: Review test coverage trends
- **Monthly**: Update testing dependencies
- **Quarterly**: Review and update testing strategy
- **As Needed**: Add tests for bug fixes and new features

### Metrics to Track

- Test execution time
- Code coverage percentage
- Number of failing tests
- Test flakiness rate
- Time to fix failing tests

---

**Last Updated**: October 2025
**Maintainers**: Development Team
**Questions?**: Contact the QA team or open an issue
