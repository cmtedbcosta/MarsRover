using System;
using System.Collections.Generic;
using FluentAssertions;
using MarsRover.Models;
using Xunit;

namespace MarsRover.Test.Models
{
    public class WhenCreatingRoverRoute
    {
        public static IEnumerable<object[]> InvalidInputForRoverRoute
        {
            get
            {
                yield return new object[] { null, new[] {Command.Move, Command.TurnRight, Command.Move,} };
                yield return new object[] { new RoverBuilder(1).Operational( 1, 1, Direction.North).Build(), null };
            }                     
        }

        [Theory]
        [MemberData(nameof(InvalidInputForRoverRoute))]
        public void GivenARoverRouteWithInvalidInput_ShouldThrowArgumentException(Rover rover, IEnumerable<Command> commands)
        {
            Action action = () =>
            {
                _ = new RoverRoute(rover, commands);
            };

            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GivenValidInputForARoverRoute_ShouldCreateARoverRoute()
        {
            var rover = new RoverBuilder(1).Operational(1, 1, Direction.North).Build();
            var commands = new[] {Command.Move, Command.TurnRight, Command.Move,};

            Action action = () =>
            {
                _ = new RoverRoute(rover, commands);
            };

            action.Should().NotThrow("Rover route was correctly created");
        }

        [Fact]
        public void GivenARoverRouteWithEmptyCommands_ShouldThrowInvalidOperationException()
        {
            var rover = new RoverBuilder(1).Operational( 1, 1, Direction.North).Build();
            var commands = Array.Empty<Command>();

            Action action = () =>
            {
                _ = new RoverRoute(rover, commands);
            };

            action.Should().Throw<InvalidOperationException>();
        }
    }
}