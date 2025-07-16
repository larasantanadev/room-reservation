using FluentAssertions;
using RoomReservation.Application.DTOs.Reservation;
using RoomReservation.Application.Features.Rooms.Commands;
using RoomReservation.Tests.APITest.TestHelpers;
using System.Net;
using System.Net.Http.Json;

namespace RoomReservation.Tests.APITest.Integration;

public class ReservationControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ReservationControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task FullReservationFlow_ShouldSucceed()
    {
        // 1. Cria uma sala
        var sala = new CreateRoomCommand
        {
            Name = "Sala de Reunião",
            Capacity = 10
        };

        var salaResponse = await _client.PostAsJsonAsync("/api/Room", sala);
        salaResponse.EnsureSuccessStatusCode();
        var salaId = await salaResponse.Content.ReadFromJsonAsync<Guid>();

        // 2. Cria uma reserva
        var reserva = new
        {
            RoomId = salaId,
            ReservedBy = "João da Silva",
            StartTime = DateTime.UtcNow.AddHours(1),
            EndTime = DateTime.UtcNow.AddHours(2)
        };

        var reservaResponse = await _client.PostAsJsonAsync("/api/Reservation", reserva);
        reservaResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var reservaId = await reservaResponse.Content.ReadFromJsonAsync<Guid>();

        // 3. Busca por ID
        var getById = await _client.GetAsync($"/api/Reservation/{reservaId}");
        getById.EnsureSuccessStatusCode();
        var reservaDetalhe = await getById.Content.ReadFromJsonAsync<ReservationDto>();
        reservaDetalhe.Should().NotBeNull();
        reservaDetalhe!.ReservedBy.Should().Be("João da Silva");

        // 4. Busca por Status (Pending por padrão)
        var getByStatus = await _client.GetAsync("/api/Reservation/status/Pending");
        getByStatus.EnsureSuccessStatusCode();
        var reservasPorStatus = await getByStatus.Content.ReadFromJsonAsync<List<ReservationDto>>();
        reservasPorStatus.Should().Contain(r => r.Id == reservaId);

        // 5. Busca por RoomId
        var getByRoom = await _client.GetAsync($"/api/Reservation/room/{salaId}");
        getByRoom.EnsureSuccessStatusCode();
        var reservasPorSala = await getByRoom.Content.ReadFromJsonAsync<List<ReservationDto>>();
        reservasPorSala.Should().Contain(r => r.Id == reservaId);

        // 6. Listagem geral
        var getAll = await _client.GetAsync("/api/Reservation");
        getAll.EnsureSuccessStatusCode();
        var todasReservas = await getAll.Content.ReadFromJsonAsync<List<ReservationDto>>();
        todasReservas.Should().Contain(r => r.Id == reservaId);
    }

    [Fact]
    public async Task UpdateAndDeleteReservation_ShouldSucceed()
    {
        // 1. Cria uma sala
        var createRoom = new CreateRoomCommand
        {
            Name = "Sala para Update/Delete",
            Capacity = 5
        };

        var salaResponse = await _client.PostAsJsonAsync("/api/Room", createRoom);
        salaResponse.EnsureSuccessStatusCode();
        var roomId = await salaResponse.Content.ReadFromJsonAsync<Guid>();

        // 2. Cria uma reserva
        var reserva = new
        {
            RoomId = roomId,
            ReservedBy = "Maria Oliveira",
            StartTime = DateTime.UtcNow.AddHours(2),
            EndTime = DateTime.UtcNow.AddHours(3)
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Reservation", reserva);
        createResponse.EnsureSuccessStatusCode();
        var reservationId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        // 3. Atualiza a reserva
        var updateDto = new
        {
            RoomId = roomId,
            ReservedBy = "Maria Atualizada",
            StartTime = DateTime.UtcNow.AddHours(3),
            EndTime = DateTime.UtcNow.AddHours(4)
        };

        var updateResponse = await _client.PutAsJsonAsync($"/api/Reservation/{reservationId}", updateDto);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // 4. Verifica se o nome foi atualizado
        var updatedResult = await _client.GetAsync($"/api/Reservation/{reservationId}");
        updatedResult.EnsureSuccessStatusCode();
        var updatedData = await updatedResult.Content.ReadFromJsonAsync<ReservationDto>();
        updatedData!.ReservedBy.Should().Be("Maria Atualizada");

        // 5. Remove a reserva
        var deleteResponse = await _client.DeleteAsync($"/api/Reservation/{reservationId}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // 6. Verifica se foi removida
        var deletedCheck = await _client.GetAsync($"/api/Reservation/{reservationId}");
        deletedCheck.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
