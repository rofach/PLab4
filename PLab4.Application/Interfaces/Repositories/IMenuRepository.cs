using RestaurantSystem.Domain.Entities;

namespace RestaurantSystem.Application.Interfaces.Repositories;

public interface IMenuRepository
{
    Task<List<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<MenuItem>> GetAvailableByIdsAsync(IEnumerable<int> itemIds, CancellationToken cancellationToken = default);
    Task<MenuItem> AddAsync(MenuItem menuItem, CancellationToken cancellationToken = default);
    Task<MenuItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task DeleteAsync(MenuItem menuItem, CancellationToken cancellationToken = default);
}
