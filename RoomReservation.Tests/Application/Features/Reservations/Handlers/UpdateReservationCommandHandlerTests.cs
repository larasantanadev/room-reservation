using FluentAssertions;
using FluentValidation;
using Moq;
using RoomReservation.Application.Features.Reservations.Commands;
using RoomReservation.Application.Features.Reservations.Handlers.CommandHandler;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;
using RoomReservation.Domain.Enums;

namespace RoomReservation.Tests.Application.Features.Reservations.Handlers;

public class UpdateReservationCommandHandlerTests
{
    private readonly Mock<IReservationRepository> _repositoryMock = new();
    private readonly Mock<IValidator<UpdateReservationCommand>> _validatorMock = new();

    [Fact]
    public async Task Handle_Should_UpdateReservation_And_Return_True()
    {
        // Arrange
        var reservationId = Guid.NewGuid();

        var command = new UpdateReservationCommand
        {
            Id = reservationId,
            RoomId = Guid.NewGuid(),
            ReservedBy = "Lara Santana",
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            Status = ReservationStatus.Confirmed
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        _repositoryMock
            .Setup(r => r.GetByIdAsync(reservationId))
            .ReturnsAsync(new Reservation { Id = reservationId });

        var handler = new UpdateReservationCommandHandler(_repositoryMock.Object, _validatorMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue("porque a reserva existente deve ser atualizada com sucesso");

        _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Reservation>(res =>
            res.Id == command.Id &&
            res.RoomId == command.RoomId &&
            res.ReservedBy == command.ReservedBy &&
            res.StartTime == command.StartTime &&
            res.EndTime == command.EndTime &&
            res.Status == command.Status
        )), Times.Once);

        _validatorMock.Verify(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()), Times.Once);
    }
}
