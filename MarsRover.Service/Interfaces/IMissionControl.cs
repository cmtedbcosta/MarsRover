using System.Collections.Generic;
using MarsRover.Models;

namespace MarsRover.Service.Interfaces
{
    public interface IMissionControl
    {
        IEnumerable<Rover> Execute(string command);
    }
}
