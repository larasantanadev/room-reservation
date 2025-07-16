using FluentAssertions;
using Moq;
using RoomReservation.Application.Features.Rooms.Handlers.QueryHandler;
using RoomReservation.Application.Features.Rooms.Queries;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;

namespace RoomReservation.Tests.Application.Features.Rooms.Queries;

public class GetAvailableRoomsQueryHandlerTests
{
    private readonly Mock<IRoomRepository> _roomRepositoryMock = new();

    [Fact]
    public async Task Handle_Should_Return_ListOfAvailableRooms()
    {
        // Arrange
        var startTime = DateTime.UtcNow.AddHours(1);
        var endTime = DateTime.UtcNow.AddHours(2);

        var rooms = new List<Room>
        {
            new() { Id = Guid.NewGuid(), Name = "Sala 1", Capacity = 10 },
            new() { Id = Guid.NewGuid(), Name = "Sala 2", Capacity = 20 }
        };

        _roomRepositoryMock
            .Setup(r => r.GetAvailableRoomsAsync(startTime, endTime))
            .ReturnsAsync(rooms);

        var handler = new GetAvailableRoomsQueryHandler(_roomRepositoryMock.Object);
        var query = new GetAvailableRoomsQuery(startTime, endTime);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Sala 1");
        result[1].Capacity.Should().Be(20);

        _roomRepositoryMock.Verify(r => r.GetAvailableRoomsAsync(startTime, endTime), Times.Once);
    }
}
