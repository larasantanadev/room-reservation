using MediatR;
using RoomReservation.Application.DTOs.Reservation;
using RoomReservation.Application.Features.Reservations.Queries;
using RoomReservation.Application.Interfaces.Repositories;

namespace RoomReservation.Application.Features.Reservations.Handlers;

public class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, ReservationDto>
{
    private readonly IReservationRepository _repository;

    public GetReservationByIdQueryHandler(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReservationDto> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        var reservation = await _repository.GetByIdAsync(request.Id);
        if (reservation == null) return null!;

        return new ReservationDto
        {
            Id = reservation.Id,
            RoomId = reservation.RoomId,
            ReservedBy = reservation.ReservedBy,
            StartTime = reservation.StartTime,
            EndTime = reservation.EndTime,
            Status = reservation.Status.ToString()
        };
    }
}
