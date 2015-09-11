using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TetrisVisualizer
{
    public enum RotationDirection
    {
        CounterClockwise = -1,
        Clockwise = 1
    }

    public class Piece
    {
        public ImmutableArray<Point> Cells { get; private set; }
        public ImmutableArray<Point> AbsoluteCoordinates { get; private set; }
        private int Width { get; set; }
        private int Height { get; set; }

        public Piece(ImmutableArray<Point> cellsParam, Point absoluteCoordinatesOfCenter)
        {
            Cells = cellsParam;
            AbsoluteCoordinates = Cells.Select(cell =>
                new Point(
                    cell.X + absoluteCoordinatesOfCenter.X,
                    cell.Y + absoluteCoordinatesOfCenter.Y)
                    )
                .ToImmutableArray();
        }

        public Piece(ImmutableArray<Point> cellsParam, int width, int height)
        {
            Cells = cellsParam;
            Width = width;
            Height = height;
            CalculateAbsoluteCoordinatesOfCells();
        }

        private Piece(ImmutableArray<Point> cellsParam, ImmutableArray<Point> absoluteCoordinatesParam)
        {
            Cells = cellsParam;
            AbsoluteCoordinates = absoluteCoordinatesParam;
        }

        public Piece Rotate(RotationDirection direction)
        {
            var newCells = Cells.Select(cell => new Point(-1*cell.Y*(int) direction, cell.X*(int) direction));
            var newAbsoluteCoordinates = AbsoluteCoordinates.Zip(
                newCells,
                (coordinates, cell) => new Point(coordinates.X + cell.X, coordinates.Y - cell.Y)
                ).ToImmutableArray();
            return new Piece(Cells, newAbsoluteCoordinates);
        }

        public Piece Move(Point direction)
        {
            throw new NotImplementedException();
        }

        private void CalculateAbsoluteCoordinatesOfCells()
        {
            var relativeLeft = Cells.Min(cell => cell.X);
            int absoluteLeft = ((Width - Cells.Min(cell => cell.X)) / 2);
            int relativeTop = Cells.Min(cell => cell.Y);
            int absolutTop = Height;
            Point centerCoordinates = new Point(absoluteLeft + relativeLeft, absolutTop - relativeTop);
            AbsoluteCoordinates =
                Cells.Select(cell =>
                        new Point(centerCoordinates.X + cell.X, centerCoordinates.Y + cell.Y)).ToImmutableArray();
        }
    }
}
