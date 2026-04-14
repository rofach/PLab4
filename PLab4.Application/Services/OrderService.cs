using RestaurantSystem.Application.DTOs;
using RestaurantSystem.Application.Interfaces.Repositories;
using RestaurantSystem.Application.Interfaces.Services;
using RestaurantSystem.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace RestaurantSystem.Application.Services;

public class OrderService : IOrderService
{
    private readonly IMenuRepository _menuRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IMenuRepository menuRepository, IOrderRepository orderRepository, ILogger<OrderService> logger)
    {
        _menuRepository = menuRepository;
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        var requestedItemIds = request.ItemIds.Distinct().ToList();
        _logger.LogInformation("Creating a new order for address {Address} with {ItemCount} requested items.", request.DeliveryAddress, requestedItemIds.Count);
        var availableItems = await _menuRepository.GetAvailableByIdsAsync(requestedItemIds, cancellationToken);

        if (availableItems.Count != requestedItemIds.Count)
        {
            _logger.LogWarning("Order creation failed because one or more requested menu items are unavailable.");
            throw new InvalidOperationException("One or more menu items are missing or unavailable.");
        }

        var order = new Order
        {
            DeliveryAddress = request.DeliveryAddress.Trim(),
            Status = "Pending",
            TotalPrice = availableItems.Sum(item => item.Price),
            OrderItems = availableItems.Select(item => new OrderItem
            {
                MenuItemId = item.Id,
                Quantity = 1
            }).ToList()
        };

        var createdOrder = await _orderRepository.AddAsync(order, cancellationToken);
        _logger.LogInformation("Order {OrderId} was created successfully with total price {TotalPrice}.", createdOrder.Id, createdOrder.TotalPrice);

        return new OrderResponseDto
        {
            Id = createdOrder.Id,
            DeliveryAddress = createdOrder.DeliveryAddress,
            TotalPrice = createdOrder.TotalPrice,
            Status = createdOrder.Status,
            CreatedAtUtc = createdOrder.CreatedAtUtc,
            Items = createdOrder.OrderItems.Select(orderItem => new OrderLineDto
            {
                MenuItemId = orderItem.MenuItemId,
                Name = orderItem.MenuItem?.Name ?? string.Empty,
                Price = orderItem.MenuItem?.Price ?? 0,
                Quantity = orderItem.Quantity
            }).ToList()
        };
    }

    public async Task<OrderResponseDto> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving order {OrderId}.", id);
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);

        if (order is null)
        {
            _logger.LogWarning("Order {OrderId} was not found.", id);
            throw new KeyNotFoundException("Order not found");
        }

        return new OrderResponseDto
        {
            Id = order.Id,
            DeliveryAddress = order.DeliveryAddress,
            TotalPrice = order.TotalPrice,
            Status = order.Status,
            CreatedAtUtc = order.CreatedAtUtc,
            Items = order.OrderItems.Select(orderItem => new OrderLineDto
            {
                MenuItemId = orderItem.MenuItemId,
                Name = orderItem.MenuItem?.Name ?? string.Empty,
                Price = orderItem.MenuItem?.Price ?? 0,
                Quantity = orderItem.Quantity
            }).ToList()
        };
    }
}
