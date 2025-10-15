namespace Identity.Application.Services;

/// <summary>
/// Service for generating JWT tokens
/// </summary>
public interface ITokenService
{
    string GenerateToken(Guid userId, string userName, string email, string role);
    bool ValidateToken(string token);
}
