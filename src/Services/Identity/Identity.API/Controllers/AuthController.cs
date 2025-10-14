using Identity.Application.DTOs;
using Identity.Application.Services;
using Identity.Domain.Aggregates;
using Identity.Domain.Repositories;
using Identity.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers;

/// <summary>
/// Authentication controller for login and registration
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordService _passwordService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IUserRepository userRepository,
        ITokenService tokenService,
        IPasswordService passwordService,
        ILogger<AuthController> logger)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordService = passwordService;
        _logger = logger;
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("Login attempt failed for email: {Email}", request.Email);
                return Unauthorized(new { message = "Invalid credentials" });
            }

            if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Invalid password for email: {Email}", request.Email);
                return Unauthorized(new { message = "Invalid credentials" });
            }

            user.RecordLogin();
            await _userRepository.UpdateAsync(user);

            var token = _tokenService.GenerateToken(
                user.Id,
                user.UserName,
                user.Email,
                user.Role.ToString());

            _logger.LogInformation("User logged in successfully: {Email}", request.Email);

            return Ok(new LoginResponse(token, user.UserName, user.Email, user.Role.ToString()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", request.Email);
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            // Check if user already exists
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "User with this email already exists" });
            }

            existingUser = await _userRepository.GetByUserNameAsync(request.UserName);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Username already taken" });
            }

            // Parse role
            if (!Enum.TryParse<UserRole>(request.Role, out var role))
            {
                return BadRequest(new { message = "Invalid role specified" });
            }

            // Create user
            var passwordHash = _passwordService.HashPassword(request.Password);
            var userName = new UserName(request.FirstName, request.LastName);
            var user = new User(request.Email, request.UserName, passwordHash, userName, role);

            await _userRepository.AddAsync(user);

            _logger.LogInformation("User registered successfully: {Email}", request.Email);

            var userDto = new UserDto(
                user.Id,
                user.Email,
                user.UserName,
                user.Name.FirstName,
                user.Name.LastName,
                user.Role.ToString(),
                user.IsActive);

            return CreatedAtAction(nameof(Register), new { id = user.Id }, userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email: {Email}", request.Email);
            return StatusCode(500, new { message = "An error occurred during registration" });
        }
    }
}
