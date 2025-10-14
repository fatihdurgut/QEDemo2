using MediatR;
using AutoMapper;
using Authors.Application.Queries;
using Authors.Application.DTOs;
using Authors.Domain.Repositories;

namespace Authors.Application.Handlers;

/// <summary>
/// Handler for GetAuthorByIdQuery
/// </summary>
public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDto?>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public GetAuthorByIdQueryHandler(IAuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<AuthorDto?> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetByIdAsync(request.AuthorId, cancellationToken);
        return author == null ? null : _mapper.Map<AuthorDto>(author);
    }
}

/// <summary>
/// Handler for GetAllAuthorsQuery
/// </summary>
public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, IEnumerable<AuthorDto>>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    public GetAllAuthorsQueryHandler(IAuthorRepository authorRepository, IMapper mapper)
    {
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AuthorDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
    {
        var authors = await _authorRepository.GetAllAsync(request.Skip, request.Take, cancellationToken);
        return _mapper.Map<IEnumerable<AuthorDto>>(authors);
    }
}
