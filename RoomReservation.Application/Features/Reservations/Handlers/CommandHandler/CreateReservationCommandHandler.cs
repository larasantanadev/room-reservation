using FluentValidation;
using MediatR;
using RoomReservation.Application.Common;
using RoomReservation.Application.Features.Reservations.Commands;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Result<Guid>>
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

    public async Task<Result<Guid>> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result<Guid>.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());

        var room = await _roomRepository.GetByIdAsync(request.RoomId);
        if (room is null)
        {
            return Result<Guid>.Fail(new List<string> { "Sala não encontrada." });
        }

        if (request.NumberOfAttendees > room.Capacity)
        {
            return Result<Guid>.Fail(new List<string> { $"A sala suporta no máximo {room.Capacity} pessoas." });
        }

        var overlappingReservations = await _repository.GetByRoomIdAsync(request.RoomId);
        bool hasConflict = overlappingReservations.Any(r =>
            r.StartTime < request.EndTime && request.StartTime < r.EndTime);

        if (hasConflict)
        {
            return Result<Guid>.Fail(new List<string> { "Já existe uma reserva para essa sala no período informado." });
        }

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
        return Result<Guid>.Ok(reservation.Id);
    }
}
