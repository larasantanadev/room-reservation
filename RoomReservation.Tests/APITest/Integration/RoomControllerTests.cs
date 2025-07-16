using FluentAssertions;
using RoomReservation.Application.DTOs.Room;
using RoomReservation.Application.Features.Rooms.Commands;
using RoomReservation.Tests.APITest.TestHelpers;
using System.Net;
using System.Net.Http.Json;

namespace RoomReservation.Tests.APITest.Integration
{
    public class RoomControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public RoomControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateRoom_ShouldReturnCreated()
        {
            // Arrange
            var command = new CreateRoomCommand
            {
                Name = "Sala Integração SQLite",
                Capacity = 25
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/room", command);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var roomId = await response.Content.ReadFromJsonAsync<Guid>();
            roomId.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetAll_ShouldReturnListOfRooms()
        {
            // Arrange: cria uma sala para garantir que GET não volte vazio
            var createCommand = new CreateRoomCommand
            {
                Name = "Sala de Teste GET",
                Capacity = 15
            };

            var postResponse = await _client.PostAsJsonAsync("/api/Room", createCommand);
            postResponse.EnsureSuccessStatusCode();

            // Act
            var getResponse = await _client.GetAsync("/api/Room");
            getResponse.EnsureSuccessStatusCode();

            var rooms = await getResponse.Content.ReadFromJsonAsync<List<RoomDto>>();

            // Assert
            rooms.Should().NotBeNull();
            rooms.Should().ContainSingle(r => r.Name == "Sala de Teste GET" && r.Capacity == 15);
        }

        [Fact]
        public async Task GetAvailableRooms_ShouldReturnOnlyRoomsWithoutConflicts()
        {
            // Arrange: cria uma sala
            var createCommand = new CreateRoomCommand
            {
                Name = "Sala Disponível",
                Capacity = 20
            };

            var postResponse = await _client.PostAsJsonAsync("/api/Room", createCommand);
            postResponse.EnsureSuccessStatusCode();

            var roomId = await postResponse.Content.ReadFromJsonAsync<Guid>();

            // Cria uma reserva nessa sala em horário específico
            var reserva = new
            {
                RoomId = roomId,
                ReservedBy = "Teste",
                StartTime = DateTime.UtcNow.AddHours(1),
                EndTime = DateTime.UtcNow.AddHours(2)
            };

            var reservaResponse = await _client.PostAsJsonAsync("/api/Reservation", reserva);
            reservaResponse.EnsureSuccessStatusCode();

            // Act 1: Consulta em horário sem conflito (antes da reserva)
            var responseSemConflito = await _client.GetAsync($"/api/Room/available?startTime={DateTime.UtcNow.AddHours(3):O}&endTime={DateTime.UtcNow.AddHours(4):O}");
            responseSemConflito.EnsureSuccessStatusCode();
            var salasDisponiveis = await responseSemConflito.Content.ReadFromJsonAsync<List<RoomDto>>();

            // Act 2: Consulta em horário com conflito
            var responseComConflito = await _client.GetAsync($"/api/Room/available?startTime={DateTime.UtcNow.AddHours(1):O}&endTime={DateTime.UtcNow.AddHours(2):O}");
            responseComConflito.EnsureSuccessStatusCode();
            var salasComConflito = await responseComConflito.Content.ReadFromJsonAsync<List<RoomDto>>();

            // Assert
            salasDisponiveis.Should().ContainSingle(r => r.Name == "Sala Disponível");
            salasComConflito.Should().NotContain(r => r.Name == "Sala Disponível");
        }
    }
}
