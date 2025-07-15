using MediatR;
using RoomReservation.Application.DTOs.Reservation;
using RoomReservation.Application.Features.Reservations.Queries;
using RoomReservation.Application.Interfaces.Repositories;

namespace RoomReservation.Application.Features.Reservations.Handlers.QueryHandler
{
    public class GetReservationsByRoomQueryHandler : IRequestHandler<GetReservationsByRoomQuery, List<ReservationDto>>
    {
        private readonly IReservationRepository _reservationRepository;

        public GetReservationsByRoomQueryHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<List<ReservationDto>> Handle(GetReservationsByRoomQuery request, CancellationToken cancellationToken)
        {
            var reservations = await _reservationRepository.GetByRoomIdAsync(request.RoomId);

            return reservations.Select(r => new ReservationDto
            {
                Id = r.Id,
                RoomId = r.RoomId,
                ReservedBy = r.ReservedBy,
                StartTime = r.StartTime, 
                EndTime = r.EndTime,    
                Status = r.Status.ToString() 
            }).ToList();
        }

    }
}
