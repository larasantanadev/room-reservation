using FluentValidation;
using MediatR;
using RoomReservation.Application.Common;
using RoomReservation.Application.Features.Reservations.Commands;
using RoomReservation.Application.Interfaces.Repositories;

public class UpdateReservationCommandHandler : IRequestHandler<UpdateReservationCommand, Result<bool>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IValidator<UpdateReservationCommand> _validator;

    public UpdateReservationCommandHandler(
        IReservationRepository reservationRepository,
        IRoomRepository roomRepository,
        IValidator<UpdateReservationCommand> validator)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
        _validator = validator;
    }

    public async Task<Result<bool>> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result<bool>.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());

        var reservation = await _reservationRepository.GetByIdAsync(request.Id);
        if (reservation is null)
            return Result<bool>.Fail("Reserva não encontrada.");

        var room = await _roomRepository.GetByIdAsync(request.RoomId);
        if (room is null)
            return Result<bool>.Fail("Sala não encontrada.");

        if (request.NumberOfAttendees > room.Capacity)
            return Result<bool>.Fail($"A sala suporta no máximo {room.Capacity} pessoas.");

        var overlappingReservations = await _reservationRepository.GetByRoomIdAsync(request.RoomId);
        bool hasConflict = overlappingReservations
            .Any(r =>
                r.Id != request.Id && 
                r.StartTime < request.EndTime &&
                request.StartTime < r.EndTime
            );

        if (hasConflict)
            return Result<bool>.Fail("Já existe uma reserva para essa sala no período informado.");

        reservation.RoomId = request.RoomId;
        reservation.ReservedBy = request.ReservedBy;
        reservation.NumberOfAttendees = request.NumberOfAttendees;
        reservation.StartTime = request.StartTime;
        reservation.EndTime = request.EndTime;
        reservation.Status = request.Status;

        await _reservationRepository.UpdateAsync(reservation);

        return Result<bool>.Ok(true);
    }
}
