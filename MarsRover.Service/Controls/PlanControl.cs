using System;
using System.Collections.Generic;
using System.Linq;
using MarsRover.Models;
using MarsRover.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace MarsRover.Service.Controls
{
    internal class PlanControl : IPlanControl
    {
        private readonly ILogger _logger;
        private const char LineSeparator = '\r';
        private const char ParameterSeparator = ' ';

        public PlanControl(ILogger logger)
        {
            _logger = logger;
        }

        public Plan GeneratePlan(string command)
        {
            if (string.IsNullOrEmpty(command?.Trim())) throw new ArgumentNullException(nameof(command));

            var lines = command.Trim().Split(LineSeparator);

            // First line should always be the plateau
            _logger.LogDebug($"Processing line[0]: {lines[0].Trim()}");

            var plateauParameters = lines[0].Trim().Split(ParameterSeparator);

            if (plateauParameters.Length != 2)
            {
                _logger.LogDebug($"Invalid amount of plateau parameters provided in command: {plateauParameters.Length}");
                throw new ArgumentException("Invalid amount of plateau parameters provided in command");
            }

            if (!(uint.TryParse(plateauParameters[0].Trim(), out var plateauX)) ||
                !(uint.TryParse(plateauParameters[1].Trim(), out var plateauY)))
            {
                _logger.LogDebug($"Invalid plateau provided in command: ({plateauParameters[0].Trim()},{plateauParameters[1].Trim()})");
                throw new ArgumentException("Invalid plateau provided in command");
            }

            var plateau = new Plateau(plateauX, plateauY);

            var roverRoutes = new List<RoverRoute>();
            var roversWithError = new List<Rover>();
            uint currentSequence = 1;

            for (var i = 1; i < lines.Length; i++)
            {
                var roverCreated = false;

                _logger.LogDebug($"Processing line[{i}]: {lines[i].Trim()}");

                try
                {
                    var roverParameters = lines[i].Trim().Split(ParameterSeparator);

                    if (roverParameters.Length != 3)
                    {
                        _logger.LogDebug($"Invalid amount of rover parameters provided in command: {roverParameters.Length}");
                        throw new ArgumentException("Invalid amount of rover parameters provided in command");
                    }


                    if (!(uint.TryParse(roverParameters[0].Trim(), out var roverX)) ||
                        !(uint.TryParse(roverParameters[1].Trim(), out var roverY)))
                    {
                        _logger.LogDebug($"Invalid rover provided in command: ({roverParameters[0].Trim()},{roverParameters[1].Trim()})");
                        throw new ArgumentException("Invalid rover provided in command");
                    }

                    var roverDirection = roverParameters[2].Trim();

                    var rover = new RoverBuilder(currentSequence).Operational(roverX, roverY, Direction.FromCode(roverDirection)).Build();
                    currentSequence++;

                    _logger.LogDebug($"{rover.Name} created at position {rover.Position} facing {rover.FacingDirection.Name}");
                    roverCreated = true;

                    // Move to next line
                    i++;

                    var commandParameters = lines[i].Trim().Replace(" ", "");

                    var commands = commandParameters.Select(Command.FromCode).ToArray();

                    roverRoutes.Add(new RoverRoute(rover, commands));

                    _logger.LogDebug($"Commands {string.Join(",",commands.Select(c => c.Name))} added to {rover.Name} route.");
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
