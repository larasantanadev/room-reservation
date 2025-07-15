using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoomReservation.Application.Interfaces.Repositories;
using RoomReservation.Infrastructure.Persistence.Contexts;
using RoomReservation.Infrastructure.Persistence.Repositories;

namespace RoomReservation.Infrastructure.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RoomReservationDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("RoomReservationConnection")));

        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();

        return services;
    }
}
