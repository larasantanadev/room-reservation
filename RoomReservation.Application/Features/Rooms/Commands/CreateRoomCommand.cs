using MediatR;
using System;

namespace RoomReservation.Application.Features.Rooms.Commands
{
    public class CreateRoomCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
    }
}
