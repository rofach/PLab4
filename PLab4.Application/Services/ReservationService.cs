using RestaurantSystem.Application.DTOs;
using RestaurantSystem.Application.Interfaces.Repositories;
using RestaurantSystem.Application.Interfaces.Services;
using RestaurantSystem.Domain.Entities;
using RestaurantSystem.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace RestaurantSystem.Application.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(IReservationRepository reservationRepository, ILogger<ReservationService> logger)
    {
        _reservationRepository = reservationRepository;
        _logger = logger;
    }

    public async Task<ReservationResponseDto> ReserveTableAsync(CreateReservationRequestDto request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Creating reservation for table {TableId} at {DateTimeUtc} for {GuestCount} guests.",
            request.TableId,
            request.DateTime,
            request.GuestCount);

        var table = await _reservationRepository.GetTableByIdAsync(request.TableId, cancellationToken);
        if (table is null)
        {
            _logger.LogWarning("Reservation failed because table {TableId} was not found.", request.TableId);
            throw new KeyNotFoundException("Table not found");
        }

        if (request.GuestCount > table.Capacity)
        {
            _logger.LogWarning(
                "Reservation failed because guest count {GuestCount} exceeds capacity {Capacity} for table {TableId}.",
                request.GuestCount,
                table.Capacity,
                request.TableId);
            throw new InvalidOperationException("Guest count exceeds table capacity.");
        }

        if (await _reservationRepository.ExistsForTableAndDateTimeAsync(request.TableId, request.DateTime, cancellationToken))
        {
            _logger.LogWarning(
                "Reservation conflict detected for table {TableId} at {DateTimeUtc}.",
                request.TableId,
                request.DateTime);
            throw new ConflictException("This table is already reserved for the selected time.");
        }

        var reservation = new Reservation
        {
            TableId = request.TableId,
            DateTimeUtc = request.DateTime,
            GuestCount = request.GuestCount
        };

        var createdReservation = await _reservationRepository.AddAsync(reservation, cancellationToken);
        _logger.LogInformation("Reservation {ReservationId} was created successfully for table {TableId}.", createdReservation.Id, createdReservation.TableId);

        return new ReservationResponseDto
        {
            Id = createdReservation.Id,
            TableId = createdReservation.TableId,
            TableName = createdReservation.Table?.Name ?? string.Empty,
            DateTimeUtc = createdReservation.DateTimeUtc,
            GuestCount = createdReservation.GuestCount,
            CreatedAtUtc = createdReservation.CreatedAtUtc
        };
    }

    public async Task DeleteReservationAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleting reservation {ReservationId}.", reservationId);
        var reservation = await _reservationRepository.GetByIdAsync(reservationId, cancellationToken);
        if (reservation is null)
        {
            _logger.LogWarning("Reservation {ReservationId} was not found for deletion.", reservationId);
            throw new KeyNotFoundException("Reservation not found");
        }

        await _reservationRepository.DeleteAsync(reservation, cancellationToken);
        _logger.LogInformation("Reservation {ReservationId} was deleted successfully.", reservationId);
    }

    public async Task<List<ReservedTableDto>> GetReservedTablesAsync(CancellationToken cancellationToken = default)
    {
        var reservations = await _reservationRepository.GetAllAsync(cancellationToken);
        _logger.LogInformation("Retrieved {Count} reserved table records.", reservations.Count);

        return reservations
            .OrderBy(reservation => reservation.DateTimeUtc)
            .Select(reservation => new ReservedTableDto
            {
                ReservationId = reservation.Id,
                TableId = reservation.TableId,
                TableName = reservation.Table?.Name ?? string.Empty,
                TableCapacity = reservation.Table?.Capacity ?? 0,
                DateTimeUtc = reservation.DateTimeUtc,
                GuestCount = reservation.GuestCount
            })
            .ToList();
    }

    public async Task<List<AvailableTableDto>> GetAvailableTablesAsync(DateTime dateTimeUtc, int guestCount, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Retrieving available tables for {DateTimeUtc} with guest count {GuestCount}.",
            dateTimeUtc,
            guestCount);

        var tables = await _reservationRepository.GetAvailableTablesAsync(dateTimeUtc, guestCount, cancellationToken);

        return tables
            .OrderBy(table => table.Capacity)
            .ThenBy(table => table.Name)
            .Select(table => new AvailableTableDto
            {
                TableId = table.Id,
                TableName = table.Name,
                Capacity = table.Capacity
            })
            .ToList();
    }
}
