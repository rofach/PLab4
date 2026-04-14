using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Application.Interfaces.Repositories;
using RestaurantSystem.Domain.Entities;
using RestaurantSystem.Infrastructure.Persistence;

namespace RestaurantSystem.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RestaurantDbContext _dbContext;

    public UserRepository(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _dbContext.Users.AnyAsync(user => user.Email == email, cancellationToken);
    }
}
