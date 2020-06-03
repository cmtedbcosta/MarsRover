using MarsRover.Models;

namespace MarsRover.Service.Interfaces
{
    internal interface IDirectionControl
    {
        Direction GetNextDirection(Direction currentDirection, Command currentCommand);
    }
}
