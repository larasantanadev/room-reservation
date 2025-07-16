using FluentAssertions;
using Moq;
using RoomReservation.Application.DTOs.Reservation;
using RoomReservation.Application.Features.Reservations.Handlers.QueryHandler;
using RoomReservation.Application.Features.Reservations.Queries;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;
using RoomReservation.Domain.Enums;

namespace RoomReservation.Tests.Application.Features.Reservations.Handlers;

public class GetReservationByIdQueryHandlerTests
{
    private readonly Mock<IReservationRepository> _repositoryMock = new();

    [Fact]
    public async Task Handle_Should_Return_ReservationDto_When_Reservation_Exists()
    {
        // Arrange
        var reservationId = Guid.NewGuid();

        var reservation = new Reservation
        {
            Id = reservationId,
            RoomId = Guid.NewGuid(),
            ReservedBy = "Lara Santana",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddHours(1),
            Status = ReservationStatus.Confirmed
        };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(reservationId))
            .ReturnsAsync(reservation);

        var handler = new GetReservationByIdQueryHandler(_repositoryMock.Object);
        var query = new GetReservationByIdQuery(reservationId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(reservation.Id);
        result.ReservedBy.Should().Be("Lara Santana");
        result.Status.Should().Be("Confirmed");

        _repositoryMock.Verify(r => r.GetByIdAsync(reservationId), Times.Once);
    }
}
