using System;
using System.Collections.Generic;
using System.Linq;
using MarsRover.Models;
using MarsRover.Service.Interfaces;

namespace MarsRover.Service.Controls
{
    internal class NavigationControl : INavigationControl
    {
        private readonly IMovementControl _movementControl;
        private readonly IDirectionControl _directionControl;

        public NavigationControl(IMovementControl movementControl, IDirectionControl directionControl)
        {
            _movementControl = movementControl;
            _directionControl = directionControl;
        }

        public Rover Navigate(Plateau plateau, Rover rover, IEnumerable<Command> commands, IEnumerable<Rover> otherRovers)
        {
            if (plateau == null) throw new ArgumentNullException(nameof(plateau));
            if (rover == null) throw new ArgumentNullException(nameof(rover));
            if (commands == null) throw new ArgumentNullException(nameof(commands));
            if (otherRovers == null) throw new ArgumentNullException(nameof(otherRovers));

            return commands.Aggregate(rover, (current, command) => Goto(plateau, current, command, otherRovers));
        }

        private static bool IsOutOfBounds((uint X, uint Y) position, Plateau plateau) =>
            position.X > plateau.MaxSizeX || position.Y > plateau.MaxSizeY;

        private static bool IsSamePositionOfOtherRover((uint X, uint Y) position, IEnumerable<Rover> otherRovers)
        {
            var occupiedPositions = otherRovers.Select(r => r.Position);
            return occupiedPositions.Contains(position);
        }

        private Rover Goto(Plateau plateau, Rover rover, Command command, IEnumerable<Rover> otherRovers)
        {
            if (rover.IsWaitingRescue)
                return rover;

            if (command == Command.Move)
            {
                var nextPosition = _movementControl.GetNextPosition(rover.Position, rover.FacingDirection);

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

            return new RoverBuilder(rover.Id).Operational(rover.Position.X,
                    rover.Position.Y,
                    _directionControl.GetNextDirection(rover.FacingDirection, command))
                .Build();
        }
    }
}
