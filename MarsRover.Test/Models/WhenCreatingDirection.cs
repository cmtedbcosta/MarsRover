using System;
using FluentAssertions;
using MarsRover.Models;
using Xunit;

namespace MarsRover.Test.Models
{
    public class WhenCreatingDirection
    {
        [Theory]
        [InlineData("N")]
        [InlineData("S")]
        [InlineData("E")]
        [InlineData("W")]
        [InlineData("n")]
        public void GivenValidInputForADirection_ShouldCreateADirection(string directionCode)
        {
            Action action = () => Direction.FromCode(directionCode);
            action.Should().NotThrow("Direction was correctly created.");
            Direction.FromCode(directionCode).Code.Should().Be(directionCode.ToUpper());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("X")]
        [InlineData("SS")]
        public void GivenInvalidInputForADirection_ShouldThrowArgumentException(string directionCode)
        {
            Action action = () => Direction.FromCode(directionCode);
            action.Should().Throw<ArgumentException>("Invalid input was provided");
        }
    }
}