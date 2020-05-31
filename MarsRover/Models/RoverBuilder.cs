using System;

namespace MarsRover.Models
{
    public class RoverBuilder
    {
        private readonly uint _id;
        private uint _positionX;
        private uint _positionY;
        private Direction _facingDirection;
        private bool _isWaitingRescue;
        private string _error;

        public RoverBuilder(uint id)
        {
            _id = id;
        }

        public RoverBuilder NotDeployed(string error)
        {
            _isWaitingRescue = true;
            _error = string.IsNullOrEmpty(error) ? throw new ArgumentNullException(nameof(error)) : error;
            return this;
        }

        public RoverBuilder StoppedBeforeCrash(uint positionX, uint positionY, Direction facingDirection, string error)
        {
            _isWaitingRescue = true;
            _error = string.IsNullOrEmpty(error) ? throw new ArgumentNullException(nameof(error)) : error;
            _positionY = positionY;
            _positionX = positionX;
            _facingDirection = facingDirection ?? throw new ArgumentNullException(nameof(facingDirection), "Facing direction is invalid");
            return this;
        }

        public RoverBuilder Operational(uint positionX, uint positionY, Direction facingDirection)
        {
            _isWaitingRescue = false;
            _error = null;
            _positionY = positionY;
            _positionX = positionX;
            _facingDirection = facingDirection ?? throw new ArgumentNullException(nameof(facingDirection), "Facing direction is invalid");
            return this;
        }

        public Rover Build() => new Rover(_id, _positionX, _positionY, _facingDirection, _isWaitingRescue, _error);
    }
}
