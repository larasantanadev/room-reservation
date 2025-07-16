using FluentValidation;
using MediatR;
using RoomReservation.Application.Features.Reservations.Commands;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Guid>
{
    private readonly IReservationRepository _repository;
    private readonly IRoomRepository _roomRepository;
    private readonly IValidator<CreateReservationCommand> _validator;

    public CreateReservationCommandHandler(
        IReservationRepository repository,
        IRoomRepository roomRepository,
        IValidator<CreateReservationCommand> validator)
    {
        _repository = repository;
        _roomRepository = roomRepository;
        _validator = validator;
    }

    public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomId = request.RoomId,
            ReservedBy = request.ReservedBy,
            NumberOfAttendees = request.NumberOfAttendees,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Status = request.Status
        };

        await _repository.AddAsync(reservation);
        return reservation.Id;
    }
}
