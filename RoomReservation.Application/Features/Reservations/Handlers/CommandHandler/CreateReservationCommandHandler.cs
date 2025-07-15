using MediatR;
using RoomReservation.Domain.Entities;
using RoomReservation.Application.Features.Reservations.Commands;
using RoomReservation.Application.Interfaces.Repositories;

namespace RoomReservation.Application.Features.Reservations.Handlers.CommandHandler;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Guid>
{
    private readonly IReservationRepository _repository;

    public CreateReservationCommandHandler(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomId = request.RoomId,
            ReservedBy = request.ReservedBy,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Status = request.Status
        };

        await _repository.AddAsync(reservation);
        return reservation.Id;
    }
}
