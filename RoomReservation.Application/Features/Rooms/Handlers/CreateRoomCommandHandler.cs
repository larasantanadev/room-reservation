using MediatR;
using RoomReservation.Application.Features.Rooms.Commands;
using RoomReservation.Domain.Entities;
using RoomReservation.Application.Interfaces.Repositories;

namespace RoomReservation.Application.Features.Rooms.Handlers
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Guid>
    {
        private readonly IRoomRepository _roomRepository;

        public CreateRoomCommandHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = new Room
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Capacity = request.Capacity
            };

            await _roomRepository.AddAsync(room);
            return room.Id;
        }
    }
}
