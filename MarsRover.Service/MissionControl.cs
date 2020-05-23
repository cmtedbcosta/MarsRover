using System;
using System.Collections.Generic;
using System.Linq;
using MarsRover.Models;
using MarsRover.Ports;
using MarsRover.Service.Controls;
using Microsoft.Extensions.Logging;

namespace MarsRover.Service
{
    public class MissionControl : IMissionControl
    {
        private readonly ILogger _logger;

        public MissionControl(ILoggerFactory loggerFactory)
        {
           
            _logger = loggerFactory.CreateLogger<MissionControl>();
        }

        public IEnumerable<Rover> Execute(string command)
        {
            if (string.IsNullOrEmpty(command?.Trim())) throw new ArgumentNullException(nameof(command));
            command = command.Trim();

            _logger.LogInformation($"Received command: {Environment.NewLine}{command}");

            _logger.LogInformation("Decoding mission plan...");
            var planControl = new PlanControl(command);
            var plan = planControl.GeneratePlan();

            // Log Rovers with error
            foreach (var roverWithError in plan.RoversWithError)
            {
                _logger.LogError(roverWithError.Error);
            }

            _logger.LogInformation($"Plateau is {plan.Plateau.MaxSizeX} x {plan.Plateau.MaxSizeY}");

            _logger.LogInformation("Moving rover(s)...");
            var roversRoutes = plan.RoverRoutes.ToArray();

            var roverMap = roversRoutes.ToDictionary(roverRoutes => roverRoutes.Rover.Id, roverRoutes => roverRoutes.Rover);

            var roversAfterNavigation = new List<Rover>();
            foreach (var roverRoutes in roversRoutes)
            {
                var navigationControl = new NavigationControl(plan.Plateau,
                    roverRoutes.Rover,
                    roverRoutes.Commands,
                    roverMap.Where(m => m.Key != roverRoutes.Rover.Id).Select(m => m.Value));

                var roverAfterNavigation =  navigationControl.Navigate();

                // Update rover map
                roverMap[roverAfterNavigation.Id] = roverAfterNavigation;

                roversAfterNavigation.Add(roverAfterNavigation);
            }

            _logger.LogInformation("New rover(s) positions:");
            foreach (var roverAfterNavigation in roversAfterNavigation)
            {
                if (!string.IsNullOrEmpty(roverAfterNavigation.Error))
                    _logger.LogCritical($"{roverAfterNavigation.Name} has errors and is waiting for rescue. {roverAfterNavigation.Error}");
                else if (roverAfterNavigation.IsWaitingRescue)
                    _logger.LogCritical($"{roverAfterNavigation.Name} is waiting for rescue.");
                else
                    _logger.LogInformation($"{roverAfterNavigation.Name} is now at position {roverAfterNavigation.Position.ToString()} facing direction {roverAfterNavigation.FacingDirection.Name}.");
            }

            return roversAfterNavigation.Concat(plan.RoversWithError).OrderBy(r => r.Id);
        }
    }
}
