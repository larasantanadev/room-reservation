using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RoomReservation.Infrastructure.Persistence.Contexts;

namespace RoomReservation.Tests.APITest.TestHelpers
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove o DbContext real
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<RoomReservationDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                // Cria conexão SQLite em memória
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                // Adiciona contexto com SQLite em memória
                services.AddDbContext<RoomReservationDbContext>(options =>
                    options.UseSqlite(connection));

                // Cria o banco
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<RoomReservationDbContext>();
                db.Database.EnsureCreated();
            });
        }
    }
}
