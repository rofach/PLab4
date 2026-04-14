using RestaurantSystem.Domain.Entities;

namespace RestaurantSystem.Application.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default);
    Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
