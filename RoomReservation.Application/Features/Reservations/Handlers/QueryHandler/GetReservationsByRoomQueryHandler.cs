using MediatR;
using RoomReservation.Application.DTOs.Reservation;
using RoomReservation.Application.Features.Reservations.Queries;
using RoomReservation.Application.Interfaces.Repositories;

namespace RoomReservation.Application.Features.Reservations.Handlers;

public sealed class GetReservationsByRoomQueryHandler : IRequestHandler<GetReservationsByRoomQuery, List<ReservationDto>>
{
    private readonly IReservationRepository _repository;

    public GetReservationsByRoomQueryHandler(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ReservationDto>> Handle(GetReservationsByRoomQuery request, CancellationToken cancellationToken)
    {
        var reservations = await _repository.GetByRoomIdAsync(request.RoomId);

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
