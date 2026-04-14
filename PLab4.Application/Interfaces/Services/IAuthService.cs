using RestaurantSystem.Application.DTOs;

namespace RestaurantSystem.Application.Interfaces.Services;

public interface IAuthService
{
    Task<UserResponseDto> RegisterAsync(RegisterUserRequestDto request, CancellationToken cancellationToken = default);
}
