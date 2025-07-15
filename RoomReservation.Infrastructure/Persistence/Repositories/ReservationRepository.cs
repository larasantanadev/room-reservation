using Microsoft.EntityFrameworkCore;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;
using RoomReservation.Domain.Enums;
using RoomReservation.Infrastructure.Persistence.Contexts;

namespace RoomReservation.Infrastructure.Persistence.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly RoomReservationDbContext _context;

    public ReservationRepository(RoomReservationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Reservation reservation)
    {
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Reservation reservation)
    {
        _context.Reservations.Update(reservation);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Reservation reservation)
    {
        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Reservation>> GetAllAsync()
    {
        return await _context.Reservations
            .Include(r => r.Room)
            .ToListAsync();
    }

    public async Task<Reservation?> GetByIdAsync(Guid id)
    {
        return await _context.Reservations
            .Include(r => r.Room) 
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Reservation>> GetByStatusAsync(ReservationStatus status)
    {
        return await _context.Reservations
            .Where(r => r.Status == status)
            .Include(r => r.Room)
            .ToListAsync();
    }

    public async Task<List<Reservation>> GetByRoomIdAsync(Guid roomId)
    {
        return await _context.Reservations
            .Include(r => r.Room)
            .Where(r => r.RoomId == roomId)
            .ToListAsync();
    }

}
