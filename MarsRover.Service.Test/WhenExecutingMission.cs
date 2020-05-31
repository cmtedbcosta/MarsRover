using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MarsRover.Service.Controls;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace MarsRover.Service.Test
{
    public class WhenExecutingMission
    {
        private readonly MissionControl _missionControl;

        public WhenExecutingMission()
        {
            var directionControl = new DirectionControl();
            var movementControl = new MovementControl();
            var navigationControl = new NavigationControl(movementControl, directionControl);
            _missionControl = new MissionControl(new NullLoggerFactory(), navigationControl);
        }

        public static IEnumerable<object[]> ValidInputForMissionControl
        {
            get
            {
                yield return new object[] { "5 5 \r 1 1 N \r MMM", new (uint, uint)[] { (1, 4) }}; // normal
                yield return new object[] { "5 5 \r 1 1 N \r MMM \r 1 1 N \r MMM", new (uint, uint)[] { (1, 4), (1, 3) }}; // with collision
                yield return new object[] { "5 5 \r 1 1 X \r MMM \r 1 1 N \r MMM", new (uint, uint)[] { (0, 0), (1, 4) }}; // with invalid rover
                yield return new object[] { "5 5 \r 1 1 N \r MMM \r 1 1 N \r MXM", new (uint, uint)[] { (1, 4), (0, 0) }}; // with invalid rover
                yield return new object[] { "5 5 \r 1 1 N \r MMM \r 1 1 N \r MMM \r 1 1 X \r MMM" , new (uint, uint)[] { (1, 4), (1, 3), (0, 0) }}; // with collision and invalid rover
            }                     
        }

        [Theory]
        [MemberData(nameof(ValidInputForMissionControl))]
        public void GivenAValidCommand_ShouldReturnCorrectPosition(string command, (uint, uint)[] expectedPositions)
        {
            var rovers = _missionControl.Execute(command).ToArray();
            
            // Assert rovers final position
            for (var i = 0; i < rovers.Count(); i++)
            {
                rovers[i].Position.Should().Be(expectedPositions[i]);
            }
        }
    }
}
