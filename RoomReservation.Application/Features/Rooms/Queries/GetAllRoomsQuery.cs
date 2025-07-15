using MediatR;
using RoomReservation.Application.DTOs.Room;

namespace RoomReservation.Application.Features.Rooms.Queries
{
    public class GetAllRoomsQuery : IRequest<List<RoomDto>>
    {
    }
}
