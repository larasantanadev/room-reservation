using FluentValidation;
using MediatR;
using RoomReservation.Application.Features.Reservations.Commands;
using RoomReservation.Application.Interfaces.Repositories;

namespace RoomReservation.Application.Features.Reservations.Handlers.CommandHandler;

public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, bool>
{
    private readonly IReservationRepository _repository;
    private readonly IValidator<UpdateReservationCommand> _validator;

    public UpdateReservationCommandHandler(
        IReservationRepository repository,
        IValidator<UpdateReservationCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<bool> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var reservation = await _repository.GetByIdAsync(request.Id);
        if (reservation == null)
            return false;

        reservation.RoomId = request.RoomId;
        reservation.ReservedBy = request.ReservedBy;
        reservation.NumberOfAttendees = request.NumberOfAttendees;
        reservation.StartTime = request.StartTime;
        reservation.EndTime = request.EndTime;
        reservation.Status = request.Status;

        await _repository.UpdateAsync(reservation);
        return true;
    }
}
