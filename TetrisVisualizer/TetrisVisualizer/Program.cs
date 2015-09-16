using System;

namespace TetrisVisualizer
{
    public static class Direction
    {
        public static readonly Point Left = new Point(-1, 0);
        public static readonly Point Right = new Point(1, 0);
        public static readonly Point Down = new Point(0, -1);
        public static readonly Point CounterClockwise = new Point(-1, 0);
        public static readonly Point Clockwise = new Point(1, 0);
    }

    public class Point
    {
        public int X{ get; private set; }
        public int Y{ get; private set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Point p = (Point)obj;
            return (X == p.X) && (Y == p.Y);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(X, Y).GetHashCode();
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            var game = new Game(args[0]);
            while (true)
            {
                game = game.GetNextState();
                if (game == null) break;
            }
        }
    }
}
