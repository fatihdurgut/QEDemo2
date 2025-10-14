# Phase 2 Implementation Guide: Core Domain Services

## Overview
This document provides a comprehensive guide for implementing the remaining microservices (Publishers, Titles, Sales, Employees, Stores) following the DDD, CQRS, and Event Sourcing patterns established in the Authors Service.

## Authors Service - Reference Implementation

The Authors Service has been fully implemented as a reference template demonstrating:

### ✅ Complete Implementation
- **Domain Layer**: Aggregates, Value Objects, Domain Events, Repository Interfaces
- **Application Layer**: Commands, Queries, Handlers, DTOs, Validators
- **Infrastructure Layer**: DbContext, Repositories, Entity Configurations
- **API Layer**: Controllers, Dependency Injection, Swagger Documentation

## Architecture Patterns

### 1. Domain-Driven Design (DDD)

#### Aggregates
An aggregate is a cluster of domain objects that can be treated as a single unit. The aggregate root is the entry point.

**Example: Author Aggregate**
```csharp
public class Author : Entity, IAggregateRoot
{
    public string Id { get; private set; }
    public AuthorName Name { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public Address? Address { get; private set; }
    public bool HasContract { get; private set; }
    
    // Factory method
    public static Author Create(...) { }
    
    // Business methods
    public void Update(...) { }
    public void SignContract() { }
    public void TerminateContract() { }
}
```

**Key Principles:**
- Aggregates enforce invariants and business rules
- All modifications go through the aggregate root
- Use factory methods for creation
- Raise domain events for significant state changes

#### Value Objects
Value objects represent concepts that are defined by their attributes rather than identity.

**Example: AuthorName Value Object**
```csharp
public class AuthorName : ValueObject
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    
    public static AuthorName Create(string firstName, string lastName)
    {
        // Validation logic
        return new AuthorName(firstName, lastName);
    }
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}
```

**Key Principles:**
- Immutable after creation
- Equality based on values, not identity
- Encapsulate validation logic
- Use private constructors with public factory methods

#### Domain Events
Domain events capture business-significant occurrences.

**Example: AuthorCreatedDomainEvent**
```csharp
public class AuthorCreatedDomainEvent : IDomainEvent
{
    public Guid EventId { get; }
    public DateTime OccurredOn { get; }
    public string AuthorId { get; }
    // Additional properties...
}
```

**Key Principles:**
- Named in past tense (AuthorCreated, not CreateAuthor)
- Contain only data needed by subscribers
- Immutable (use records or readonly properties)
- Raised within aggregate methods

### 2. CQRS Pattern

#### Commands (Write Operations)
Commands represent intentions to change state.

**Example: CreateAuthorCommand**
```csharp
public record CreateAuthorCommand : ICommand<CommandResult<string>>
{
    public string Id { get; init; }
    public string FirstName { get; init; }
    // Additional properties...
}

public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
{
    public CreateAuthorCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().MaximumLength(11);
        // Additional validation rules...
    }
}

public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, CommandResult<string>>
{
    private readonly IAuthorRepository _repository;
    
    public async Task<CommandResult<string>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        // Validation, business logic, persistence
    }
}
```

#### Queries (Read Operations)
Queries retrieve data without side effects.

**Example: GetAuthorByIdQuery**
```csharp
public record GetAuthorByIdQuery(string AuthorId) : IQuery<AuthorDto?>;

public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDto?>
{
    private readonly IAuthorRepository _repository;
    private readonly IMapper _mapper;
    
    public async Task<AuthorDto?> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await _repository.GetByIdAsync(request.AuthorId, cancellationToken);
        return _mapper.Map<AuthorDto>(author);
    }
}
```

### 3. Repository Pattern

**Interface in Domain Layer:**
```csharp
public interface IAuthorRepository : IRepository<Author>
{
    Task<Author?> GetByIdAsync(string authorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Author>> GetAllAsync(int skip = 0, int take = 100, CancellationToken cancellationToken = default);
    Author Add(Author author);
    void Update(Author author);
    void Delete(Author author);
    Task<bool> ExistsAsync(string authorId, CancellationToken cancellationToken = default);
}
```

**Implementation in Infrastructure Layer:**
```csharp
public class AuthorRepository : IAuthorRepository
{
    private readonly AuthorsDbContext _context;
    
    public IUnitOfWork UnitOfWork => _context;
    
    // Implementation methods...
}
```

