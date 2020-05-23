using System;
using MarsRover.Models;

namespace MarsRover.Service.Controls
{
    internal class DirectionControl
    {
        private readonly Direction _currentDirection;
        private readonly Command _command;

        public DirectionControl(Direction currentDirection, Command command)
        {
            _currentDirection = currentDirection ?? throw new ArgumentNullException(nameof(currentDirection));
            _command = command ?? throw new ArgumentNullException(nameof(command));
        }

        public Direction GetNextDirection()
        {
            if (_command == Command.Move)
                throw new InvalidOperationException("Direction control does not support move command");
            
            return _command == Command.TurnLeft ? _currentDirection.Previous() : _currentDirection.Next();
        }

    }
}
