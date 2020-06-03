using System;
using System.Collections.Generic;
using System.Linq;
using MarsRover.Models;
using MarsRover.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace MarsRover.Service.Controls
{
    internal class NavigationControl : INavigationControl
    {
        private readonly IMovementControl _movementControl;
        private readonly IDirectionControl _directionControl;
        private readonly ILogger _logger;

        public NavigationControl(IMovementControl movementControl, IDirectionControl directionControl, ILogger logger)
        {
            _movementControl = movementControl;
            _directionControl = directionControl;
            _logger = logger;
        }

        public Rover Navigate(Plateau plateau, Rover rover, IEnumerable<Command> commands, IEnumerable<Rover> otherRovers)
        {
            if (plateau == null) throw new ArgumentNullException(nameof(plateau));
            if (rover == null) throw new ArgumentNullException(nameof(rover));
            if (commands == null) throw new ArgumentNullException(nameof(commands));
            if (otherRovers == null) throw new ArgumentNullException(nameof(otherRovers));

            return commands.Aggregate(rover, (current, command) => Goto(plateau, current, command, otherRovers));
        }

        private bool IsOutOfBounds((uint X, uint Y) position, Plateau plateau, bool isDeployment = false)
        {
            if (position.X > plateau.MaxSizeX || position.Y > plateau.MaxSizeY)
                _logger.LogDebug(isDeployment
                    ? $"Can't deploy position {position} because that is out of plateau"
                    : $"Can't move to position {position} because that is out of plateau");

            return position.X > plateau.MaxSizeX || position.Y > plateau.MaxSizeY;
        }

        private bool IsSamePositionOfOtherRover((uint X, uint Y) position, IEnumerable<Rover> otherRovers, bool isDeployment = false)
        {
            var roverInDestinyPosition = otherRovers.FirstOrDefault(r => r.Position == position);

            if (roverInDestinyPosition != null)
            {
                _logger.LogDebug(isDeployment
                    ? $"Can't deploy to position {position} because it's occupied by rover {roverInDestinyPosition.Name}"
                    : $"Can't move to position {position} because it's occupied by rover {roverInDestinyPosition.Name}");
            }

            return roverInDestinyPosition != null;
        }

        private Rover Goto(Plateau plateau, Rover rover, Command command, IEnumerable<Rover> otherRovers)
        {
            if (rover.IsWaitingRescue)
            {
                _logger.LogDebug($"{rover.Name} is waiting for rescue. Ignoring command {command.Name}");
                return rover;
            }

            // Check deployment position
            if (IsOutOfBounds(rover.Position, plateau, true))
                return new RoverBuilder(rover.Id)
                    .NotDeployed($"Rover would be deployed in an invalid position {rover.Position}")
                    .Build();

            // Check for other rovers in deployment position
            if (IsSamePositionOfOtherRover(rover.Position, otherRovers, true))
                return new RoverBuilder(rover.Id)
                    .NotDeployed($"Collision detected on deployment position {rover.Position}.")
                    .Build();

            _logger.LogDebug($"Processing command {command.Name} for rover {rover.Name}");

            if (command != Command.Move)
            {
                var direction = _directionControl.GetNextDirection(rover.FacingDirection, command);

                _logger.LogDebug($"Next direction for rover {rover.Name} is {direction.Name}");

                return new RoverBuilder(rover.Id).Operational(rover.Position.X,
                        rover.Position.Y,
                        direction)
                    .Build();
            }

            var nextPosition = _movementControl.GetNextPosition(rover.Position, rover.FacingDirection);

            _logger.LogDebug($"Next position for rover {rover.Name} is {nextPosition}");

            if (IsSamePositionOfOtherRover(nextPosition, otherRovers))
                return new RoverBuilder(rover.Id).StoppedBeforeCrash(rover.Position.X,
                        rover.Position.Y,
                        rover.FacingDirection,
                        $"Collision detected on {nextPosition}.")
                    .Build();

            return IsOutOfBounds(nextPosition,
                plateau)
                ? new RoverBuilder(rover.Id).StoppedBeforeCrash(rover.Position.X,
                        rover.Position.Y,
                        rover.FacingDirection,
                        $"Out of bounds detected on {nextPosition}.")
                    .Build()
                : new RoverBuilder(rover.Id).Operational(nextPosition.X,
                        nextPosition.Y,
                        rover.FacingDirection)
                    .Build();

        }
    }
}
