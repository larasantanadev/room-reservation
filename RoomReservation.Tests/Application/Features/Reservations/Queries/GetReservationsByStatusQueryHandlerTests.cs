using FluentAssertions;
using Moq;
using RoomReservation.Application.DTOs.Reservation;
using RoomReservation.Application.Features.Reservations.Handlers.QueryHandler;
using RoomReservation.Application.Features.Reservations.Queries;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Domain.Entities;
using RoomReservation.Domain.Enums;

namespace RoomReservation.Tests.Application.Features.Reservations.Handlers;

public class GetReservationsByStatusQueryHandlerTests
{
    private readonly Mock<IReservationRepository> _repositoryMock = new();

    [Fact]
    public async Task Handle_Should_Return_Reservations_With_Specified_Status()
    {
        // Arrange
        var status = ReservationStatus.Confirmed;

        var reservations = new List<Reservation>
        {
            new()
            {
                Id = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                ReservedBy = "Lara Santana",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(1),
                Status = status
            },
            new()
            {
                Id = Guid.NewGuid(),
                RoomId = Guid.NewGuid(),
                ReservedBy = "Outro Usuário",
                StartTime = DateTime.UtcNow.AddHours(2),
                EndTime = DateTime.UtcNow.AddHours(3),
                Status = status
            }
        };

        _repositoryMock
            .Setup(r => r.GetByStatusAsync(status))
            .ReturnsAsync(reservations);

        var handler = new GetReservationsByStatusQueryHandler(_repositoryMock.Object);
        var query = new GetReservationsByStatusQuery(status);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.All(r => r.Status == "Confirmed").Should().BeTrue();
        result[0].ReservedBy.Should().Be("Lara Santana");

        _repositoryMock.Verify(r => r.GetByStatusAsync(status), Times.Once);
    }
}
