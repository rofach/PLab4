using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Application.Interfaces.Repositories;
using RestaurantSystem.Domain.Entities;
using RestaurantSystem.Infrastructure.Persistence;

namespace RestaurantSystem.Infrastructure.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly RestaurantDbContext _dbContext;

    public MenuRepository(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MenuItem> AddAsync(MenuItem menuItem, CancellationToken cancellationToken = default)
    {
        _dbContext.MenuItems.Add(menuItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return menuItem;
    }

    public Task<List<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.MenuItems
            .AsNoTracking()
            .OrderBy(menuItem => menuItem.Id)
            .ToListAsync(cancellationToken);
    }

    public Task<List<MenuItem>> GetAvailableByIdsAsync(IEnumerable<int> itemIds, CancellationToken cancellationToken = default)
    {
        return _dbContext.MenuItems
            .Where(menuItem => itemIds.Contains(menuItem.Id) && menuItem.IsAvailable)
            .ToListAsync(cancellationToken);
    }

    public Task<MenuItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _dbContext.MenuItems.FirstOrDefaultAsync(menuItem => menuItem.Id == id, cancellationToken);
    }

    public async Task DeleteAsync(MenuItem menuItem, CancellationToken cancellationToken = default)
    {
        _dbContext.MenuItems.Remove(menuItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
