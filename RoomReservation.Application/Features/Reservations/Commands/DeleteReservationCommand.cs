using MediatR;

namespace RoomReservation.Application.Features.Reservations.Commands;

public class DeleteReservationCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}
