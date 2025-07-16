using FluentAssertions;
using Moq;
using RoomReservation.Application.Features.Rooms.Handlers.QueryHandler;
using RoomReservation.Application.Features.Rooms.Queries;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;

namespace RoomReservation.Tests.Application.Features.Rooms.Queries;

public class GetAllRoomsQueryHandlerTests
{
    private readonly Mock<IRoomRepository> _repositoryMock = new();

    [Fact]
    public async Task Handle_Should_Return_ListOfRoomDto()
    {
        // Arrange
        var rooms = new List<Room>
        {
            new() { Id = Guid.NewGuid(), Name = "Sala 101", Capacity = 10 },
            new() { Id = Guid.NewGuid(), Name = "Sala 102", Capacity = 20 }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(rooms);

        var handler = new GetAllRoomsQueryHandler(_repositoryMock.Object);
        var query = new GetAllRoomsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Sala 101");
        result[1].Capacity.Should().Be(20);

        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }
}
