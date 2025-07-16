using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RoomReservation.API;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Application.Interfaces.Services;
using RoomReservation.Infrastructure.Configurations;
using RoomReservation.Infrastructure.Persistence.Contexts;
using RoomReservation.Infrastructure.Persistence.Repositories;
using RoomReservation.Infrastructure.Services;


namespace RoomReservation.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private static readonly InMemoryDatabaseRoot InMemoryDatabaseRoot = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Remove o contexto anterior
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<RoomReservationDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            // Adiciona a infraestrutura com InMemory
            services.AddInfrastructure(new ConfigurationBuilder().Build(), options =>
                options.UseInMemoryDatabase("TestDb", InMemoryDatabaseRoot));

            // Garante que o banco seja criado
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<RoomReservationDbContext>();
            db.Database.EnsureCreated();
        });
    }
}


