namespace RoomReservation.Application.Interfaces.Services
{
    public interface IExternalSimulatorService
    {
        Task<string> GetSimulatedResponseAsync();
    }
}
