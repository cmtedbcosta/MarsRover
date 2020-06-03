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
            RoversWithError = roversWithError ?? throw new ArgumentNullException(nameof(roversWithError));

            if (!roverRoutes.Any() && !roversWithError.Any())
                throw new ArgumentException("Plan must contain at least one rover routed or with error", nameof(roverRoutes));
        }

        public Plateau Plateau { get;  }

        public IEnumerable<RoverRoute> RoverRoutes { get; }

        public IEnumerable<Rover> RoversWithError { get; }
    }
}
