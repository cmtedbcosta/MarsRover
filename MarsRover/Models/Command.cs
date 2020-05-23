using System;
using System.Collections.Generic;
using System.Linq;

namespace MarsRover.Models
{
    public class Command
    {
        public static readonly Command Move = new Command("M", "Move");
        public static readonly Command TurnLeft = new Command("L", "Turn left");
        public static readonly Command TurnRight = new Command("R", "Turn right");

        private static readonly List<Command> All = new List<Command>
        {
            Move,
            TurnLeft,
            TurnRight
        };

        private Command(string code, string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Code = code ?? throw new ArgumentNullException(nameof(code));
        }

        public string Name { get;  }

        public string Code { get; }

        public static Command FromCode(string code)
        {
            return All.FirstOrDefault(s => s.Code == code?.ToUpper()) ?? throw new ArgumentException("Command code provided is not valid");
        }

        public static Command FromCode(char code)
        {
            return FromCode(code.ToString());
        }

        private bool Equals(Command other)
        {
            return Code == other.Code;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Command) obj);
        }

        public override int GetHashCode()
        {
            return (Code != null ? Code.GetHashCode() : 0);
        }

        public static bool operator ==(Command left, Command right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Command left, Command right)
        {
            return !Equals(left, right);
        }
    }
}
