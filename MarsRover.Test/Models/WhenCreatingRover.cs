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
                yield return new object[] { 0,0, Direction.North };
                yield return new object[] { 1,0, Direction.South };
                yield return new object[] { 0,1, Direction.East };
                yield return new object[] { 1,1, Direction.West };
            }                     
        }

        [Theory]
        [MemberData(nameof(ValidInputForARover))]
        public void GivenValidInputForARover_ShouldCreateARover(uint positionX, uint positionY, Direction direction)
        {
            Action action = () => new Rover(1, positionX, positionY, direction);
            action.Should().NotThrow("Rover was correctly created.");
            var rover = new Rover(1, positionX, positionY, direction);
            rover.Position.Should().Be((positionX, positionY));
            rover.FacingDirection.Should().Be(direction);
        }

        [Theory]
        [InlineData(0,0)]
        [InlineData(1,0)]
        [InlineData(0,1)]
        [InlineData(99,99)]
        public void GivenInputMissingDirectionForARover_ShouldThrowArgumentException(uint positionX, uint positionY)
        {
            Action action = () => new Rover(1, positionX, positionY, null);
            action.Should().Throw<ArgumentException>("Invalid input was provided");
        }

        [Fact]
        public void GivenValidInputForErrorRover_ShouldNotThrowException()
        {
            Action action = () => new Rover(1, "some error");
            action.Should().NotThrow("Rover was correctly created");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void GivenInValidInputForErrorRover_ShouldThrowArgumentNullException(string error)
        {
            Action action = () =>new Rover(1, error);
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GivenValidInputForCollisionRover_ShouldNotThrowException()
        {
            Action action = () => new Rover(1, 1, 1, Direction.East, "collision");
            action.Should().NotThrow("Rover was correctly created");
        }

        public static IEnumerable<object[]> InvalidInputForARover
        {
            get
            {
                yield return new object[] { Direction.North, "" };
                yield return new object[] { Direction.North, null };
                yield return new object[] { null, "some error" };
            }                     
        }

        [Theory]
        [MemberData(nameof(InvalidInputForARover))]
        public void GivenInValidInputForCollisionRover_ShouldThrowArgumentNullException(Direction direction, string error)
        {
            Action action = () => new Rover(1, 1, 1, direction, error);
            action.Should().Throw<ArgumentNullException>();
        }
    }
}