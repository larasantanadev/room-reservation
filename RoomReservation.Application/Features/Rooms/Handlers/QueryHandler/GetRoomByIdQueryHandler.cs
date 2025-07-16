using MediatR;
using RoomReservation.Application.DTOs.Room;
using RoomReservation.Application.Features.Rooms.Queries;
using RoomReservation.Application.Interfaces.Repositories;

namespace RoomReservation.Application.Features.Rooms.Handlers.QueryHandler;

public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, RoomDto>
{
    private readonly IRoomRepository _repository;

    public GetRoomByIdQueryHandler(IRoomRepository repository)
    {
        _repository = repository;
    }

    public async Task<RoomDto> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        var room = await _repository.GetByIdAsync(request.Id);
        if (room is null) return null!;

        return new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            Capacity = room.Capacity
        };
    }
}
