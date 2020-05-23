using System.Collections.Generic;
using MarsRover.Models;

namespace MarsRover.Ports
{
    public interface IMissionControl
    {
        IEnumerable<Rover> Execute(string command);
    }
}
