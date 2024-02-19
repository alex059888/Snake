using System;
using System.Collections.Generic;

namespace util
{
    public class Direction
    {
        public readonly static Direction LEFT = new Direction(0,-1);
        public readonly static Direction RIGHT = new Direction(0, 1);
        public readonly static Direction UP = new Direction(-1, 0);
        public readonly static Direction DOWN = new Direction(1, 0);
        private int row;
        private int col;

        private Direction(int row, int col) { 
            this.row = row; this.col = col;
        }

        public Direction Opposite() {
            return new Direction(-row, -col);
        }

        public override bool Equals(object? obj)
        {
            return obj is Direction direction &&
                   row == direction.row &&
                   col == direction.col;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(row, col);
        }

        public static bool operator ==(Direction? left, Direction? right)
        {
            return EqualityComparer<Direction>.Default.Equals(left, right);
        }

        public static bool operator !=(Direction? left, Direction? right)
        {
            return !(left == right);
        }

        public int getRow()
        {
            return row;
        }

        public int getCol()
        {
            return col;
        }
    }
}
