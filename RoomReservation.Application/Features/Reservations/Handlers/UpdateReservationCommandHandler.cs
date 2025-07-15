using MediatR;
using RoomReservation.Application.Features.Reservations.Commands;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Enums;

namespace RoomReservation.Application.Features.Reservations.Handlers;

public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, bool>
{
    private readonly IReservationRepository _repository;

    public UpdateReservationCommandHandler(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _repository.GetByIdAsync(request.Id);
        if (reservation == null) return false;

        reservation.RoomId = request.RoomId;
        reservation.ReservedBy = request.ReservedBy;
        reservation.StartTime = request.StartTime;
        reservation.EndTime = request.EndTime;
        reservation.Status = request.Status;

        await _repository.UpdateAsync(reservation);
        return true;
    }
}
