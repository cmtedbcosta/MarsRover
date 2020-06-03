using System;
using MarsRover.Models;
using MarsRover.Service.Interfaces;

namespace MarsRover.Service.Controls
{
    internal class DirectionControl : IDirectionControl
    {
        public Direction GetNextDirection(Direction currentDirection, Command currentCommand)
        {
            if (currentDirection == null) throw new ArgumentNullException(nameof(currentDirection));

            return currentCommand switch
            {
                { } command when command == Command.TurnRight => currentDirection.NextRight(),
                { } command when command == Command.TurnLeft => currentDirection.NextLeft(),
                null => throw new ArgumentNullException(nameof(currentCommand)),
                _ => throw new InvalidOperationException("Invalid command.")
            };
        }

    }
}
