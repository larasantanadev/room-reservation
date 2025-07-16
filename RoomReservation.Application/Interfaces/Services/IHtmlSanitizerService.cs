namespace RoomReservation.Application.Services;

public interface IHtmlSanitizerService
{
    string Sanitize(string input);
}
