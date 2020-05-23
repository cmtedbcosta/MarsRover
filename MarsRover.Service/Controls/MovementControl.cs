using System;
using MarsRover.Models;

namespace MarsRover.Service.Controls
{
    internal class MovementControl
    {
        private readonly (uint X, uint Y) _currentPosition;
        private readonly Direction _facingDirection;
        private const uint MovementUnit =  1;

        public MovementControl((uint, uint) currentPosition, Direction facingDirection)
        {
            _currentPosition = currentPosition;
            _facingDirection = facingDirection ?? throw new ArgumentNullException(nameof(facingDirection));
        }

        public (uint X, uint Y) GetNextPosition()
        {
            if (_facingDirection == Direction.North)
            {
                return (_currentPosition.X, _currentPosition.Y + MovementUnit);
            }

            if (_facingDirection == Direction.South)
            {
                return (_currentPosition.X, _currentPosition.Y - MovementUnit);
            }

            return _facingDirection == Direction.East
                ? (_currentPosition.X + MovementUnit, _currentPosition.Y)
                // West
                : (_currentPosition.X - MovementUnit, _currentPosition.Y);
        }
    }
}
