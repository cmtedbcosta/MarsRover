using System;

namespace MarsRover.Models
{
    public class Rover
    {
        public Rover(uint id, uint positionX, uint positionY, Direction facingDirection, bool isWaitingRescue = false)
        {
            Position = (positionX, positionY);
            FacingDirection = facingDirection ?? throw new ArgumentNullException(nameof(facingDirection), "Facing direction is invalid");
            IsWaitingRescue = isWaitingRescue;
            Id = id;
        }

        public Rover(uint id, string error)
        {
            Id = id;
            IsWaitingRescue = false;
            Error = string.IsNullOrEmpty(error) ? throw new ArgumentNullException(nameof(error)) : error;
        }

        public Rover(uint id, uint positionX, uint positionY, Direction facingDirection, string error)
        {
            Id = id;
            IsWaitingRescue = true;
            Position = (positionX, positionY);
            FacingDirection = facingDirection ?? throw new ArgumentNullException(nameof(facingDirection), "Facing direction is invalid");
            Error = string.IsNullOrEmpty(error) ? throw new ArgumentNullException(nameof(error)) : error;
        }

        public string Name => $"Rover {Id.ToString()}";

        public uint Id { get; }

        public (uint X, uint Y) Position { get; }

        public Direction FacingDirection { get; }

        public bool IsWaitingRescue { get; }

        public string Error { get; }

        protected bool Equals(Rover other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Rover) obj);
        }

        public override int GetHashCode()
        {
            return (int) Id;
        }

        public static bool operator ==(Rover left, Rover right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Rover left, Rover right)
        {
            return !Equals(left, right);
        }
    }
}
