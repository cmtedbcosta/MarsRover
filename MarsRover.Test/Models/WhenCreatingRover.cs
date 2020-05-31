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
                yield return new object[] { 0, 0, Direction.North, true, string.Empty };
                yield return new object[] { 1, 0, Direction.South, true, string.Empty };
                yield return new object[] { 0, 1, Direction.East, true, string.Empty };
                yield return new object[] { 1, 1, Direction.West, true, string.Empty };

                yield return new object[] { 0, 0, Direction.North, false, string.Empty };
                yield return new object[] { 1, 0, Direction.South, false, string.Empty };
                yield return new object[] { 0, 1, Direction.East, false, string.Empty };
                yield return new object[] { 1, 1, Direction.West, false, string.Empty };

                yield return new object[] { 0, 0, Direction.North, true, "Some Error" };
                yield return new object[] { 1, 0, Direction.South, true, "Some Error" };
                yield return new object[] { 0, 1, Direction.East, true, "Some Error" };
                yield return new object[] { 1, 1, Direction.West, true, "Some Error" };

                yield return new object[] { 0, 0, Direction.North, false, "Some Error" };
                yield return new object[] { 1, 0, Direction.South, false, "Some Error" };
                yield return new object[] { 0, 1, Direction.East, false, "Some Error" };
                yield return new object[] { 1, 1, Direction.West, false, "Some Error" };

                yield return new object[] { 1, 1, null, false, "Some Error" };
                yield return new object[] { 1, 1, Direction.West, false, null };
            }
        }

        [Theory]
        [MemberData(nameof(ValidInputForARover))]
        public void GivenValidInputForARover_ShouldCreateARover(uint positionX, uint positionY, Direction direction, bool isWaitingForRescue, string error)
        {
            Action action = () =>
            {
                var rover = new Rover(1, positionX, positionY, direction, isWaitingForRescue, error);
                rover.Position.Should().Be((positionX, positionY));
                rover.FacingDirection.Should().Be(direction);
                rover.Error.Should().Be(error);
                rover.IsWaitingRescue.Should().Be(isWaitingForRescue);
            };
            action.Should().NotThrow("Rover was correctly created.");
        }
    }
}