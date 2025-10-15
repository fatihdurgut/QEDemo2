namespace Identity.Application.DTOs;

public record LoginRequest(string Email, string Password);

public record LoginResponse(string Token, string UserName, string Email, string Role);

public record RegisterRequest(
    string Email, 
    string UserName, 
    string Password,
    string FirstName,
    string LastName,
    string Role);

public record UserDto(
    Guid Id,
    string Email,
    string UserName,
    string FirstName,
    string LastName,
    string Role,
    bool IsActive);
