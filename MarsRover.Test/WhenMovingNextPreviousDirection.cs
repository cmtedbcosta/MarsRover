using FluentAssertions;
using MarsRover.Models;
using Xunit;

namespace MarsRover.Test
{
    public class WhenMovingNextPreviousDirection
    {

        [Theory]
        [InlineData("N", "E")]
        [InlineData("E", "S")]
        [InlineData("S", "W")]
        [InlineData("W", "N")]
        public void GivenCurrentDirection_ShouldMoveNextToCorrectDirection(string currentDirectionCode, string nextDirectionCode)
        {
            var currentDirection = Direction.FromCode(currentDirectionCode);
            var nextDirection = currentDirection.NextRight();
            nextDirection.Code.Should().Be(nextDirectionCode);
        }

        [Theory]
        [InlineData("N", "W")]
        [InlineData("W", "S")]
        [InlineData("S", "E")]
        [InlineData("E", "N")]
        public void GivenCurrentDirection_ShouldMovePreviousToCorrectDirection(string currentDirectionCode, string nextDirectionCode)
        {
            var currentDirection = Direction.FromCode(currentDirectionCode);
            var nextDirection = currentDirection.NextLeft();
            nextDirection.Code.Should().Be(nextDirectionCode);
        }
    }
}
