using MediatR;
using RoomReservation.Application.DTOs.Room;
using RoomReservation.Application.Features.Rooms.Queries;
using RoomReservation.Application.Interfaces.Repositories;

namespace RoomReservation.Application.Features.Rooms.Handlers.QueryHandler
{
    public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, List<RoomDto>>
    {
        private readonly IRoomRepository _roomRepository;

        public GetAllRoomsQueryHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<List<RoomDto>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
        {
            var rooms = await _roomRepository.GetAllAsync();

            return rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                Name = r.Name,
                Capacity = r.Capacity
            }).ToList();
        }
    }
}
