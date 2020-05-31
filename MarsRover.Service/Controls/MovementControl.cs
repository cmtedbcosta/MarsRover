using System;
using MarsRover.Models;
using MarsRover.Service.Interfaces;

namespace MarsRover.Service.Controls
{
    internal class MovementControl : IMovementControl
    {
        private const uint MovementUnit =  1;

        public (uint X, uint Y) GetNextPosition((uint X, uint Y) currentPosition, Direction facingDirection)
        {
            if (facingDirection == null) throw new ArgumentNullException(nameof(facingDirection));

            if (facingDirection == Direction.North)
            {
                return (currentPosition.X, currentPosition.Y + MovementUnit);
            }

            if (facingDirection == Direction.South)
            {
                return (currentPosition.X, currentPosition.Y - MovementUnit);
            }

            return facingDirection == Direction.East
                ? (currentPosition.X + MovementUnit, currentPosition.Y)
                : (currentPosition.X - MovementUnit, currentPosition.Y);
        }
    }
}
