using MediatR;
using RoomReservation.Application.Common;
using RoomReservation.Domain.Enums;

namespace RoomReservation.Application.Features.Reservations.Commands;

public class CreateReservationCommand : IRequest<Result<Guid>>
{
    public Guid RoomId { get; set; }
    public required string ReservedBy { get; set; }
    public int NumberOfAttendees { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ReservationStatus Status { get; set; }
}

