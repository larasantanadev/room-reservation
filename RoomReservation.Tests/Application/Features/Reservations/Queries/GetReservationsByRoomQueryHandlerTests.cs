using FluentAssertions;
using Moq;
using RoomReservation.Application.DTOs.Reservation;
using RoomReservation.Application.Features.Reservations.Handlers;
using RoomReservation.Application.Features.Reservations.Queries;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;
using RoomReservation.Domain.Enums;

namespace RoomReservation.Tests.Application.Features.Reservations.Handlers;

public class GetReservationsByRoomQueryHandlerTests
{
    private readonly Mock<IReservationRepository> _repositoryMock = new();

    [Fact]
    public async Task Handle_Should_Return_Reservations_For_Specific_Room()
    {
        // Arrange
        var roomId = Guid.NewGuid();

        var reservations = new List<Reservation>
        {
            new()
            {
                Id = Guid.NewGuid(),
                RoomId = roomId,
                ReservedBy = "Lara Santana",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(1),
                Status = ReservationStatus.Confirmed
            },
            new()
            {
                Id = Guid.NewGuid(),
                RoomId = roomId,
                ReservedBy = "Outro Usuário",
                StartTime = DateTime.UtcNow.AddHours(2),
                EndTime = DateTime.UtcNow.AddHours(3),
                Status = ReservationStatus.Pending
            }
        };

        _repositoryMock
            .Setup(r => r.GetByRoomIdAsync(roomId))
            .ReturnsAsync(reservations);

        var handler = new GetReservationsByRoomQueryHandler(_repositoryMock.Object);
        var query = new GetReservationsByRoomQuery(roomId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.All(r => r.RoomId == roomId).Should().BeTrue();
        result[0].ReservedBy.Should().Be("Lara Santana");
        result[0].Status.Should().Be("Confirmed");

        _repositoryMock.Verify(r => r.GetByRoomIdAsync(roomId), Times.Once);
    }
}
