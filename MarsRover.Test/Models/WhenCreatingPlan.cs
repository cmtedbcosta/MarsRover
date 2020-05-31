using System;
using System.Collections.Generic;
using FluentAssertions;
using MarsRover.Models;
using Xunit;

namespace MarsRover.Test.Models
{
    public class WhenCreatingPlan
    {
        private Rover _rover;
        private Command[] _commands;
        private Plateau _plateau;
        private RoverRoute _roverRoute;

        private void Setup()
        {
            _rover = new RoverBuilder(1).Operational(1, 1, Direction.North).Build();
            _commands = new[] {Command.Move, Command.TurnRight, Command.Move};
            _plateau = new Plateau(5,5);
            _roverRoute = new RoverRoute(_rover, _commands);
        }

        [Fact]
        public void GivenValidInputForAPlan_ShouldCreateAPlan()
        {
            Setup();

            Action action = () =>
            {
                _ = new Plan(_plateau, new[] {_roverRoute}, Array.Empty<Rover>());
            };

            action.Should().NotThrow("Plan was correctly created.");
        }

        [Fact]
        public void GivenAPlanWithEmptyRoverRoutes_ShouldThrowInvalidOperationException()
        {
            Setup();

            Action action = () =>
            {
                _ = new Plan(_plateau, Array.Empty<RoverRoute>(), Array.Empty<Rover>());
            };

            action.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> InvalidInputForPlan
        {
            get
            {
                var plateau = new Plateau(5,5);
                var rover = new RoverBuilder(1).Operational(1, 1, Direction.North).Build();
                var commands = new[] {Command.Move, Command.TurnRight, Command.Move};
                var roverRoute = new RoverRoute(rover, commands);

                yield return new object[] { null, new[] { roverRoute }, Array.Empty<Rover>() };
                yield return new object[] { plateau, null, Array.Empty<Rover>() };
                yield return new object[] { plateau, new[] { roverRoute }, null };
            }                     
        }

        [Theory]
        [MemberData(nameof(InvalidInputForPlan))]
        public void GivenAPlanWithInvalidInput_ShouldThrowArgumentException(Plateau plateau, IEnumerable<RoverRoute> routes, IEnumerable<Rover> roversWithError)
        {
            Action action = () =>
            {
                _ = new Plan(plateau, routes, roversWithError);
            };

            action.Should().Throw<ArgumentException>();
        }
    }
}