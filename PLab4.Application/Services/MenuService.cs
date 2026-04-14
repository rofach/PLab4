using RestaurantSystem.Application.DTOs;
using RestaurantSystem.Application.Interfaces.Repositories;
using RestaurantSystem.Application.Interfaces.Services;
using RestaurantSystem.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace RestaurantSystem.Application.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;
    private readonly ILogger<MenuService> _logger;

    public MenuService(IMenuRepository menuRepository, ILogger<MenuService> logger)
    {
        _menuRepository = menuRepository;
        _logger = logger;
    }

    public async Task<List<MenuItemResponseDto>> GetMenuAsync(CancellationToken cancellationToken = default)
    {
        var menuItems = await _menuRepository.GetAllAsync(cancellationToken);
        _logger.LogInformation("Retrieved {Count} menu items.", menuItems.Count);
        return menuItems.Select(Map).ToList();
    }

    public async Task<MenuItemResponseDto> AddMenuItemAsync(MenuItemCreateRequestDto request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Adding a new menu item named {Name}.", request.Name);

        var menuItem = new MenuItem
        {
            Name = request.Name.Trim(),
            Price = request.Price,
            IsAvailable = request.IsAvailable
        };

        var createdMenuItem = await _menuRepository.AddAsync(menuItem, cancellationToken);
        _logger.LogInformation("Menu item {Name} was created with id {MenuItemId}.", createdMenuItem.Name, createdMenuItem.Id);
        return Map(createdMenuItem);
    }

    public async Task DeleteMenuItemAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting menu item {MenuItemId}.", id);

        var menuItem = await _menuRepository.GetByIdAsync(id, cancellationToken);
        if (menuItem is null)
        {
            _logger.LogWarning("Menu item {MenuItemId} was not found for deletion.", id);
            throw new KeyNotFoundException("Menu item not found");
        }

        await _menuRepository.DeleteAsync(menuItem, cancellationToken);
        _logger.LogInformation("Menu item {MenuItemId} was deleted successfully.", id);
    }

    private static MenuItemResponseDto Map(MenuItem menuItem)
    {
        return new MenuItemResponseDto
        {
            Id = menuItem.Id,
            Name = menuItem.Name,
            Price = menuItem.Price,
            IsAvailable = menuItem.IsAvailable
        };
    }
}
