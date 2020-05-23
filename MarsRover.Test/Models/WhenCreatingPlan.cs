using System;
using System.Collections.Generic;
using FluentAssertions;
using MarsRover.Models;
using Xunit;

namespace MarsRover.Test.Models
{
    public class WhenCreatingPlan
    {
        [Fact]
        public void GivenValidInputForAPlan_ShouldCreateAPlan()
        {
            var rover = new Rover(1, 1, 1, Direction.North);
            var commands = new[] {Command.Move, Command.TurnRight, Command.Move};
            var roverRoute = new RoverRoute(rover, commands);
            var plateau = new Plateau(5,5);

            Action action = () => new Plan(plateau, new [] {roverRoute}, Array.Empty<Rover>());
            action.Should().NotThrow("Plan was correctly created.");
        }

        [Fact]
        public void GivenAPlanWithEmptyRoverRoutes_ShouldThrowInvalidOperationException()
        {
            var plateau = new Plateau(5,5);

            Action action = () => new Plan(plateau, Array.Empty<RoverRoute>(), Array.Empty<Rover>());
            action.Should().Throw<ArgumentException>();
        }

        public static IEnumerable<object[]> InvalidInputForPlan
        {
            get
            {
                var plateau = new Plateau(5,5);
                var rover = new Rover(1, 1, 1, Direction.North);
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
            Action action = () => new Plan(plateau, routes, roversWithError);
            action.Should().Throw<ArgumentException>();
        }
    }
}