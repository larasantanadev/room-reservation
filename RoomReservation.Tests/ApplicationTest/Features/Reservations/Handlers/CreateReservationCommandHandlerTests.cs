using FluentAssertions;
using FluentValidation;
using Moq;
using RoomReservation.Application.Features.Reservations.Commands;
using RoomReservation.Application.Features.Reservations.Handlers.CommandHandler;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;
using RoomReservation.Domain.Enums;

namespace RoomReservation.Tests.Application.Features.Reservations.Handlers;

public class CreateReservationCommandHandlerTests
{
    private readonly Mock<IReservationRepository> _reservationRepositoryMock = new();
    private readonly Mock<IRoomRepository> _roomRepositoryMock = new(); 
    private readonly Mock<IValidator<CreateReservationCommand>> _validatorMock = new();

    [Fact]
    public async Task Handle_Should_CreateReservation_And_Return_Id()
    {
        // Arrange
        var command = new CreateReservationCommand
        {
            RoomId = Guid.NewGuid(),
            ReservedBy = "Lara Santana",
            NumberOfAttendees = 5,
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2),
            Status = ReservationStatus.Confirmed
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult());

        var handler = new CreateReservationCommandHandler(
            _reservationRepositoryMock.Object,
            _roomRepositoryMock.Object, 
            _validatorMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeEmpty("porque o handler deve retornar o Guid da reserva criada");

        _reservationRepositoryMock.Verify(
            r => r.AddAsync(It.Is<Reservation>(
                res => res.RoomId == command.RoomId &&
                       res.ReservedBy == command.ReservedBy &&
                       res.NumberOfAttendees == command.NumberOfAttendees &&
                       res.StartTime == command.StartTime &&
                       res.EndTime == command.EndTime &&
                       res.Status == command.Status
            )), Times.Once);

        _validatorMock.Verify(
            v => v.ValidateAsync(command, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
