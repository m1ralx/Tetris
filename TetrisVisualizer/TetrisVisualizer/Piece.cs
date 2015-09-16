using System;
using System.Collections.Immutable;
using System.Linq;

namespace TetrisVisualizer
{
    public class Piece
    {
//        public ImmutableArray<Point> Cells { get; private set; }
//        public ImmutableArray<Point> AbsoluteCoordinates { get; private set; }
        public ImmutableHashSet<Point> Cells { get; private set; }
        public ImmutableHashSet<Point> AbsoluteCoordinates { get; private set; }
        private Point AbsoluteCenterCoordinates { get; set; }

        private int Width { get; set; }
        private int Height { get; set; }

//        private Piece(ImmutableArray<Point> cellsParam, Point absoluteCoordinatesOfCenter)
//        {
//            Cells = cellsParam;
//            AbsoluteCoordinates = Cells.Select(cell =>
//                new Point(
//                    cell.X + absoluteCoordinatesOfCenter.X,
//                    cell.Y + absoluteCoordinatesOfCenter.Y
//                    )).ToImmutableArray();
//            Commands = GetCommandsDictionary();
//        }

//        public Piece(ImmutableArray<Point> cellsParam, int width, int height)
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
//                            )).ToImmutableArray();
                            )).ToImmutableHashSet();
//            Commands = GetCommandsDictionary();
        }

//        private Piece(ImmutableArray<Point> cellsParam, ImmutableArray<Point> absoluteCoordinatesParam, Point absCenterCoordinatesParam)
        private Piece(ImmutableHashSet<Point> cellsParam, ImmutableHashSet<Point> absoluteCoordinatesParam, Point absCenterCoordinatesParam)
        {
            Cells = cellsParam;
            AbsoluteCoordinates = absoluteCoordinatesParam;
            AbsoluteCenterCoordinates = absCenterCoordinatesParam;
//            Commands = GetCommandsDictionary();
        }

        public Piece Rotate(Point direction)
        {
            var newCells = Cells.Select(cell => new Point(
                -1*cell.Y*direction.X,
                cell.X*direction.X
//                )).ToImmutableArray();
                )).ToImmutableHashSet();
            var newAbsoluteCoordinates = newCells
                .Select(cell => new Point( 
                    cell.X + AbsoluteCenterCoordinates.X,
                    AbsoluteCenterCoordinates.Y - cell.Y
//                    )).ToImmutableArray();
                    )).ToImmutableHashSet();
            return new Piece(newCells, newAbsoluteCoordinates, AbsoluteCenterCoordinates);
        }

        public Piece Move(Point direction)
        {
            var newAbsoluteCoordinates = AbsoluteCoordinates
                .Select(coordinates => new Point(
                    coordinates.X + direction.X,
                    coordinates.Y + direction.Y)
//                    ).ToImmutableArray();
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
