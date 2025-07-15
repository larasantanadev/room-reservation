using MediatR;
using RoomReservation.Application.DTOs.Reservation;
using RoomReservation.Domain.Entities;

namespace RoomReservation.Application.Features.Reservations.Queries;

public sealed class GetReservationsByRoomQuery : IRequest<List<ReservationDto>>
{
    public Guid RoomId { get; }

    public GetReservationsByRoomQuery(Guid roomId)
    {
        RoomId = roomId;
    }
}
