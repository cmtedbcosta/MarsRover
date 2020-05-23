using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRover.Models
{
    public class RoverRoute
    {
        public RoverRoute(Rover rover, IEnumerable<Command> commands)
        {
            Rover = rover ?? throw new ArgumentNullException(nameof(rover));
            Commands = commands ?? throw new ArgumentNullException(nameof(commands));

            if (!commands.Any())
                throw new InvalidOperationException("Rover route must contain at least 1 command");
        }

        public Rover Rover { get; }
        public IEnumerable<Command> Commands { get; }
    }
}
