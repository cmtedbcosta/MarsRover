using System;
using FluentAssertions;
using MarsRover.Models;
using Xunit;

namespace MarsRover.Test.Models
{
    public class WhenCreatingPlateau
    {
        [Theory]
        [InlineData(0,0)]
        [InlineData(1,0)]
        [InlineData(0,1)]
        public void GivenInValidInputForAPlateau_ShouldThrowArgumentOutOfRangeException(uint maxX, uint maxY)
        {
            Action action = () =>
            {
                _ = new Plateau(maxX, maxY);
            };

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(1,1)]
        [InlineData(5,5)]
        [InlineData(1,5)]
        [InlineData(5,1)]
        [InlineData(99,99)]
        public void GivenValidInputForAPlateau_ShouldCreateAPlateau(uint maxX, uint maxY)
        {
            Action action = () =>
            {
                var plateau = new Plateau(maxX, maxY);
                plateau.MaxSizeX.Should().Be(maxX-1);
                plateau.MaxSizeY.Should().Be(maxY-1);
            };

            action.Should().NotThrow("Direction was correctly created.");
        }
    }
}