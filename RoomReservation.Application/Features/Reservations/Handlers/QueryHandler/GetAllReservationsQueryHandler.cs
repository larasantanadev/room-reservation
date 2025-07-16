using MediatR;
using RoomReservation.Application.DTOs.Reservation;
using RoomReservation.Application.Features.Reservations.Queries;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;

namespace RoomReservation.Application.Features.Reservations.Handlers.QueryHandler
{
    public class GetAllReservationsQueryHandler : IRequestHandler<GetAllReservationsQuery, List<ReservationDto>>
    {
        private readonly IReservationRepository _repository;

        public GetAllReservationsQueryHandler(IReservationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ReservationDto>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
        {
            var reservations = await _repository.GetAllAsync();

            return reservations.Select(r => new ReservationDto
            {
                Id = r.Id,
                RoomId = r.RoomId,
                ReservedBy = r.ReservedBy,
                NumberOfAttendees = r.NumberOfAttendees,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                Status = r.Status.ToString()
            }).ToList();
        }
    }
}
