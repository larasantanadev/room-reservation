using FluentAssertions;
using RoomReservation.Application.Features.Reservations.Commands;
using RoomReservation.Application.Features.Rooms.Commands;
using RoomReservation.Domain.Enums;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace RoomReservation.Tests.Integration.Controllers;

public class ReservationsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ReservationsControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_CreateReservation_ReturnsCreated()
    {
        // Arrange — cria uma sala válida
        var roomCommand = new CreateRoomCommand
        {
            Name = "Sala Teste",
            Capacity = 10
        };

        var roomResponse = await _client.PostAsJsonAsync("/api/Rooms", roomCommand);
        roomResponse.EnsureSuccessStatusCode(); // lança erro se não for sucesso

        var roomId = await roomResponse.Content.ReadFromJsonAsync<Guid>();
        roomId.Should().NotBe(Guid.Empty);

        // Act — cria a reserva com a sala criada
        var reservationCommand = new CreateReservationCommand
        {
            RoomId = roomId,
            ReservedBy = "Usuário Teste",
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2),
            Status = ReservationStatus.Confirmed
        };

        var reservationResponse = await _client.PostAsJsonAsync("/api/Reservation", reservationCommand);

        // Assert
        reservationResponse.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
