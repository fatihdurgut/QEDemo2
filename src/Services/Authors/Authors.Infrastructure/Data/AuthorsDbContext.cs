using Microsoft.EntityFrameworkCore;
using MediatR;
using Common.Domain;
using Authors.Domain.Aggregates;

namespace Authors.Infrastructure.Data;

/// <summary>
/// Authors database context
/// </summary>
public class AuthorsDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public DbSet<Author> Authors { get; set; } = null!;

    public AuthorsDbContext(DbContextOptions<AuthorsDbContext> options, IMediator mediator) 
        : base(options)
    {
        _mediator = mediator;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthorsDbContext).Assembly);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(cancellationToken);
        var result = await SaveChangesAsync(cancellationToken);
        return result > 0;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEntities = ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}
