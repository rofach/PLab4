using Microsoft.AspNetCore.Mvc;
using RestaurantSystem.Application.DTOs;
using RestaurantSystem.Application.Interfaces.Services;

namespace RestaurantSystem.Api.Controllers;

[ApiController]
[Route("api/menu")]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet]
    public async Task<ActionResult<List<MenuItemResponseDto>>> GetMenu(CancellationToken cancellationToken)
    {
        var menuItems = await _menuService.GetMenuAsync(cancellationToken);
        return Ok(menuItems);
    }

    [HttpPost]
    public async Task<ActionResult<MenuItemResponseDto>> AddMenuItem(
        [FromBody] MenuItemCreateRequestDto request,
        CancellationToken cancellationToken)
    {
        var createdMenuItem = await _menuService.AddMenuItemAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetMenu), new { id = createdMenuItem.Id }, createdMenuItem);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMenuItem(int id, CancellationToken cancellationToken)
    {
        await _menuService.DeleteMenuItemAsync(id, cancellationToken);
        return NoContent();
    }
}
