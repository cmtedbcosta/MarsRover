using System;
using System.Collections.Generic;
using System.Linq;
using MarsRover.Models;

namespace MarsRover.Service.Controls
{
    internal class NavigationControl
    {
        private readonly Plateau _plateau;
        private readonly Rover _rover;
        private readonly IEnumerable<Command> _commands;
        private readonly IEnumerable<Rover> _otherRovers;

        public NavigationControl(Plateau plateau, Rover rover, IEnumerable<Command> commands, IEnumerable<Rover> otherRovers)
        {
            _plateau = plateau ?? throw new ArgumentNullException(nameof(plateau));
            _rover = rover ?? throw new ArgumentNullException(nameof(rover));
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
            _otherRovers = otherRovers ?? throw new ArgumentNullException(nameof(otherRovers));
        }

        private static bool IsOutOfBounds((uint X, uint Y) position, Plateau plateau) =>
            position.X > plateau.MaxSizeX || position.Y > plateau.MaxSizeY;

        private static bool IsSamePositionOfOtherRover((uint X, uint Y) position, IEnumerable<Rover> otherRovers)
        {
            var occupiedPositions = otherRovers.Select(r => r.Position);
            return occupiedPositions.Contains(position);
        }

        public Rover Navigate() => _commands.Aggregate(_rover, Goto);

        private Rover Goto(Rover rover, Command command)
        {
            if (rover.IsWaitingRescue)
                return rover;

            if (command == Command.Move)
            {
                var movementControl = new MovementControl(rover.Position, rover.FacingDirection);
                var nextPosition = movementControl.GetNextPosition();

                if (IsSamePositionOfOtherRover(nextPosition, _otherRovers))
                    return new Rover(rover.Id, rover.Position.X, rover.Position.Y, rover.FacingDirection,  $"Collision detected on {nextPosition}.");

                return IsOutOfBounds(nextPosition,
                    _plateau)
                    ? new Rover(rover.Id,
                        rover.Position.X,
                        rover.Position.Y,
                        rover.FacingDirection,
                        true)
                    : new Rover(rover.Id,
                        nextPosition.X,
                        nextPosition.Y,
                        rover.FacingDirection);
            }

            var directionControl = new DirectionControl(rover.FacingDirection, command);
            return new Rover(rover.Id, rover.Position.X, rover.Position.Y, directionControl.GetNextDirection());
        }
    }
}
