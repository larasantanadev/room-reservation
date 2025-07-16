using MediatR;
using RoomReservation.Application.Common;
using RoomReservation.Application.DTOs.Reservation;
using RoomReservation.Domain.Enums;

namespace RoomReservation.Application.Features.Reservations.Commands;

public class UpdateReservationCommand : IRequest<Result<bool>>
{
    public Guid Id { get; init; }
    public Guid RoomId { get; init; }
    public string ReservedBy { get; init; } = default!;
    public int NumberOfAttendees { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public ReservationStatus Status { get; init; }

    public static UpdateReservationCommand From(Guid id, UpdateReservationDto dto) => new()
    {
        Id = id,
        RoomId = dto.RoomId,
        ReservedBy = dto.ReservedBy,
        NumberOfAttendees = dto.NumberOfAttendees,
        StartTime = dto.StartTime,
        EndTime = dto.EndTime,
        Status = dto.Status
    };
}
