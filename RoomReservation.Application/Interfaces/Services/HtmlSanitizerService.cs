using Ganss.Xss;

namespace RoomReservation.Application.Services;

public class HtmlSanitizerService : IHtmlSanitizerService
{
    private readonly HtmlSanitizer _sanitizer;

    public HtmlSanitizerService()
    {
        _sanitizer = new HtmlSanitizer();
    }

    public string Sanitize(string input)
    {
        return _sanitizer.Sanitize(input);
    }
}
