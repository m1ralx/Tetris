using System.Collections.Immutable;
using System.Linq;

namespace TetrisVisualizer
{
    public class Piece
    {
        public readonly ImmutableHashSet<Point> Cells;
        public readonly ImmutableHashSet<Point> AbsoluteCoordinates;
        private Point AbsoluteCenterCoordinates { get; set; }

        private int Width { get; set; }
        private int Height { get; set; }

        public Piece(ImmutableHashSet<Point> cellsParam, int width, int height)
        {
            Cells = cellsParam;
            Width = width;
            Height = height;
            AbsoluteCenterCoordinates = CalculateAbsoluteCoordinatesOfCenter();
            AbsoluteCoordinates =
                Cells.Select(cell =>
                        new Point(
                            AbsoluteCenterCoordinates.X + cell.X,
                            AbsoluteCenterCoordinates.Y - cell.Y
                            )).ToImmutableHashSet();
        }

        private Piece(ImmutableHashSet<Point> cellsParam, ImmutableHashSet<Point> absoluteCoordinatesParam, Point absCenterCoordinatesParam)
        {
            Cells = cellsParam;
            AbsoluteCoordinates = absoluteCoordinatesParam;
            AbsoluteCenterCoordinates = absCenterCoordinatesParam;
        }

        public Piece Rotate(Point direction)
        {
            var newCells = Cells.Select(cell => new Point(
                -1*cell.Y*direction.X,
                cell.X*direction.X
                )).ToImmutableHashSet();

            var newAbsoluteCoordinates = newCells
                .Select(cell => new Point( 
                    cell.X + AbsoluteCenterCoordinates.X,
                    AbsoluteCenterCoordinates.Y - cell.Y
                    )).ToImmutableHashSet();
            return new Piece(newCells, newAbsoluteCoordinates, AbsoluteCenterCoordinates);
        }

        public Piece Move(Point direction)
        {
            var newAbsoluteCoordinates = AbsoluteCoordinates
                .Select(coordinates => new Point(
                    coordinates.X + direction.X,
                    coordinates.Y + direction.Y)
                    ).ToImmutableHashSet();
            var newAbsoluteCenterCoordinates = new Point(AbsoluteCenterCoordinates.X + direction.X,
                AbsoluteCenterCoordinates.Y + direction.Y);
            return new Piece(Cells, newAbsoluteCoordinates, newAbsoluteCenterCoordinates);
        }

        private Point CalculateAbsoluteCoordinatesOfCenter()
        {
            var relativeLeft = Cells.Min(cell => cell.X);
            var widthOfPiece = Cells.Max(cell => cell.X) - Cells.Min(cell => cell.X) + 1;
            int absoluteLeft = ((Width - widthOfPiece) / 2);
            int relativeTop = Cells.Min(cell => cell.Y);
            int absolutTop = Height - 1;
            return new Point(absoluteLeft - relativeLeft, absolutTop + relativeTop);
        }
    }
}
