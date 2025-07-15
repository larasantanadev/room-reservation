using Microsoft.Extensions.DependencyInjection;
using RoomReservation.Application.Features.Rooms.Handlers.CommandHandler;

namespace RoomReservation.Application.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(CreateRoomCommandHandler).Assembly));

            return services;
        }
    }
}
