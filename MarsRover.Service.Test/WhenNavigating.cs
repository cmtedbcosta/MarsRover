using System;
using System.Collections.Generic;
using FluentAssertions;
using MarsRover.Models;
using MarsRover.Service.Controls;
using Xunit;

namespace MarsRover.Service.Test
{
    public class WhenNavigating
    {
        public static IEnumerable<object[]> ValidInputForDirectionControl
        {
            get
            {
                var plateau = new Plateau(99, 99);
                var rover = new Rover(1, 1, 1, Direction.North);

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
            var navigationControl = new NavigationControl(plateau, rover, commands, Array.Empty<Rover>());
            var newRover = navigationControl.Navigate();
            newRover.Position.Should().Be(expectedPosition);
            newRover.FacingDirection.Should().Be(expectedDirection);
            newRover.IsWaitingRescue.Should().BeFalse();
        }

        [Fact]
        public void GivenPlateauRover_AndCommandsThatSendTheRoverOutOfBounds_ShouldStopMovingAndBeWaitingForRescue()
        {
            var plateau = new Plateau(5, 5);
            var rover = new Rover(1, 3, 3, Direction.North);

            var commands = new[]
            {
                Command.Move, Command.Move, Command.Move, Command.Move, Command.Move, Command.Move, Command.Move
            };

            var expectedPosition = ((uint) 3, (uint) 5);
            var expectedDirection = Direction.North; 

            var navigationControl = new NavigationControl(plateau, rover, commands, Array.Empty<Rover>());
            var newRover = navigationControl.Navigate();
            newRover.Position.Should().Be(expectedPosition);
            newRover.FacingDirection.Should().Be(expectedDirection);
            newRover.IsWaitingRescue.Should().BeTrue();
        }

        [Fact]
        public void GivenPlateauRover_AndCommandsThatSendTheRoverInCollisionWithOtherRover_ShouldStopMovingAndBeWaitingForRescue()
        {
            var plateau = new Plateau(5, 5);
            var rover = new Rover(1, 0, 0, Direction.North);

            var commands = new[]
            {
                Command.Move, Command.Move, Command.Move, Command.Move
            };

            var otherRovers = new[] {new Rover(2, 0, 2, Direction.North)};

            var expectedPosition = ((uint) 0, (uint) 1);
            var expectedDirection = Direction.North; 

            var navigationControl = new NavigationControl(plateau, rover, commands, otherRovers);
            var newRover = navigationControl.Navigate();
            newRover.Position.Should().Be(expectedPosition);
            newRover.FacingDirection.Should().Be(expectedDirection);
            newRover.IsWaitingRescue.Should().BeTrue();
            newRover.Error.Should().Contain("Collision detected");
        }

    }
}
