using FluentAssertions;
using Moq;
using RoomReservation.Application.Features.Reservations.Commands;
using RoomReservation.Application.Features.Reservations.Handlers.CommandHandler;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;

namespace RoomReservation.Tests.Application.Features.Reservations.Handlers;

public class DeleteReservationCommandHandlerTests
{
    private readonly Mock<IReservationRepository> _repositoryMock = new();

    [Fact]
    public async Task Handle_Should_DeleteReservation_And_Return_True()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var command = new DeleteReservationCommand { Id = reservationId };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(reservationId))
            .ReturnsAsync(new Reservation { Id = reservationId });

        var handler = new DeleteReservationCommandHandler(_repositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        _repositoryMock.Verify(r => r.DeleteAsync(It.Is<Reservation>(res => res.Id == reservationId)), Times.Once);
        _repositoryMock.Verify(r => r.GetByIdAsync(reservationId), Times.Once);
    }
}
