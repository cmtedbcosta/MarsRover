using MarsRover.Ports;
using MarsRover.Service.Controls;
using MarsRover.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MarsRover.Service
{
    public static class ServiceDependencies
    {
        public static void Register(IServiceCollection services, bool showDebugLogs)
        {
            services.AddSingleton(new Settings(showDebugLogs));
            services.AddScoped<ILogger, ServiceLogger>();
            services.AddScoped<IDirectionControl, DirectionControl>();
            services.AddScoped<IMovementControl, MovementControl>();
            services.AddScoped<IPlanControl, PlanControl>();
            services.AddScoped<INavigationControl, NavigationControl>();
            services.AddScoped<IMissionControl, MissionControl>();
        }
    }
}
