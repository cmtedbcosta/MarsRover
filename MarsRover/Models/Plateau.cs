using System;

namespace MarsRover.Models
{
    public class Plateau
    {
        public Plateau(uint maxSizeX, uint maxSizeY)
        {
            if (maxSizeX <= 0) throw new ArgumentOutOfRangeException(nameof(maxSizeX));
            if (maxSizeY <= 0) throw new ArgumentOutOfRangeException(nameof(maxSizeY));

            // Because 0 position is considered, than the max X and Y are the ones provided minus 1
            MaxSizeX = maxSizeX-1;
            MaxSizeY = maxSizeY-1;
        }

        public uint MaxSizeX { get;}

        public uint MaxSizeY { get; }

        protected bool Equals(Plateau other)
        {
            return MaxSizeX == other.MaxSizeX && MaxSizeY == other.MaxSizeY;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Plateau) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) MaxSizeX * 397) ^ (int) MaxSizeY;
            }
        }

        public static bool operator ==(Plateau left, Plateau right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Plateau left, Plateau right)
        {
            return !Equals(left, right);
        }
    }
}
