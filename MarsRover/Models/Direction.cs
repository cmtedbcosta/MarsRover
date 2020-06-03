using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRover.Models
{
    public class Direction
    {
        public static readonly Direction North = new Direction("N", "North", 0);
        public static readonly Direction East = new Direction("E", "East", 1);
        public static readonly Direction South = new Direction("S", "South", 2);
        public static readonly Direction West = new Direction("W", "West", 3);

        public static readonly List<Direction> All = new List<Direction>
        {
            North,
            South,
            East,
            West
        };

        private Direction(string code, string name, uint index)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Code = code ?? throw new ArgumentNullException(nameof(code));
            Index = index;
        }

        public string Name { get; }

        public string Code { get; }

        private uint Index { get; }

        public static Direction FromCode(string code)
        {
            return All.FirstOrDefault(s => s.Code == code?.ToUpper()) ?? throw new ArgumentException("Direction code provided is not valid");
        }

        public Direction NextRight() => Index == 3 ? North : All.Single(d => d.Index == Index + 1);

        public Direction NextLeft() => Index == 0 ? West : All.Single(d => d.Index == Index - 1);

        private bool Equals(Direction other)
        {
            return Code == other.Code;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Direction) obj);
        }

        public override int GetHashCode()
        {
            return (Code != null ? Code.GetHashCode() : 0);
        }

        public static bool operator ==(Direction left, Direction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Direction left, Direction right)
        {
            return !Equals(left, right);
        }
    }
}
