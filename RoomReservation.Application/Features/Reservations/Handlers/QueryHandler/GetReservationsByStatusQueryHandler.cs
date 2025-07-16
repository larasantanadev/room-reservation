using MediatR;
using RoomReservation.Application.DTOs.Reservation;
using RoomReservation.Application.Features.Reservations.Queries;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;

namespace RoomReservation.Application.Features.Reservations.Handlers.QueryHandler;

public class GetReservationsByStatusQueryHandler : IRequestHandler<GetReservationsByStatusQuery, List<ReservationDto>>
{
    private readonly IReservationRepository _reservationRepository;

    public GetReservationsByStatusQueryHandler(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<List<ReservationDto>> Handle(GetReservationsByStatusQuery request, CancellationToken cancellationToken)
    {
        var reservations = await _reservationRepository.GetByStatusAsync(request.Status);

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
