using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TetrisVisualizer
{
    public enum RotationDirection
    {
        Clockwise,
        CounterClockwise
    }

    public class Piece
    {
        private IEnumerable<Point> points { get; set; }

        public Piece(IEnumerable<Point> pointsParam)
        {
            points = pointsParam;
        }

        public Piece Rotate(RotationDirection direction)
        {
            throw new NotImplementedException();
        }

        public Piece Move(Point direction)
        {
            throw new NotImplementedException();
        }

    }
}
