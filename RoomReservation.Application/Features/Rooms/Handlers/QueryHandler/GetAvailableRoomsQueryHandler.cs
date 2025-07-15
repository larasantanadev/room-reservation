using MediatR;
using RoomReservation.Application.Features.Rooms.Queries;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;

namespace RoomReservation.Application.Features.Rooms.Handlers.QueryHandler
{
    public class GetAvailableRoomsQueryHandler : IRequestHandler<GetAvailableRoomsQuery, List<Room>>
    {
        private readonly IRoomRepository _roomRepository;

        public GetAvailableRoomsQueryHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<List<Room>> Handle(GetAvailableRoomsQuery request, CancellationToken cancellationToken)
        {
            return await _roomRepository.GetAvailableRoomsAsync(request.StartTime, request.EndTime);
        }
    }
}
