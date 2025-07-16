using Microsoft.EntityFrameworkCore;
using RoomReservation.Domain.Entities;
using RoomReservation.Infrastructure.Persistence.Contexts;
using RoomReservation.Infrastructure.Persistence.Repositories;
using Xunit;

namespace RoomReservation.Tests.Infrastructure.Repositories
{
    public class RoomRepositoryTests
    {
        private static RoomReservationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<RoomReservationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // banco isolado por teste
                .Options;

            return new RoomReservationDbContext(options);
        }

        [Fact]
        public async Task AddRoom_ShouldSaveCorrectly()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new RoomRepository(context);
            var room = new Room("Sala Reunião A", 12);

            // Act
            await repository.AddAsync(room);
            var saved = await context.Rooms.FirstOrDefaultAsync(r => r.Id == room.Id);

            // Assert
            Assert.NotNull(saved);
            Assert.Equal("Sala Reunião A", saved.Name);
            Assert.Equal(12, saved.Capacity);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllRooms()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new RoomRepository(context);
            await repository.AddAsync(new Room("Sala 1", 8));
            await repository.AddAsync(new Room("Sala 2", 10));

            // Act
            var allRooms = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, allRooms.Count);
        }

        [Fact]
        public async Task GetAvailableRoomsAsync_ShouldReturnOnlyAvailableRooms()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            var repository = new RoomRepository(context);

            var salaDisponivel = new Room("Sala Livre", 10);
            var salaOcupada = new Room("Sala Ocupada", 10);
            context.Rooms.AddRange(salaDisponivel, salaOcupada);
            await context.SaveChangesAsync();

            var reserva = new Reservation
            {
                Id = Guid.NewGuid(),
                RoomId = salaOcupada.Id,
                StartTime = DateTime.Today.AddHours(14),
                EndTime = DateTime.Today.AddHours(16),
                ReservedBy = "João"
            };

            context.Reservations.Add(reserva);
            await context.SaveChangesAsync();

            // Act
            var disponiveis = await repository.GetAvailableRoomsAsync(
                DateTime.Today.AddHours(15), // conflito com a reserva
                DateTime.Today.AddHours(17)
            );

            // Assert
            Assert.Single(disponiveis);
            Assert.Equal("Sala Livre", disponiveis[0].Name);
        }
    }
}
