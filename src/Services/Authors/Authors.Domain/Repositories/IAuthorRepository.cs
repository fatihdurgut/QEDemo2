using Common.Domain;
using Authors.Domain.Aggregates;

namespace Authors.Domain.Repositories;

/// <summary>
/// Repository interface for Author aggregate
/// </summary>
public interface IAuthorRepository : IRepository<Author>
{
    /// <summary>
    /// Gets an author by ID
    /// </summary>
    Task<Author?> GetByIdAsync(string authorId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all authors with pagination
    /// </summary>
    Task<IEnumerable<Author>> GetAllAsync(int skip = 0, int take = 100, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new author
    /// </summary>
    Author Add(Author author);

    /// <summary>
    /// Updates an existing author
    /// </summary>
    void Update(Author author);

    /// <summary>
    /// Deletes an author
    /// </summary>
    void Delete(Author author);

    /// <summary>
    /// Checks if an author exists
    /// </summary>
    Task<bool> ExistsAsync(string authorId, CancellationToken cancellationToken = default);
}