### 4. Entity Framework Configuration

**Value Objects as Owned Types:**
```csharp
public class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("authors");
        builder.HasKey(a => a.Id);
        
        // Owned type for value objects
        builder.OwnsOne(a => a.Name, name =>
        {
            name.Property(n => n.FirstName).HasColumnName("au_fname");
            name.Property(n => n.LastName).HasColumnName("au_lname");
        });
        
        // Ignore domain events collection
        builder.Ignore(a => a.DomainEvents);
    }
}
```

### 5. API Layer

**Controller Implementation:**
```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IMediator _mediator;
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAllAuthors(
        [FromQuery] int skip = 0, [FromQuery] int take = 100)
    {
        var query = new GetAllAuthorsQuery(skip, take);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<string>> CreateAuthor([FromBody] CreateAuthorCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Success) return BadRequest(result.ErrorMessage);
        return CreatedAtAction(nameof(GetAuthorById), new { id = result.Data }, result.Data);
    }
}
```

## Implementation Checklist for Each Service

### Domain Layer
- [ ] Create aggregate root entity
- [ ] Implement value objects
- [ ] Define domain events
- [ ] Create repository interface
- [ ] Implement domain exceptions if needed
- [ ] Add business rules and invariants

### Application Layer
- [ ] Define DTOs
- [ ] Create commands with validators
- [ ] Create queries
- [ ] Implement command handlers
- [ ] Implement query handlers
- [ ] Create AutoMapper profile

### Infrastructure Layer
- [ ] Create DbContext
- [ ] Implement entity configurations
- [ ] Implement repository
- [ ] Configure domain event dispatching

### API Layer
- [ ] Create controller with endpoints
- [ ] Configure dependency injection
- [ ] Add health checks
- [ ] Configure Swagger documentation
- [ ] Update appsettings.json

## Service-Specific Domain Models

### Publishers Service (TASK-012)

**Aggregates:**
- `Publisher`: pub_id, pub_name, city, state, country
- `PubInfo`: pub_id (FK), logo, pr_info

**Value Objects:**
- `PublisherName`
- `Location` (city, state, country)

**Domain Events:**
- `PublisherCreatedDomainEvent`
- `PublisherUpdatedDomainEvent`
- `PublisherInfoUpdatedDomainEvent`

**Business Rules:**
- Publisher must have a unique ID
- Country defaults to "USA"
- PubInfo is optional additional information

### Titles Service (TASK-013)

**Aggregates:**
- `Title`: title_id, title, type, pub_id, price, advance, royalty, ytd_sales, notes, pubdate
- `TitleAuthor`: Relationship entity (title_id, au_id, au_ord, royaltyper)
- `RoyaltySchedule`: title_id, lorange, hirange, royalty

**Value Objects:**
- `Money` (for price and advance)
- `TitleType` (enumeration)
- `RoyaltyRange`

**Domain Events:**
- `TitleCreatedDomainEvent`
- `TitleUpdatedDomainEvent`
- `TitleAuthorAddedDomainEvent`
- `RoyaltyScheduleUpdatedDomainEvent`

**Business Rules:**
- Title must have at least one author
- Royalty schedules must not overlap
- Price and advance must be positive
- YTD sales tracking

### Sales Service (TASK-014)

**Aggregates:**
- `Sale`: stor_id, ord_num, ord_date, qty, payterms, title_id
- `Discount`: discounttype, stor_id, lowqty, highqty, discount

**Value Objects:**
- `OrderNumber`
- `Quantity`
- `PaymentTerms`
- `DiscountPercentage`

**Domain Events:**
- `SaleCreatedDomainEvent`
- `SaleUpdatedDomainEvent`
- `DiscountAppliedDomainEvent`

**Business Rules:**
- Quantity must be positive
- Discount ranges must be valid
- Payment terms validation
- Composite key handling

### Employees Service (TASK-015)

**Aggregates:**
- `Employee`: emp_id, fname, minit, lname, job_id, job_lvl, pub_id, hire_date
- `Job`: job_id, job_desc, min_lvl, max_lvl

**Value Objects:**
- `EmployeeName`
- `JobLevel`
- `HireDate`

**Domain Events:**
- `EmployeeHiredDomainEvent`
- `EmployeePromotedDomainEvent`
- `JobCreatedDomainEvent`

