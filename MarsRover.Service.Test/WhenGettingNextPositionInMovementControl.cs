using System;
using System.Collections.Generic;
using FluentAssertions;
using MarsRover.Models;
using MarsRover.Service.Controls;
using Xunit;

namespace MarsRover.Service.Test
{
    public class WhenGettingNextPositionInMovementControl
    {
        public static IEnumerable<object[]> ValidInputForDirectionControl
        {
            get
            {
                yield return new object[] { ((uint) 1, (uint) 1), Direction.North, ((uint) 1, (uint) 2) };
                yield return new object[] { ((uint) 1, (uint) 1), Direction.South, ((uint) 1, (uint) 0) };
                yield return new object[] { ((uint) 1, (uint) 1), Direction.East, ((uint) 2, (uint) 1) };
                yield return new object[] { ((uint) 1, (uint) 1), Direction.West, ((uint) 0, (uint) 1) };
            }                     
        }

        [Theory]
        [MemberData(nameof(ValidInputForDirectionControl))]
        public void GivenCurrentPosition_AndADirection_ShouldOutputCorrectNextPosition((uint X, uint Y) position, Direction direction, (uint X, uint Y) nextPosition)
        {
            var movementControl = new MovementControl(position, direction);
            var nextCalculatedPosition = movementControl.GetNextPosition();
            nextCalculatedPosition.Should().Be(nextPosition);
        }

        [Fact]
        public void GivenInvalidInputForMovementControl_ShouldThrowArgumentNullException()
        {
            Action action = () => new MovementControl(((uint) 1, (uint) 1), null);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}
