using FluentAssertions;
using Moq;
using RoomReservation.Application.DTOs.Reservation;
using RoomReservation.Application.Features.Reservations.Handlers.QueryHandler;
using RoomReservation.Application.Features.Reservations.Queries;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;
using RoomReservation.Domain.Enums;

namespace RoomReservation.Tests.Application.Features.Reservations.Handlers;

public class GetAllReservationsQueryHandlerTests
{
    private readonly Mock<IReservationRepository> _repositoryMock = new();

    [Fact]
    public async Task Handle_Should_Return_ListOfReservationDto()
    {
        // Arrange
        var reservations = new List<Reservation>
        {
            new()
            {
                Id = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                ReservedBy = "Lara Santana",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(1),
                Status = ReservationStatus.Confirmed
            },
            new()
            {
                Id = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                ReservedBy = "Outro Usuário",
                StartTime = DateTime.UtcNow.AddHours(2),
                EndTime = DateTime.UtcNow.AddHours(3),
                Status = ReservationStatus.Pending
            }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(reservations);

        var handler = new GetAllReservationsQueryHandler(_repositoryMock.Object);
        var query = new GetAllReservationsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result[0].ReservedBy.Should().Be("Lara Santana");
        result[0].Status.Should().Be("Confirmed");

        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }
}
