using System;
using System.Collections.Generic;
using FluentAssertions;
using MarsRover.Models;
using Xunit;

namespace MarsRover.Test.Models
{
    public class WhenCreatingRover
    {
        public static IEnumerable<object[]> ValidInputForARover
        {
            get
            {
                yield return new object[] { 0, 0, Direction.North, true, true, string.Empty };
                yield return new object[] { 1, 0, Direction.South, true, true, string.Empty };
                yield return new object[] { 0, 1, Direction.East, true, true, string.Empty };
                yield return new object[] { 1, 1, Direction.West, true, true, string.Empty };

                yield return new object[] { 0, 0, Direction.North, false, true, string.Empty };
                yield return new object[] { 1, 0, Direction.South, false, true, string.Empty };
                yield return new object[] { 0, 1, Direction.East, false, true, string.Empty };
                yield return new object[] { 1, 1, Direction.West, false, true, string.Empty };

                yield return new object[] { 0, 0, Direction.North, true, true, "Some Error" };
                yield return new object[] { 1, 0, Direction.South, true, true, "Some Error" };
                yield return new object[] { 0, 1, Direction.East, true, true, "Some Error" };
                yield return new object[] { 1, 1, Direction.West, true, true, "Some Error" };

                yield return new object[] { 0, 0, Direction.North, false, true, "Some Error" };
                yield return new object[] { 1, 0, Direction.South, false, true, "Some Error" };
                yield return new object[] { 0, 1, Direction.East, false, true, "Some Error" };
                yield return new object[] { 1, 1, Direction.West, false, true, "Some Error" };

                yield return new object[] { 1, 1, null, false, true, "Some Error" };
                yield return new object[] { 1, 1, Direction.West, false, true, null };

                yield return new object[] { 0, 0, Direction.North, true, false, string.Empty };
                yield return new object[] { 1, 0, Direction.South, true, false, string.Empty };
                yield return new object[] { 0, 1, Direction.East, true, false, string.Empty };
                yield return new object[] { 1, 1, Direction.West, true, false, string.Empty };

                yield return new object[] { 0, 0, Direction.North, false, false, string.Empty };
                yield return new object[] { 1, 0, Direction.South, false, false, string.Empty };
                yield return new object[] { 0, 1, Direction.East, false, false, string.Empty };
                yield return new object[] { 1, 1, Direction.West, false, false, string.Empty };

                yield return new object[] { 0, 0, Direction.North, true, false, "Some Error" };
                yield return new object[] { 1, 0, Direction.South, true, false, "Some Error" };
                yield return new object[] { 0, 1, Direction.East, true, false, "Some Error" };
                yield return new object[] { 1, 1, Direction.West, true, false, "Some Error" };

                yield return new object[] { 0, 0, Direction.North, false, false, "Some Error" };
                yield return new object[] { 1, 0, Direction.South, false, false, "Some Error" };
                yield return new object[] { 0, 1, Direction.East, false, false, "Some Error" };
                yield return new object[] { 1, 1, Direction.West, false, false, "Some Error" };

                yield return new object[] { 1, 1, null, false, false, "Some Error" };
                yield return new object[] { 1, 1, Direction.West, false, true, null };
            }
        }

        [Theory]
        [MemberData(nameof(ValidInputForARover))]
        public void GivenValidInputForARover_ShouldCreateARover(uint positionX, uint positionY, Direction direction, bool isWaitingForRescue, bool isStoppedBeforeCollision, string error)
        {
            Action action = () =>
            {
                var rover = new Rover(1, positionX, positionY, direction, isWaitingForRescue, isStoppedBeforeCollision, error);
                rover.Position.Should().Be((positionX, positionY));
                rover.FacingDirection.Should().Be(direction);
                rover.Error.Should().Be(error);
                rover.IsWaitingRescue.Should().Be(isWaitingForRescue);
            };
            action.Should().NotThrow("Rover was correctly created.");
        }
    }
}