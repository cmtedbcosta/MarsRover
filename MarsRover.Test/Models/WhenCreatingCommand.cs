using System;
using FluentAssertions;
using MarsRover.Models;
using Xunit;

namespace MarsRover.Test.Models
{
    public class WhenCreatingCommand
    {
        [Theory]
        [InlineData("M")]
        [InlineData("L")]
        [InlineData("R")]
        [InlineData("r")]
        public void GivenValidInputForACommand_ShouldCreateACommand(string commandCode)
        {
            Action action = () => Command.FromCode(commandCode);
            action.Should().NotThrow("Command was correctly created.");
            Command.FromCode(commandCode).Code.Should().Be(commandCode.ToUpper());
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("X")]
        [InlineData("MM")]
        public void GivenInvalidInputForACommand_ShouldThrowArgumentException(string commandCode)
        {
            Action action = () => Command.FromCode(commandCode);
            action.Should().Throw<ArgumentException>("Invalid input was provided");
        }
    }
}