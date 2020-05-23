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
                yield return new object[] { new Rover(1, 1, 1, Direction.North), null };
            }                     
        }

        [Theory]
        [MemberData(nameof(InvalidInputForRoverRoute))]
        public void GivenARoverRouteWithInvalidInput_ShouldThrowArgumentException(Rover rover, IEnumerable<Command> commands)
        {
            Action action = () => new RoverRoute(rover, commands);
            action.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GivenValidInputForARoverRoute_ShouldCreateARoverRoute()
        {
            var rover = new Rover(1, 1, 1, Direction.North);
            var commands = new[] {Command.Move, Command.TurnRight, Command.Move,};

            Action action = () => new RoverRoute(rover, commands);
            action.Should().NotThrow("Rover route was correctly created");
        }

        [Fact]
        public void GivenARoverRouteWithEmptyCommands_ShouldThrowInvalidOperationException()
        {
            var rover = new Rover(1, 1, 1, Direction.North);
            var commands = Array.Empty<Command>();

            Action action = () => new RoverRoute(rover, commands);
            action.Should().Throw<InvalidOperationException>();
        }
    }
}