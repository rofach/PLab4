using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Application.Interfaces.Repositories;
using RestaurantSystem.Domain.Entities;
using RestaurantSystem.Infrastructure.Persistence;

namespace RestaurantSystem.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly RestaurantDbContext _dbContext;

    public OrderRepository(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await _dbContext.Orders
            .Include(savedOrder => savedOrder.OrderItems)
            .ThenInclude(orderItem => orderItem.MenuItem)
            .FirstAsync(savedOrder => savedOrder.Id == order.Id, cancellationToken);
    }

    public Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Orders
            .AsNoTracking()
            .Include(order => order.OrderItems)
            .ThenInclude(orderItem => orderItem.MenuItem)
            .FirstOrDefaultAsync(order => order.Id == id, cancellationToken);
    }
}
