using System;
using System.Collections.Generic;
using System.Linq;
using MarsRover.Models;

namespace MarsRover.Service.Controls
{
    internal class PlanControl
    {
        private readonly string _command;
        private const char LineSeparator = '\r';
        private const char ParameterSeparator = ' ';

        public PlanControl(string command)
        {
            if (string.IsNullOrEmpty(command?.Trim())) throw new ArgumentNullException(nameof(command));
            _command = command.Trim();
        }

        public Plan GeneratePlan()
        {
            var lines = _command.Split(LineSeparator);

            // First line should always be the plateau
            var plateauParameters = lines[0].Trim().Split(ParameterSeparator);

            if (plateauParameters.Length != 2)
                throw new ArgumentException("Invalid amount of plateau parameters provided in command");

            if (!(uint.TryParse(plateauParameters[0].Trim(), out var plateauX)) || !(uint.TryParse(plateauParameters[1].Trim(), out var plateauY)))
                throw new ArgumentException("Invalid plateau provided in command");

            var plateau = new Plateau(plateauX, plateauY);

            var roverRoutes = new List<RoverRoute>();
            var roversWithError = new List<Rover>();
            uint currentSequence = 1;

            for (var i = 1; i < lines.Length; i++)
            {
                var roverCreated = false;

                try
                {
                    var roverParameters = lines[i].Trim().Split(ParameterSeparator);

                    if (roverParameters.Length != 3)
                        throw new ArgumentException("Invalid amount of rover parameters provided in command");

                    if (!(uint.TryParse(roverParameters[0].Trim(), out var roverX)) || !(uint.TryParse(roverParameters[1].Trim(), out var roverY)))
                        throw new ArgumentException("Invalid rover provided in command");

                    var roverDirection = roverParameters[2].Trim();

                    var rover = new RoverBuilder(currentSequence).Operational(roverX, roverY, Direction.FromCode(roverDirection)).Build();
                    currentSequence++;

                    roverCreated = true;

                    // Move to next line
                    i++;

                    var commandParameters = lines[i].Trim().Replace(" ", "");

                    var commands = commandParameters.Select(Command.FromCode).ToArray();

                    roverRoutes.Add(new RoverRoute(rover, commands));
                }
                catch (Exception e)
                {
                    // If the rover was already created, get last one
                    roversWithError.Add(roverCreated
                        ? new RoverBuilder(currentSequence - 1).NotDeployed($"Rover {currentSequence - 1} has invalid commands." + Environment.NewLine + e.Message).Build()
                        : new RoverBuilder(currentSequence).NotDeployed($"Rover {currentSequence} has invalid parameters." + Environment.NewLine + e.Message).Build());

                    if (roverCreated) continue;

                    // If the exception was on Rover creation, skip command line and increase sequence
                    i++;
                    currentSequence++;
                }
            }

            return new Plan(plateau, roverRoutes, roversWithError);
        }
    }
}
