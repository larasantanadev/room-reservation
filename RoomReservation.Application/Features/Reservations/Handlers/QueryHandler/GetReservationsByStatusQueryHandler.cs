using MediatR;
using RoomReservation.Application.Features.Reservations.Queries;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;

namespace RoomReservation.Application.Features.Reservations.Handlers.QueryHandler
{
    public class GetReservationsByStatusQueryHandler : IRequestHandler<GetReservationsByStatusQuery, List<Reservation>>
    {
        private readonly IReservationRepository _reservationRepository;

        public GetReservationsByStatusQueryHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<List<Reservation>> Handle(GetReservationsByStatusQuery request, CancellationToken cancellationToken)
        {
            return await _reservationRepository.GetByStatusAsync(request.Status);
        }
    }
}
