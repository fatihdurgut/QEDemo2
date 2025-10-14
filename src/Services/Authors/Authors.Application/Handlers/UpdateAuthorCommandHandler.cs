using MediatR;
using Common.Application;
using Authors.Application.Commands;
using Authors.Domain.Repositories;

namespace Authors.Application.Handlers;

/// <summary>
/// Handler for UpdateAuthorCommand
/// </summary>
public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, CommandResult>
{
    private readonly IAuthorRepository _authorRepository;

    public UpdateAuthorCommandHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<CommandResult> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var author = await _authorRepository.GetByIdAsync(request.Id, cancellationToken);
            if (author == null)
            {
                return CommandResult.Failed($"Author with ID {request.Id} not found");
            }

            author.Update(
                request.FirstName,
                request.LastName,
                request.Phone,
                request.Street,
                request.City,
                request.State,
                request.ZipCode,
                request.HasContract);

            _authorRepository.Update(author);
            await _authorRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            return CommandResult.Successful();
        }
        catch (Exception ex)
        {
            return CommandResult.Failed(ex.Message);
        }
    }
}
