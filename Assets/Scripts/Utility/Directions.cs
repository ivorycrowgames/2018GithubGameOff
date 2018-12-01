using IvoryCrow.Extensions;
using System;

namespace IvoryCrow.Utilities
{
    public enum DirectionLR
    {
        Left,
        Right
    }

    public static class Directions
    {
        public static int ToScalar(DirectionLR direction)
        {
            return (direction == DirectionLR.Left) ? -1 : 1;
        }

        public static bool IsVertical(Direction direction)
        {
            return (direction == Direction.Up || direction == Direction.Down);
        }

        public static bool IsHorizontal(Direction direction)
        {
            return !IsVertical(direction);
        }
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
}
