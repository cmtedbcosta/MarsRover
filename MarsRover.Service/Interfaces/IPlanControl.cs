using MarsRover.Models;

namespace MarsRover.Service.Interfaces
{
    public interface IPlanControl
    {
        Plan GeneratePlan(string command);
    }
}
