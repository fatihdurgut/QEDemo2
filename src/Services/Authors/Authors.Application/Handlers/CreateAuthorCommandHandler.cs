using MediatR;
using Common.Application;
using Authors.Application.Commands;
using Authors.Domain.Aggregates;
using Authors.Domain.Repositories;

namespace Authors.Application.Handlers;

/// <summary>
/// Handler for CreateAuthorCommand
/// </summary>
public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, CommandResult<string>>
{
    private readonly IAuthorRepository _authorRepository;

    public CreateAuthorCommandHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<CommandResult<string>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (await _authorRepository.ExistsAsync(request.Id, cancellationToken))
            {
                return CommandResult<string>.Failed($"Author with ID {request.Id} already exists");
            }

            var author = Author.Create(
                request.Id,
                request.FirstName,
                request.LastName,
                request.Phone,
                request.Street,
                request.City,
                request.State,
                request.ZipCode,
                request.HasContract);

            _authorRepository.Add(author);
            await _authorRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return CommandResult<string>.Successful(author.Id);
        }
        catch (Exception ex)
        {
            return CommandResult<string>.Failed(ex.Message);
        }
    }
}
