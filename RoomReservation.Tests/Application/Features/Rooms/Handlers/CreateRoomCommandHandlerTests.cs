using FluentAssertions;
using Moq;
using RoomReservation.Application.Features.Rooms.Commands;
using RoomReservation.Application.Features.Rooms.Handlers.CommandHandler;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;

namespace RoomReservation.Tests.Application.Features.Rooms.Handlers;

public class CreateRoomCommandHandlerTests
{
    private readonly Mock<IRoomRepository> _roomRepositoryMock = new();

    [Fact]
    public async Task Handle_Should_CreateRoom_And_Return_Id()
    {
        // Arrange
        var command = new CreateRoomCommand
        {
            Name = "Sala de Reunião 1",
            Capacity = 10
        };

        var handler = new CreateRoomCommandHandler(_roomRepositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty("porque a sala criada deve retornar um Guid válido");

        _roomRepositoryMock.Verify(r => r.AddAsync(It.Is<Room>(room =>
            room.Name == command.Name &&
            room.Capacity == command.Capacity
        )), Times.Once);
    }
}
