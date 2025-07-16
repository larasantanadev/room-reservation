using Microsoft.EntityFrameworkCore;
using RoomReservation.Domain.Entities;
using RoomReservation.Domain.Enums;
using RoomReservation.Infrastructure.Persistence.Contexts;
using RoomReservation.Infrastructure.Persistence.Repositories;
using Xunit;

namespace RoomReservation.Tests.Infrastructure.Repositories;

public class ReservationRepositoryTests
{
    private RoomReservationDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<RoomReservationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new RoomReservationDbContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldSaveReservation()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var repository = new ReservationRepository(context);

        var room = new Room("Sala Teste", 10);
        context.Rooms.Add(room);
        await context.SaveChangesAsync();

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            ReservedBy = "lara@empresa.com",
            StartTime = DateTime.Now.AddHours(1),
            EndTime = DateTime.Now.AddHours(2),
            Status = ReservationStatus.Pending
        };

        // Act
        await repository.AddAsync(reservation);
        var saved = await context.Reservations.FirstOrDefaultAsync(r => r.Id == reservation.Id);

        // Assert
        Assert.NotNull(saved);
        Assert.Equal(room.Id, saved!.RoomId);
        Assert.Equal(ReservationStatus.Pending, saved.Status);
        Assert.Equal("lara@empresa.com", saved.ReservedBy);
    }

    [Fact]
    public async Task GetByRoomIdAsync_ShouldReturnReservationsForRoom()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var repository = new ReservationRepository(context);

        var room1 = new Room("Sala 1", 8);
        var room2 = new Room("Sala 2", 12);
        context.Rooms.AddRange(room1, room2);
        await context.SaveChangesAsync();

        var res1 = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomId = room1.Id,
            ReservedBy = "teste1@dev.com",
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(1),
            Status = ReservationStatus.Confirmed
        };

        var res2 = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomId = room2.Id,
            ReservedBy = "teste2@dev.com",
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(1),
            Status = ReservationStatus.Confirmed
        };

        context.Reservations.AddRange(res1, res2);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByRoomIdAsync(room1.Id);

        // Assert
        Assert.Single(result);
        Assert.Equal(room1.Id, result[0].RoomId);
        Assert.Equal("teste1@dev.com", result[0].ReservedBy);
    }

    [Fact]
    public async Task GetByStatusAsync_ShouldReturnMatchingReservations()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var repository = new ReservationRepository(context);

        var room = new Room("Sala X", 15);
        context.Rooms.Add(room);
        await context.SaveChangesAsync();

        var confirmed = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            ReservedBy = "confirmado@empresa.com",
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(1),
            Status = ReservationStatus.Confirmed
        };

        var pending = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            ReservedBy = "pendente@empresa.com",
            StartTime = DateTime.Now.AddHours(2),
            EndTime = DateTime.Now.AddHours(3),
            Status = ReservationStatus.Pending
        };

        context.Reservations.AddRange(confirmed, pending);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByStatusAsync(ReservationStatus.Confirmed);

        // Assert
        Assert.Single(result);
        Assert.All(result, r => Assert.Equal(ReservationStatus.Confirmed, r.Status));
        Assert.Contains(result, r => r.ReservedBy == "confirmado@empresa.com");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectReservation()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var repository = new ReservationRepository(context);

        var room = new Room("Sala Detalhe", 6);
        context.Rooms.Add(room);
        await context.SaveChangesAsync();

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            ReservedBy = "Lara",
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(1),
            Status = ReservationStatus.Confirmed
        };

        context.Reservations.Add(reservation);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(reservation.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(reservation.Id, result!.Id);
        Assert.Equal("Lara", result.ReservedBy);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyReservation()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var repository = new ReservationRepository(context);

        var room = new Room("Sala Editar", 20);
        context.Rooms.Add(room);
        await context.SaveChangesAsync();

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            ReservedBy = "Original",
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(1),
            Status = ReservationStatus.Pending
        };

        context.Reservations.Add(reservation);
        await context.SaveChangesAsync();

        // Act
        reservation.ReservedBy = "Atualizado";
        reservation.Status = ReservationStatus.Confirmed;
        await repository.UpdateAsync(reservation);

        var updated = await context.Reservations.FindAsync(reservation.Id);

        // Assert
        Assert.Equal("Atualizado", updated!.ReservedBy);
        Assert.Equal(ReservationStatus.Confirmed, updated.Status);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveReservation()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var repository = new ReservationRepository(context);

        var room = new Room("Sala Remoção", 5);
        context.Rooms.Add(room);
        await context.SaveChangesAsync();

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            ReservedBy = "Remover",
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(1),
            Status = ReservationStatus.Cancelled
        };

        context.Reservations.Add(reservation);
        await context.SaveChangesAsync();

        // Act
        await repository.DeleteAsync(reservation);
        var deleted = await context.Reservations.FindAsync(reservation.Id);

        // Assert
        Assert.Null(deleted);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllReservations()
    {
        // Arrange
        using var context = CreateInMemoryDbContext();
        var repository = new ReservationRepository(context);

        var room = new Room("Sala Principal", 20);
        context.Rooms.Add(room);
        await context.SaveChangesAsync();

        var res1 = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            ReservedBy = "Alice",
            StartTime = DateTime.Now,
            EndTime = DateTime.Now.AddHours(1),
            Status = ReservationStatus.Confirmed
        };

        var res2 = new Reservation
        {
            Id = Guid.NewGuid(),
            RoomId = room.Id,
            ReservedBy = "Bob",
            StartTime = DateTime.Now.AddHours(2),
            EndTime = DateTime.Now.AddHours(3),
            Status = ReservationStatus.Pending
        };

        context.Reservations.AddRange(res1, res2);
        await context.SaveChangesAsync();

        // Act
        var allReservations = await repository.GetAllAsync();

        // Assert
        Assert.Equal(2, allReservations.Count);
        Assert.Contains(allReservations, r => r.ReservedBy == "Alice");
        Assert.Contains(allReservations, r => r.ReservedBy == "Bob");
    }
}
