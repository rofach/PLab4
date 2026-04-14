using RestaurantSystem.Application.DTOs;
using RestaurantSystem.Application.Interfaces.Repositories;
using RestaurantSystem.Application.Interfaces.Services;
using RestaurantSystem.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace RestaurantSystem.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserResponseDto> RegisterAsync(RegisterUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        _logger.LogInformation("Registering a new user with email {Email}.", normalizedEmail);

        if (await _userRepository.ExistsByEmailAsync(normalizedEmail, cancellationToken))
        {
            _logger.LogWarning("Registration failed because user with email {Email} already exists.", normalizedEmail);
            throw new InvalidOperationException("A user with this email already exists.");
        }

        var user = new User
        {
            FullName = request.FullName.Trim(),
            Email = normalizedEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        var createdUser = await _userRepository.AddAsync(user, cancellationToken);

        _logger.LogInformation("User with email {Email} was registered successfully with id {UserId}.", createdUser.Email, createdUser.Id);

        return new UserResponseDto
        {
            Id = createdUser.Id,
            FullName = createdUser.FullName,
            Email = createdUser.Email,
            CreatedAtUtc = createdUser.CreatedAtUtc
        };
    }
}
