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
        [InlineData(99,99)]
        public void GivenValidInputForAPlateau_ShouldCreateAPlateau(uint maxX, uint maxY)
        {
            Action action = () => new Plateau(maxX, maxY);
            action.Should().NotThrow("Direction was correctly created.");
            var plateau = new Plateau(maxX, maxY);
            plateau.MaxSizeX.Should().Be(maxX);
            plateau.MaxSizeY.Should().Be(maxY);
        }
    }
}