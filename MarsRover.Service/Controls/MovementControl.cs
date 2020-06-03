using System;
using MarsRover.Models;
using MarsRover.Service.Interfaces;

namespace MarsRover.Service.Controls
{
    internal class MovementControl : IMovementControl
    {
        private const uint MovementUnit =  1;

        public (uint X, uint Y) GetNextPosition((uint X, uint Y) currentPosition, Direction facingDirection) =>
            facingDirection switch
            {
                { } direction when direction == Direction.North => (currentPosition.X, currentPosition.Y + MovementUnit),
                { } direction when direction == Direction.South => (currentPosition.X, currentPosition.Y - MovementUnit),
                { } direction when direction == Direction.East => (currentPosition.X + MovementUnit, currentPosition.Y),
                { } direction when direction == Direction.West => (currentPosition.X - MovementUnit, currentPosition.Y),
                null => throw new ArgumentNullException(nameof(facingDirection)),
                _ => throw new InvalidOperationException("Invalid direction")
            };
    }
}
