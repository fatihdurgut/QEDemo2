using Microsoft.EntityFrameworkCore;
using Common.Domain;
using Authors.Domain.Aggregates;
using Authors.Domain.Repositories;
using Authors.Infrastructure.Data;

namespace Authors.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for Author aggregate
/// </summary>
public class AuthorRepository : IAuthorRepository
{
    private readonly AuthorsDbContext _context;

    public IUnitOfWork UnitOfWork => _context;

    public AuthorRepository(AuthorsDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Author?> GetByIdAsync(string authorId, CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .FirstOrDefaultAsync(a => a.Id == authorId, cancellationToken);
    }

    public async Task<IEnumerable<Author>> GetAllAsync(int skip = 0, int take = 100, CancellationToken cancellationToken = default)
    {
        return await _context.Authors
            .OrderBy(a => a.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public Author Add(Author author)
    {
        return _context.Authors.Add(author).Entity;
    }

    public void Update(Author author)
    {
        _context.Entry(author).State = EntityState.Modified;
    }

    public void Delete(Author author)
    {
        _context.Authors.Remove(author);
    }

    public async Task<bool> ExistsAsync(string authorId, CancellationToken cancellationToken = default)
    {
        return await _context.Authors.AnyAsync(a => a.Id == authorId, cancellationToken);
    }
}
