using System;
using System.Collections.Generic;
using FluentAssertions;
using MarsRover.Models;
using Xunit;

namespace MarsRover.Test.Models
{
    public class WhenBuildingARover
    {
        private const uint SomeId = 1;

        [Fact]
        public void GivenValidInput_ForANotDeployedRover_ShouldBuild_AndRoverShouldWaitForRescue()
        {
            Action action = () =>
            {
                var rover = new RoverBuilder(SomeId).NotDeployed("Some error").Build();
                rover.IsWaitingRescue.Should().BeTrue();
            };

            action.Should().NotThrow("Rover was correctly built.");
        }

        public static IEnumerable<object[]> InvalidInputForANotDeployedRover
        {
            get
            {
                yield return new object[] { null };
                yield return new object[] { string.Empty };
            }                     
        }

        [Theory]
        [MemberData(nameof(InvalidInputForANotDeployedRover))]
        public void GivenInvalidInput_ForANotDeployedRover_ShouldNotBuild(string error)
        {
            Action action = () =>
            {
                _ = new RoverBuilder(SomeId).NotDeployed(error).Build();
            };

            action.Should().Throw<ArgumentNullException>();
        }

        public static IEnumerable<object[]> ValidInputForAnOperationalRover
        {
            get
            {
                yield return new object[] { 0, 0, Direction.North };
                yield return new object[] { 1, 1, Direction.North };
                yield return new object[] { 0, 1, Direction.East };
                yield return new object[] { 1, 0, Direction.West };
                yield return new object[] { 99, 99, Direction.South };
            }                     
        }

        [Theory]
        [MemberData(nameof(ValidInputForAnOperationalRover))]
        public void GivenValidInput_ForAnOperationalRover_ShouldBuild_AndNotWaitRescue(uint positionX, uint positionY, Direction facingDirection)
        {
            Action action = () =>
            {
                var rover = new RoverBuilder(SomeId).Operational(positionX, positionY, facingDirection).Build();
                rover.IsWaitingRescue.Should().BeFalse();
            };

            action.Should().NotThrow("Rover was correctly built.");
        }

        [Fact]
        public void GivenInvalidInput_ForAnOperationalRover_ShouldNotBuild()
        {
            Action action = () =>
            {
                _ = new RoverBuilder(SomeId).Operational(1, 1, null).Build();
            };

            action.Should().Throw<ArgumentNullException>();
        }

        public static IEnumerable<object[]> ValidInputForStoppedBeforeCrashRover
        {
            get
            {
                yield return new object[] { 0, 0, Direction.North, "Some Error" };
                yield return new object[] { 1, 1, Direction.North, "Some Error" };
                yield return new object[] { 0, 1, Direction.East, "Some Error" };
                yield return new object[] { 1, 0, Direction.West, "Some Error" };
                yield return new object[] { 99, 99, Direction.South, "Some Error" };
            }                     
        }

        [Theory]
        [MemberData(nameof(ValidInputForStoppedBeforeCrashRover))]
        public void GivenValidInput_ForAStoppedBeforeCrashRover_ShouldBuild_AndRoverShouldWaitForRescue(uint positionX, uint positionY, Direction facingDirection, string error)
        {
            Action action = () =>
            {
                var rover = new RoverBuilder(SomeId).StoppedBeforeCrash(positionX, positionY, facingDirection, error).Build();
                rover.IsWaitingRescue.Should().BeTrue();
            };

            action.Should().NotThrow("Rover was correctly built.");
        }

        public static IEnumerable<object[]> InvalidInputForAStoppedBeforeCrashRover
        {
            get
            {
                yield return new object[] { null, null };
                yield return new object[] { null, string.Empty };
                yield return new object[] { Direction.North, string.Empty };
                yield return new object[] { Direction.North, null };
                yield return new object[] { null, "Some Error" };
            }                     
        }

        [Theory]
        [MemberData(nameof(InvalidInputForAStoppedBeforeCrashRover))]
        public void GivenInvalidInput_ForAStoppedBeforeCrashRover_ShouldNotBuild(Direction facingDirection, string error)
        {
            Action action = () =>
            {
                _ = new RoverBuilder(SomeId).StoppedBeforeCrash(1, 1, facingDirection, error).Build();
            };

            action.Should().Throw<ArgumentNullException>();
        }
    }
}
