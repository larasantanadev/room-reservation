using Polly;
using RoomReservation.API.Middlewares;
using RoomReservation.Application.Configurations;
using RoomReservation.Infrastructure.Configurations;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Controllers + Enum como string
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Application e Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Polly + HttpClient 
var externalApiBaseUrl = builder.Configuration["ExternalApi:BaseUrl"] ?? throw new InvalidOperationException("ExternalApi:BaseUrl não configurado");

builder.Services.AddHttpClient("ExternalApi", client =>
{
    client.BaseAddress = new Uri(externalApiBaseUrl);
})
.AddPolicyHandler(Policy<HttpResponseMessage>
    .Handle<HttpRequestException>()
    .OrResult(r => !r.IsSuccessStatusCode)
    .WaitAndRetryAsync(3, retry => TimeSpan.FromSeconds(2),
        onRetry: (response, timespan, retryCount, context) =>
        {
            Console.WriteLine($"[Polly Retry] Tentativa {retryCount} após {timespan.TotalSeconds}s devido a {(response.Exception != null ? response.Exception.Message : response.Result.StatusCode.ToString())}");
        }))
.AddPolicyHandler(Policy<HttpResponseMessage>
    .Handle<HttpRequestException>()
    .OrResult(r => !r.IsSuccessStatusCode)
    .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30)));

// Health Check
builder.Services.AddHealthChecks();

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();

// Endpoint de saúde
app.MapHealthChecks("/health");

app.MapControllers();
app.Run();

public partial class Program { }