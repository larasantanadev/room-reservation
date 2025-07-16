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

builder.Services.AddHttpClient("ExternalApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:9999");
})
.AddTransientHttpErrorPolicy(policy =>
    policy.WaitAndRetryAsync(2, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
.AddTransientHttpErrorPolicy(policy =>
    policy.CircuitBreakerAsync(2, TimeSpan.FromSeconds(30)));

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
