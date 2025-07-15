using Microsoft.EntityFrameworkCore;
using RoomReservation.Domain.Entities;

namespace RoomReservation.Infrastructure.Persistence.Contexts
{
    public class RoomReservationDbContext : DbContext
    {
        public RoomReservationDbContext(DbContextOptions<RoomReservationDbContext> options) : base(options) { }

        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Reservation> Reservations => Set<Reservation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
                .Property(r => r.Status)
                .HasConversion<string>(); 

            base.OnModelCreating(modelBuilder);
        }
    }
}
