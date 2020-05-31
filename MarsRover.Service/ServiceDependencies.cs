using MarsRover.Ports;
using MarsRover.Service.Controls;
using MarsRover.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MarsRover.Service
{
    public static class ServiceDependencies
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IDirectionControl, DirectionControl>();
            services.AddScoped<IMovementControl, MovementControl>();
            services.AddScoped<INavigationControl, NavigationControl>();
            services.AddScoped<IMissionControl, MissionControl>();
        }
    }
}
