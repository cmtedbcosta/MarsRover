using MarsRover.Models;

namespace MarsRover.Service.Interfaces
{
    internal interface IMovementControl
    {
        (uint X, uint Y) GetNextPosition((uint X, uint Y) currentPosition, Direction facingDirection);
    }
}
