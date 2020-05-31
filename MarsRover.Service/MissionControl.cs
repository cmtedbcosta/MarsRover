using System;
using System.Collections.Generic;
using System.Linq;
using MarsRover.Models;
using MarsRover.Ports;
using MarsRover.Service.Controls;
using MarsRover.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace MarsRover.Service
{
    public class MissionControl : IMissionControl
    {
        private readonly INavigationControl _navigationControl;
        private readonly ILogger _logger;

        public MissionControl(ILoggerFactory loggerFactory, INavigationControl navigationControl)
        {
            _navigationControl = navigationControl;
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

            var roversCurrentPosition = roversRoutes.ToDictionary(roverRoutes => roverRoutes.Rover.Id, roverRoutes => roverRoutes.Rover);

            var roversAfterNavigation = new List<Rover>();
            foreach (var roverRoutes in roversRoutes)
            {
                var roverAfterNavigation =  _navigationControl.Navigate(plan.Plateau,
                    roverRoutes.Rover,
                    roverRoutes.Commands,
                    roversCurrentPosition.Where(m => m.Key != roverRoutes.Rover.Id).Select(m => m.Value));

                // Update rover map
                roversCurrentPosition[roverAfterNavigation.Id] = roverAfterNavigation;

                roversAfterNavigation.Add(roverAfterNavigation);
            }

            _logger.LogInformation("New rover(s) positions:");
            foreach (var roverAfterNavigation in roversAfterNavigation)
            {
                if (!string.IsNullOrEmpty(roverAfterNavigation.Error))
                    _logger.LogCritical($"{roverAfterNavigation.Name} was not deployed because of errors and is waiting for rescue. {roverAfterNavigation.Error}");
                else if (roverAfterNavigation.IsWaitingRescue)
                    _logger.LogCritical($"{roverAfterNavigation.Name} is waiting for rescue at position {roverAfterNavigation.Position} facing direction {roverAfterNavigation.FacingDirection.Name}.");
                else
                    _logger.LogInformation($"{roverAfterNavigation.Name} is now at position {roverAfterNavigation.Position} facing direction {roverAfterNavigation.FacingDirection.Name}.");
            }

            return roversAfterNavigation.Concat(plan.RoversWithError).OrderBy(r => r.Id);
        }
    }
}