**Business Rules:**
- Job level must be within min/max range
- Employee must be assigned to a publisher
- Hire date cannot be in the future
- Job level progression validation

### Stores Service (TASK-016)

**Aggregates:**
- `Store`: stor_id, stor_name, stor_address, city, state, zip

**Value Objects:**
- `StoreName`
- `StoreAddress`
- `ZipCode`

**Domain Events:**
- `StoreCreatedDomainEvent`
- `StoreUpdatedDomainEvent`

**Business Rules:**
- Store ID must be unique
- Store name is required
- Address validation

## Testing Strategy

### Unit Tests
Test domain logic in isolation:

```csharp
public class AuthorTests
{
    [Fact]
    public void Create_ValidData_Success()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        
        // Act
        var author = Author.Create("123456789", firstName, lastName, "555-1234", 
            null, null, null, null, true);
        
        // Assert
        Assert.NotNull(author);
        Assert.Equal(firstName, author.Name.FirstName);
        Assert.True(author.HasContract);
    }
    
    [Fact]
    public void SignContract_WhenAlreadyHasContract_ThrowsException()
    {
        // Arrange
        var author = Author.Create("123456789", "John", "Doe", "555-1234", 
            null, null, null, null, true);
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => author.SignContract());
    }
}
```

### Integration Tests
Test repository and database interactions:

```csharp
public class AuthorRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly AuthorsDbContext _context;
    
    [Fact]
    public async Task Add_ValidAuthor_Success()
    {
        // Arrange
        var repository = new AuthorRepository(_context);
        var author = Author.Create(/* parameters */);
        
        // Act
        repository.Add(author);
        await repository.UnitOfWork.SaveEntitiesAsync();
        
        // Assert
        var retrieved = await repository.GetByIdAsync(author.Id);
        Assert.NotNull(retrieved);
    }
}
```

### API Tests
Test controller endpoints:

```csharp
public class AuthorsControllerTests
{
    [Fact]
    public async Task GetAllAuthors_ReturnsOkResult()
    {
        // Arrange
        var mediator = Mock.Of<IMediator>();
        var logger = Mock.Of<ILogger<AuthorsController>>();
        var controller = new AuthorsController(mediator, logger);
        
        // Act
        var result = await controller.GetAllAuthors();
        
        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }
}
```

## Database Migration Commands

```bash
# Add migration
dotnet ef migrations add InitialCreate --project src/Services/Authors/Authors.Infrastructure --startup-project src/Services/Authors/Authors.API

# Update database
dotnet ef database update --project src/Services/Authors/Authors.Infrastructure --startup-project src/Services/Authors/Authors.API

# Remove last migration
dotnet ef migrations remove --project src/Services/Authors/Authors.Infrastructure --startup-project src/Services/Authors/Authors.API
```

## Running the Service

```bash
# Run the Authors API
cd src/Services/Authors/Authors.API
dotnet run

# Access Swagger UI
http://localhost:5001

# Health check
http://localhost:5001/health
```

## Key Takeaways

1. **Domain Layer**: Pure business logic, no dependencies on infrastructure
2. **Application Layer**: Orchestrates use cases, no business logic
3. **Infrastructure Layer**: Implements technical concerns (persistence, messaging)
4. **API Layer**: Exposes functionality, handles HTTP concerns

5. **Aggregates**: Consistency boundaries, enforce invariants
6. **Value Objects**: Immutable, equality by value
7. **Domain Events**: Communicate state changes
8. **Repository**: Abstract persistence, aggregate-oriented

9. **CQRS**: Separate read and write models
10. **MediatR**: Decouples command/query senders from handlers
11. **FluentValidation**: Declarative validation rules
12. **AutoMapper**: Simplify DTO mapping

## Next Steps

1. Implement remaining services following this pattern
2. Add comprehensive unit tests (target: 85% coverage)
3. Implement integration events for cross-service communication
4. Add distributed tracing and logging
5. Implement API Gateway routing
6. Add authentication and authorization

## References

- [Domain-Driven Design by Eric Evans](https://www.domainlanguage.com/ddd/)
- [Implementing Domain-Driven Design by Vaughn Vernon](https://vaughnvernon.com/?page_id=168)
- [CQRS Pattern](https://martinfowler.com/bliki/CQRS.html)
- [Event Sourcing](https://martinfowler.com/eaaDev/EventSourcing.html)
- [.NET Microservices Architecture](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/)
