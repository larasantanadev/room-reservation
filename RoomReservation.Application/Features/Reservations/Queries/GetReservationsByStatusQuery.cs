using MediatR;
using RoomReservation.Application.DTOs.Reservation;
using RoomReservation.Domain.Entities;
using RoomReservation.Domain.Enums;

namespace RoomReservation.Application.Features.Reservations.Queries
{
    public sealed class GetReservationsByStatusQuery : IRequest<List<ReservationDto>>
    {
        public ReservationStatus Status { get; }

        public GetReservationsByStatusQuery(ReservationStatus status)
        {
            Status = status;
        }
    }
}
