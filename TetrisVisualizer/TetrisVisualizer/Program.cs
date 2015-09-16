using System;
using System.Collections.Immutable;
using System.Diagnostics;

namespace TetrisVisualizer
{
    public class Direction
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
//            var stopwatch = new Stopwatch();
//            stopwatch.Start();
//            var game = new Game("clever-w9-h10-c200.json");
//            var game = new Game("clever-w100-h99-c10000.json");
//            var game = new Game("clever-w500-h7-c5000.json");
//            var game = new Game("random-w1000-h1000-c1000000.json");
//            var game = new Game("cubes-w8-h8-c100.json");
            
//            var game = new Game("smallest.json");
//            var game = new Game("clever-w20-h25-c100000.json");
//            var game = new Game("cubes-w1000-h1000-c1000000.json");
//            var game = new Game("myTest.json");
            var game = new Game(args[0]);
            while (true)
            {
                game = game.GetNextState();
                if (game == null) break;
            }
//            stopwatch.Stop();
//            Console.WriteLine(stopwatch.Elapsed);
        }
    }
}
