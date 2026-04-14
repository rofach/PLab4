using RestaurantSystem.Application.DTOs;

namespace RestaurantSystem.Application.Interfaces.Services;

public interface IOrderService
{
    Task<OrderResponseDto> CreateOrderAsync(CreateOrderRequestDto request, CancellationToken cancellationToken = default);
    Task<OrderResponseDto> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default);
}
