using Microsoft.AspNetCore.Mvc;
using MediatR;
using Authors.Application.Commands;
using Authors.Application.Queries;
using Authors.Application.DTOs;

namespace Authors.API.Controllers;

/// <summary>
/// Authors API controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthorsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthorsController> _logger;

    public AuthorsController(IMediator mediator, ILogger<AuthorsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all authors with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AuthorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAllAuthors(
        [FromQuery] int skip = 0,
        [FromQuery] int take = 100,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetAllAuthorsQuery(skip, take);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving authors");
            return StatusCode(500, "An error occurred while retrieving authors");
        }
    }

    /// <summary>
    /// Get author by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthorDto>> GetAuthorById(
        string id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetAuthorByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            
            if (result == null)
                return NotFound($"Author with ID {id} not found");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving author {AuthorId}", id);
            return StatusCode(500, "An error occurred while retrieving the author");
        }
    }

    /// <summary>
    /// Create a new author
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> CreateAuthor(
        [FromBody] CreateAuthorCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _mediator.Send(command, cancellationToken);
            
            if (!result.Success)
                return BadRequest(result.ErrorMessage);

            return CreatedAtAction(nameof(GetAuthorById), new { id = result.Data }, result.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating author");
            return StatusCode(500, "An error occurred while creating the author");
        }
    }

    /// <summary>
    /// Update an existing author
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAuthor(
        string id,
        [FromBody] UpdateAuthorCommand command,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (id != command.Id)
                return BadRequest("ID in URL does not match ID in request body");

            var result = await _mediator.Send(command, cancellationToken);
            
            if (!result.Success)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                    return NotFound(result.ErrorMessage);
                    
                return BadRequest(result.ErrorMessage);
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating author {AuthorId}", id);
            return StatusCode(500, "An error occurred while updating the author");
        }
    }
}
