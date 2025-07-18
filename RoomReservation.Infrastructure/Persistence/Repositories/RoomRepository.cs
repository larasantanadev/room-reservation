﻿using Microsoft.EntityFrameworkCore;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;
using RoomReservation.Infrastructure.Persistence.Contexts;

namespace RoomReservation.Infrastructure.Persistence.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly RoomReservationDbContext _context;

        public RoomRepository(RoomReservationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Room>> GetAllAsync()
        {
            return await _context.Rooms.ToListAsync();
        }

        public async Task<Room?> GetByIdAsync(Guid id) 
        {
            return await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Room>> GetAvailableRoomsAsync(DateTime startTime, DateTime endTime)
        {
            var reservedRoomIds = await _context.Reservations
                .Where(r =>
                    (startTime < r.EndTime) &&
                    (endTime > r.StartTime)
                )
                .Select(r => r.RoomId)
                .Distinct()
                .ToListAsync();

            return await _context.Rooms
                .Where(room => !reservedRoomIds.Contains(room.Id))
                .ToListAsync();
        }

    }
}
