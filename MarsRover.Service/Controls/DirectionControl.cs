using System;
using MarsRover.Models;
using MarsRover.Service.Interfaces;

namespace MarsRover.Service.Controls
{
    internal class DirectionControl : IDirectionControl
    {
        public Direction GetNextDirection(Direction currentDirection, Command command)
        {
            if (currentDirection == null) throw new ArgumentNullException(nameof(currentDirection));
            if (command == null) throw new ArgumentNullException(nameof(command));

            if (command == Command.Move)
                throw new InvalidOperationException("Direction control does not support move command");
            
            return command == Command.TurnLeft ? currentDirection.Previous() : currentDirection.Next();
        }

    }
}
