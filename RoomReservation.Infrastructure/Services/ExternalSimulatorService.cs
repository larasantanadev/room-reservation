using RoomReservation.Application.Interfaces.Services;

namespace RoomReservation.Infrastructure.Services
{
    public class ExternalSimulatorService : IExternalSimulatorService
    {
        private readonly HttpClient _httpClient;

        public ExternalSimulatorService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ExternalApi");
        }

        public async Task<string> GetSimulatedResponseAsync()
        {
            var response = await _httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
