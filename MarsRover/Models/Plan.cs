using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRover.Models
{
    public class Plan
    {
        public Plan(Plateau plateau, IEnumerable<RoverRoute> roverRoutes, IEnumerable<Rover> roversWithError)
        {
            Plateau = plateau ?? throw new ArgumentNullException(nameof(plateau));
            RoverRoutes = roverRoutes ?? throw new ArgumentNullException(nameof(roverRoutes));
            if (!roverRoutes.Any())
                throw new ArgumentException("Plan must contain at least 1 rover route", nameof(roverRoutes));
            RoversWithError = roversWithError ?? throw new ArgumentNullException(nameof(roversWithError));
        }

        public Plateau Plateau { get;  }

        public IEnumerable<RoverRoute> RoverRoutes { get; }

        public IEnumerable<Rover> RoversWithError { get; }
    }
}
