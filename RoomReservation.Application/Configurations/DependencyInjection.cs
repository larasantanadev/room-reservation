using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RoomReservation.Application.Behaviors;
using RoomReservation.Application.Features.Rooms.Handlers.CommandHandler;
using RoomReservation.Application.Services;
using System.Reflection;

namespace RoomReservation.Application.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(CreateRoomCommandHandler).Assembly));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddScoped<IHtmlSanitizerService, HtmlSanitizerService>();

            return services;
        }
    }
}
