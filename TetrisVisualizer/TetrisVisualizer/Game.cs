using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TetrisVisualizer
{
    public class Game
    {
        private readonly Piece _currentPiece;
        private readonly int _commandNumber;
        private readonly IEnumerator<Piece> _piecesEnumerator;
        private readonly IEnumerator<string> _commandsEnumerator;

        private int Width  { get; set; }
        private int Height { get; set; }

        private readonly ImmutableDictionary<int, ImmutableHashSet<int>> _usedCells;
        private int Score { get; set; }

        public Game(string path)
        {
            var infoLoader = new GameInfoLoader(path);

            var size = infoLoader.GetWidthAndHeight();
            Width = size.Item1;
            Height = size.Item2;
            
            var commandsSequence = infoLoader.GetCommandsSequence();
            var piecesSequence = infoLoader.GetPiecesSequence(Width, Height);
            _piecesEnumerator = piecesSequence.GetEnumerator();
            _commandsEnumerator = commandsSequence.GetEnumerator();
            _piecesEnumerator.MoveNext();
            
            _commandNumber = 0;
            _currentPiece = _piecesEnumerator.Current;
            _usedCells = ImmutableDictionary<int, ImmutableHashSet<int>>.Empty;
        }

        public Game(ImmutableDictionary<int, ImmutableHashSet<int>> usedCells, 
            IEnumerator<string> commandsEnumerator, IEnumerator<Piece> piecesEnumerator, int commandNumber, Piece currentPiece)
        {
            _usedCells = usedCells;
            _commandsEnumerator = commandsEnumerator;
            _piecesEnumerator = piecesEnumerator;
            _commandNumber = commandNumber;
            _currentPiece = currentPiece;
        }

        public Game GetNextState()
        {
            var newUsedCells = _usedCells.ToImmutableDictionary();
            if (!_commandsEnumerator.MoveNext())
                return null;
            var command = _commandsEnumerator.Current;
            if (command == "P")
            {
                Print(newUsedCells, _currentPiece);
                return new Game(newUsedCells, _commandsEnumerator, _piecesEnumerator, _commandNumber + 1, _currentPiece)
                {
                    Score = Score,
                    Width = Width,
                    Height = Height
                };
            }
            var newPiece = ApplyCommand(command);
            Piece newCurrentPiece;
            if (!PieceCanBeHere(newPiece, newUsedCells))
            {
                newUsedCells = FixPiece(newUsedCells);
                newUsedCells = ClearFullRows(newUsedCells);
                newCurrentPiece = SpawnNewPiece(ref newUsedCells);
                Console.WriteLine("{0} {1}", _commandNumber, Score);
            }
            else
                newCurrentPiece = newPiece;
            return new Game(newUsedCells, _commandsEnumerator, _piecesEnumerator, _commandNumber + 1, newCurrentPiece)
            {
                Score = Score,
                Width = Width,
                Height = Height
            };
        }

        private Piece SpawnNewPiece(ref ImmutableDictionary<int, ImmutableHashSet<int>> newUsedCells)
        {
            _piecesEnumerator.MoveNext();
            var newCurrentPiece = _piecesEnumerator.Current;
            if (!PieceCanBeHere(newCurrentPiece, newUsedCells))
            {
                newUsedCells = newUsedCells.Clear();
                Score -= 10;
            }
            return newCurrentPiece;
        }

        private Piece ApplyCommand(string command)
        {
            // почему switch, а не, например, словарь <string, Func<Piece>?
            // Потому что фигуры постоянно меняются и слишком затрано
            // для каждой инициализировать свой словарь
            switch (command)
            {
                case "A":
                    return _currentPiece.Move(Direction.Left);
                case "S":
                    return _currentPiece.Move(Direction.Down);
                case "D":
                    return _currentPiece.Move(Direction.Right);
                case "Q":
                    return _currentPiece.Rotate(Direction.CounterClockwise);
                case "E":
                    return _currentPiece.Rotate(Direction.Clockwise);
                default:
                    return null;
            }
        }

        private bool PointInField(Point point)
        {
            return point.X >= 0 && point.X < Width && point.Y >= 0 && point.Y < Height;
        }
        
        private bool PieceInField(Piece piece)
        {
            return piece.AbsoluteCoordinates.All(PointInField);
        }
        
        private bool PieceCanBeHere(Piece piece, ImmutableDictionary<int, ImmutableHashSet<int>> usedCells)
        {
            bool anyCellsAlreadyUsed = piece.AbsoluteCoordinates.Any(
                piecePoint => usedCells.ContainsKey(piecePoint.Y) &&
                    usedCells[piecePoint.Y].Contains(piecePoint.X)
                );
            return !(anyCellsAlreadyUsed || !PieceInField(piece));
        }

        private ImmutableDictionary<int, ImmutableHashSet<int>> FixPiece(ImmutableDictionary<int, ImmutableHashSet<int>> newUsedCells)
        {
            foreach (var coordinates in _currentPiece.AbsoluteCoordinates)
            {
                if (!newUsedCells.ContainsKey(coordinates.Y))
                    newUsedCells = newUsedCells.Add(coordinates.Y, ImmutableHashSet<int>.Empty);
                newUsedCells = newUsedCells.SetItem(coordinates.Y, newUsedCells[coordinates.Y].Add(coordinates.X));
            }
            return newUsedCells;
        }

        private ImmutableDictionary<int, ImmutableHashSet<int>> ClearFullRows(ImmutableDictionary<int, ImmutableHashSet<int>> usedCells)
        {
            int minIndexOfFullRow = int.MaxValue;
            int countOfFullRows = 0;
            foreach (var rowIndex in usedCells.Keys)
            {
                if (usedCells.ContainsKey(rowIndex) && usedCells[rowIndex].Count == Width)
                {
                    usedCells = usedCells.Remove(rowIndex);
                    Score++;
                    if (rowIndex < minIndexOfFullRow)
                        minIndexOfFullRow = rowIndex;
                    countOfFullRows++;
                }
            }
            for (var i = 0; i < countOfFullRows; i++)
                usedCells = ShiftUpperRowsDown(minIndexOfFullRow, usedCells);
            return usedCells;
        }

        private ImmutableDictionary<int, ImmutableHashSet<int>> ShiftUpperRowsDown(int rowIndex, 
            ImmutableDictionary<int, ImmutableHashSet<int>> usedCells)
        {
            for (var y = rowIndex; y < Height - 1; y++)
            {
                if (usedCells.ContainsKey(y))
                    if (usedCells.ContainsKey(y + 1))
                    {
                        usedCells = usedCells.SetItem(y, usedCells[y + 1]);
                        usedCells = usedCells.Remove(y + 1);
                    }
                    else
                        usedCells = usedCells.Remove(y);
                else
                    if (usedCells.ContainsKey(y + 1))
                    {
                        usedCells = usedCells.Add(y, usedCells[y + 1]);
                        usedCells = usedCells.Remove(y + 1);
                    }
            }
            return usedCells;
        }

        private void Print(ImmutableDictionary<int, ImmutableHashSet<int>> usedCells, Piece newCurrentPiece)
        {
            for (var y = Height - 1; y >= 0; y-- )
            {
                for (var x = 0; x < Width; x++)
                {
                    var currentPoint = new Point(x, y);
                    if (usedCells.ContainsKey(y) && usedCells[y].Contains(currentPoint.X))
                    {
                        Console.Write('#');
                        continue;
                    }
                    if (newCurrentPiece.AbsoluteCoordinates.Contains(currentPoint))
                    {
                        Console.Write('*');
                        continue;
                    }
                    Console.Write('.');
                }
                Console.WriteLine();
            }
        }
    }
}
