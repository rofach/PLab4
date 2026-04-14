using RestaurantSystem.Application.DTOs;

namespace RestaurantSystem.Application.Interfaces.Services;

public interface IMenuService
{
    Task<List<MenuItemResponseDto>> GetMenuAsync(CancellationToken cancellationToken = default);
    Task<MenuItemResponseDto> AddMenuItemAsync(MenuItemCreateRequestDto request, CancellationToken cancellationToken = default);
    Task DeleteMenuItemAsync(int id, CancellationToken cancellationToken = default);
}
