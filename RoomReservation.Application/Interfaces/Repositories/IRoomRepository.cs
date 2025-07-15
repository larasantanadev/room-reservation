using RoomReservation.Domain.Entities;

namespace RoomReservation.Application.Interfaces.Repositories
{
    public interface IRoomRepository
    {
        Task AddAsync(Room room);
        Task<List<Room>> GetAllAsync();
        Task<List<Room>> GetAvailableRoomsAsync(DateTime startTime, DateTime endTime);
    }
}
