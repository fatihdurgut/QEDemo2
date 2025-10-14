using Common.Application;
using Authors.Application.DTOs;

namespace Authors.Application.Queries;

/// <summary>
/// Query to get an author by ID
/// </summary>
public record GetAuthorByIdQuery(string AuthorId) : IQuery<AuthorDto?>;

/// <summary>
/// Query to get all authors with pagination
/// </summary>
public record GetAllAuthorsQuery(int Skip = 0, int Take = 100) : IQuery<IEnumerable<AuthorDto>>;
