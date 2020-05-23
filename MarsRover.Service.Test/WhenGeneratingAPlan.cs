using System;
using System.Linq;
using FluentAssertions;
using MarsRover.Models;
using MarsRover.Service.Controls;
using Xunit;

namespace MarsRover.Service.Test
{
    public class WhenGeneratingAPlan
    {
        
        [Theory]
        [InlineData("5 5 \r 1 1 N \r MMR")]
        [InlineData("5 5 \r 1 1 N \r M M R")]
        public void GivenAValidCommand_ShouldGenerateACorrectPlan_AndNoRoversWithError(string command)
        {
            var plateau = new Plateau(5,5);
            var rover = new Rover(1, 1, 1, Direction.North);
            object[] commands = {Command.Move, Command.Move, Command.TurnRight};

            var commandParser = new PlanControl(command);
            var plan = commandParser.GeneratePlan();

            plan.Plateau.Should().Be(plateau);

            plan.RoverRoutes.Count().Should().Be(1, "One route should be planned.");
            
            var roverRoute = plan.RoverRoutes.First();
            roverRoute.Rover.Should().Be(rover);
            roverRoute.Commands.Should().BeEquivalentTo(commands);
            roverRoute.Rover.Position.Should().Be(rover.Position);
            roverRoute.Rover.FacingDirection.Should().Be(rover.FacingDirection);

            plan.RoversWithError.Should().BeEmpty();
        }

        [Fact]
        public void GivenAValidCommandFor2Rovers_ShouldGenerateACorrectPlan_AndNoRoversWithError()
        {
            var plateau = new Plateau(5,5);
            
            var rover1 = new Rover(1, 1, 1, Direction.North);
            object[] commands1 = {Command.Move, Command.Move, Command.TurnRight};
            
            var rover2 = new Rover(2, 3, 3, Direction.West);
            object[] commands2 = {Command.Move, Command.TurnLeft, Command.Move};
            
            const string stringCommand = "5 5 \r 1 1 N \r MMR \r 3 3 W \r MLM";

            var commandParser = new PlanControl(stringCommand);
            var plan = commandParser.GeneratePlan();

            plan.Plateau.Should().Be(plateau);

            plan.RoverRoutes.Count().Should().Be(2, "Two routes should be planned.");
            
            // First Rover
            var roverRoute1 = plan.RoverRoutes.ElementAt(0);
            roverRoute1.Rover.Should().Be(rover1);
            roverRoute1.Commands.Should().BeEquivalentTo(commands1);
            roverRoute1.Rover.Position.Should().Be(rover1.Position);
            roverRoute1.Rover.FacingDirection.Should().Be(rover1.FacingDirection);

            // Second Rover
            var roverRoute2 = plan.RoverRoutes.ElementAt(1);
            roverRoute2.Rover.Should().Be(rover2);
            roverRoute2.Commands.Should().BeEquivalentTo(commands2);
            roverRoute2.Rover.Position.Should().Be(rover2.Position);
            roverRoute2.Rover.FacingDirection.Should().Be(rover2.FacingDirection);

            plan.RoversWithError.Should().BeEmpty();
        }

        [Theory]
        [InlineData("5 5 \r 1 1 X \r MMR \r 3 3 W \r MLM")]
        [InlineData("5 5 \r 1 1 N \r MXR \r 3 3 W \r MLM")]
        public void GivenACommandFor2RoversWithFirstInvalid_ShouldGenerateACorrectPlanForSecondRover_AndFirstRoverHasError(string command)
        {
            var plateau = new Plateau(5,5);

            var rover1 = new Rover(1, 1, 1, Direction.North);

            var rover2 = new Rover(2, 3, 3, Direction.West);
            object[] commands2 = {Command.Move, Command.TurnLeft, Command.Move};

            var commandParser = new PlanControl(command);
            var plan = commandParser.GeneratePlan();

            plan.Plateau.Should().Be(plateau);

            plan.RoverRoutes.Count().Should().Be(1, "One route should be planned.");
            
            // First Rover is with error
            plan.RoversWithError.Should().Contain(rover1);

            // Second Rover has routes
            var roverRoute1 = plan.RoverRoutes.ElementAt(0);
            roverRoute1.Rover.Should().Be(rover2);
            roverRoute1.Commands.Should().BeEquivalentTo(commands2);
            roverRoute1.Rover.Position.Should().Be(rover2.Position);
            roverRoute1.Rover.FacingDirection.Should().Be(rover2.FacingDirection);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GivenAnEmptyOrNullCommand_ShouldThrowArgumentNullException(string command)
        {
            Action action = () => new PlanControl(command);
            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("5 \r 1 1 N \r MMR")]
        [InlineData("5 5 \r 1 1 \r MMR")]
        [InlineData("5 5 \r MMR")]
        [InlineData("1 1 N \r MMR")]
        [InlineData("MMR")]
        [InlineData("5 5 ")]
        [InlineData("5 5 \r 1 1 X \r MMR")]
        [InlineData("5 5 \r 1 1 N \r MXR")]
        [InlineData("5  5 \r 1  1  N \r MXR")]
        public void GivenAnInvalidCommand_ShouldThrowArgumentException(string command)
        {
            var commandParser = new PlanControl(command);
            Action action = () => commandParser.GeneratePlan();
            action.Should().Throw<ArgumentException>();
        }
    }
}
