using MediatR;
using RoomReservation.Application.Features.Reservations.Commands;
using RoomReservation.Application.Interfaces.Repositories;

namespace RoomReservation.Application.Features.Reservations.Handlers;

public class DeleteReservationCommandHandler : IRequestHandler<DeleteReservationCommand, bool>
{
    private readonly IReservationRepository _repository;

    public DeleteReservationCommandHandler(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _repository.GetByIdAsync(request.Id);
        if (reservation == null) return false;

        await _repository.DeleteAsync(reservation);
        return true;
    }
}
