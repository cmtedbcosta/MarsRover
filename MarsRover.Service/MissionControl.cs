using System;
using System.Collections.Generic;
using System.Linq;
using MarsRover.Models;
using MarsRover.Ports;
using MarsRover.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace MarsRover.Service
{
    public class MissionControl : IMissionControl
    {
        private readonly INavigationControl _navigationControl;
        private readonly IPlanControl _planControl;
        private readonly ILogger _logger;

        public MissionControl(ILogger logger, INavigationControl navigationControl, IPlanControl planControl)
        {
            _navigationControl = navigationControl;
            _planControl = planControl;
            _logger = logger;
        }

        public IEnumerable<Rover> Execute(string command)
        {
            if (string.IsNullOrEmpty(command?.Trim())) throw new ArgumentNullException(nameof(command));
            command = command.Trim();

            _logger.LogDebug($"Received command: {Environment.NewLine}{command}");

            var plan = _planControl.GeneratePlan(command);

            // Log Rovers with error
            foreach (var roverWithError in plan.RoversWithError)
            {
                _logger.LogError(roverWithError.Error);
            }

            _logger.LogDebug($"Plateau is {plan.Plateau.MaxSizeX} x {plan.Plateau.MaxSizeY}");

            var roversRoutes = plan.RoverRoutes.ToArray();

            var roversCurrentPosition = roversRoutes.ToDictionary(roverRoutes => roverRoutes.Rover.Id,
                roverRoutes => roverRoutes.Rover);

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

            ReportRoversFinalPositions(roversAfterNavigation);

            return roversAfterNavigation.Concat(plan.RoversWithError).OrderBy(r => r.Id);
        }

        private void ReportRoversFinalPositions(IEnumerable<Rover> roversAfterNavigation)
        {
            if (roversAfterNavigation == null) throw new ArgumentNullException(nameof(roversAfterNavigation));

            foreach (var roverAfterNavigation in roversAfterNavigation)
            {
                if (roverAfterNavigation.IsStoppedBeforeCollision)
                    _logger.LogCritical(
                        $"{roverAfterNavigation.Name} is waiting for rescue at position {roverAfterNavigation.Position} facing direction {roverAfterNavigation.FacingDirection.Name}. {roverAfterNavigation.Error}");
                else if (roverAfterNavigation.IsWaitingRescue)
                    _logger.LogCritical(
                        $"{roverAfterNavigation.Name} was not deployed because of errors and is waiting for rescue. {roverAfterNavigation.Error}");
                else
                    _logger.LogInformation(
                        $"{roverAfterNavigation.Name} is now at position {roverAfterNavigation.Position} facing direction {roverAfterNavigation.FacingDirection.Name}.");
            }
        }
    }
}
