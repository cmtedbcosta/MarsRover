using System;
using System.Collections.Generic;
using FluentAssertions;
using MarsRover.Models;
using MarsRover.Service.Controls;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace MarsRover.Service.Test
{
    public class WhenNavigating
    {
        private readonly NavigationControl _navigationControl;

        public WhenNavigating()
        {
            var logger = new NullLoggerFactory().CreateLogger("Logger");
            var directionControl = new DirectionControl();
            var movementControl = new MovementControl();
            _navigationControl = new NavigationControl(movementControl, directionControl, logger);
        }

        public static IEnumerable<object[]> ValidInputForDirectionControl
        {
            get
            {
                var plateau = new Plateau(99, 99);
                var rover = new RoverBuilder(1).Operational( 1, 1, Direction.North).Build();

                var route1 = new[]
                {
                    Command.Move, Command.TurnRight, Command.Move, Command.TurnRight, Command.Move, Command.TurnRight,
                    Command.Move, Command.TurnRight
                };

                var route2 = new[]
                {
                    Command.Move, Command.TurnLeft, Command.Move, Command.TurnLeft, Command.Move, Command.TurnLeft,
                    Command.Move, Command.TurnLeft
                };

                var route3 = new[]
                {
                    Command.Move, Command.Move, Command.Move, Command.TurnLeft, Command.TurnLeft, Command.Move,
                    Command.Move, Command.Move, Command.TurnLeft, Command.TurnLeft
                };

                var route4 = new[]
                {
                    Command.TurnRight, //1,1 E
                    Command.Move,      //2,1 E
                    Command.TurnLeft,  //2,1 N
                    Command.Move,      //2,2 N
                    Command.Move,      //3,2 N
                    Command.TurnRight, //3,2 E
                    Command.Move,      //4,2 E
                    Command.Move,      //5,2 E
                    Command.TurnRight, //5,2 S
                    Command.Move,      //4,2 S
                    Command.TurnRight, //4,2 W
                    Command.Move,      //3,2 W
                    Command.Move,      //2,2 W
                };

                yield return new object[] {plateau, rover, route1, ((uint) 1, (uint) 1), Direction.North};
                yield return new object[] {plateau, rover, route2, ((uint) 1, (uint) 1), Direction.North};
                yield return new object[] {plateau, rover, route3, ((uint) 1, (uint) 1), Direction.North};
                yield return new object[] {plateau, rover, route4, ((uint) 2, (uint) 2), Direction.West};
            }
        }

        [Theory]
        [MemberData(nameof(ValidInputForDirectionControl))]
        public void GivenPlateauRoverAndCommands_ShouldNavigateToPosition_AndBeFacingDirection(Plateau plateau, Rover rover, Command[] commands, 
            (uint X, uint Y) expectedPosition, Direction expectedDirection)
        {
            var newRover = _navigationControl.Navigate(plateau, rover, commands, Array.Empty<Rover>());
            newRover.Position.Should().Be(expectedPosition);
            newRover.FacingDirection.Should().Be(expectedDirection);
            newRover.IsWaitingRescue.Should().BeFalse();
        }

        [Fact]
        public void GivenPlateauRover_AndCommandsThatSendTheRoverOutOfBounds_ShouldStopMovingAndBeWaitingForRescue()
        {
            var plateau = new Plateau(5, 5);
            var rover = new RoverBuilder(1).Operational(3, 3, Direction.North).Build();

            var commands = new[]
            {
                Command.Move, Command.Move, Command.Move, Command.Move, Command.Move, Command.Move, Command.Move
            };

            var expectedPosition = ((uint) 3, (uint) 5);
            var expectedDirection = Direction.North; 

            var newRover = _navigationControl.Navigate(plateau, rover, commands, Array.Empty<Rover>());
            newRover.Position.Should().Be(expectedPosition);
            newRover.FacingDirection.Should().Be(expectedDirection);
            newRover.IsWaitingRescue.Should().BeTrue();
        }

        [Fact]
        public void GivenPlateauRover_AndCommandsThatSendTheRoverInCollisionWithOtherRover_ShouldStopMovingAndBeWaitingForRescue()
        {
            var plateau = new Plateau(5, 5);
            var rover = new RoverBuilder(1).Operational(0, 0, Direction.North).Build();

            var commands = new[]
            {
                Command.Move, Command.Move, Command.Move, Command.Move
            };

            var otherRovers = new[] { new RoverBuilder(2).Operational( 0, 2, Direction.North).Build() };

            var expectedPosition = ((uint) 0, (uint) 1);
            var expectedDirection = Direction.North; 

            var newRover = _navigationControl.Navigate(plateau, rover, commands, otherRovers);
            newRover.Position.Should().Be(expectedPosition);
            newRover.FacingDirection.Should().Be(expectedDirection);
            newRover.IsWaitingRescue.Should().BeTrue();
            newRover.Error.Should().Contain("Collision detected");
        }

    }
}
