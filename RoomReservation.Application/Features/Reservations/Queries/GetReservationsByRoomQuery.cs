using MediatR;
using RoomReservation.Application.DTOs.Reservation;

namespace RoomReservation.Application.Features.Reservations.Queries
{
    public class GetReservationsByRoomQuery : IRequest<List<ReservationDto>>
    {
        public Guid RoomId { get; set; }

        public GetReservationsByRoomQuery(Guid roomId)
        {
            RoomId = roomId;
        }
    }
}
