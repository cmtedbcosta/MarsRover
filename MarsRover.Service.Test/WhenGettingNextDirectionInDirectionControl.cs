using System;
using System.Collections.Generic;
using FluentAssertions;
using MarsRover.Models;
using MarsRover.Service.Controls;
using Xunit;

namespace MarsRover.Service.Test
{
    public class WhenGettingNextDirectionInDirectionControl
    {
    
        public static IEnumerable<object[]> ValidInputForDirectionControl
        {
            get
            {
                yield return new object[] { Direction.North, Command.TurnRight, Direction.East };
                yield return new object[] { Direction.North, Command.TurnLeft, Direction.West };


                yield return new object[] { Direction.East, Command.TurnRight, Direction.South };
                yield return new object[] { Direction.East, Command.TurnLeft, Direction.North };

                yield return new object[] { Direction.South, Command.TurnRight, Direction.West };
                yield return new object[] { Direction.South, Command.TurnLeft, Direction.East };

                yield return new object[] { Direction.West, Command.TurnRight, Direction.North };
                yield return new object[] { Direction.West, Command.TurnLeft, Direction.South };
            }                     
        }

        [Theory]
        [MemberData(nameof(ValidInputForDirectionControl))]
        public void GivenCurrentDirection_AndAValidCommand_ShouldOutputCorrectDirection(Direction currentDirection, Command command, Direction nextDirection)
        {
            var directionControl = new DirectionControl(currentDirection, command);
            var nextCalculatedDirection = directionControl.GetNextDirection();
            nextCalculatedDirection.Should().Be(nextDirection);
        }

        private Direction GetRandomDirection()
        {
            var random = new Random();
            var index = random.Next(Direction.All.Count);
            return Direction.All[index];
        }

        [Fact]
        public void GivenARandomDirection_WithAMoveCommand_ShouldThrowInvalidException()
        {
            var directionControl = new DirectionControl(GetRandomDirection(), Command.Move);
            Action action = () =>  directionControl.GetNextDirection();
            action.Should().Throw<InvalidOperationException>();
        }
    }
}
