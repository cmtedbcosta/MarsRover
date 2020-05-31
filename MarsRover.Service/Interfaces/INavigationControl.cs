using System.Collections.Generic;
using MarsRover.Models;

namespace MarsRover.Service.Interfaces
{
    public interface INavigationControl
    {
        Rover Navigate(Plateau plateau, Rover rover, IEnumerable<Command> commands, IEnumerable<Rover> otherRovers);
    }
}
