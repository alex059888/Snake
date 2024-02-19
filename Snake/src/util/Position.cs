using System;
using System.Collections.Generic;

namespace util
{
    public class Position
    {
        private int row;
        private int col;

        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public Position Translate(Direction d) {
            return new Position(row + d.getRow(), col + d.getCol());
        }

        public override bool Equals(object? obj)
        {
            return obj is Position position &&
                   row == position.row &&
                   col == position.col;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(row, col);
        }

        public static bool operator ==(Position? left, Position? right)
        {
            return EqualityComparer<Position>.Default.Equals(left, right);
        }

        public static bool operator !=(Position? left, Position? right)
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
