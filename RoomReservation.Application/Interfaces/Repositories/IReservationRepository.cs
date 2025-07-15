using RoomReservation.Domain.Entities;

namespace RoomReservation.Application.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        Task AddAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(Reservation reservation);
        Task<List<Reservation>> GetAllAsync();
        Task<Reservation?> GetByIdAsync(Guid id);
    }
}
