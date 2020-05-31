namespace MarsRover.Models
{
    public class Rover
    {
        internal Rover(uint id, uint positionX, uint positionY, Direction facingDirection, bool isWaitingRescue, string error)
        {
            Position = (positionX, positionY);
            FacingDirection = facingDirection;
            IsWaitingRescue = isWaitingRescue;
            Id = id;
            Error = error;
        }

        public string Name => $"Rover {Id}";

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
