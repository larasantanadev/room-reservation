using MediatR;
using RoomReservation.Domain.Entities;

namespace RoomReservation.Application.Features.Rooms.Queries
{
    public sealed class GetAvailableRoomsQuery : IRequest<List<Room>>
    {
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }

        public GetAvailableRoomsQuery(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
