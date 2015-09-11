using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisVisualizer
{
    public class Game
    {
        private Piece CurrentPiece { get; set; }
        private IEnumerator<string> CommandsEnumerator { get; set; }
        private IEnumerator<Piece> PiecesEnumerator { get; set; }
        private IEnumerable<string> CommandsSequence { get; set; }
        private IEnumerable<Piece> PiecesSequence { get; set; }
        private int Width { get; set; }
        private int Height { get; set; }
        private ImmutableDictionary<int, ImmutableHashSet<Point>> UsedCells { get; set; }

//        private ImmutableHashSet<Point> 

        public Game(string path)
        {
            var infoLoader = new GameInfoLoader(path);
            var size = infoLoader.GetWidthAndHeight();
            Width = size.Item1;
            Height = size.Item2;
            CommandsSequence = infoLoader.ReadCommandsSequence();
            PiecesSequence = infoLoader.ReadPiecesSequence(Width, Height);
            CommandsEnumerator = CommandsSequence.GetEnumerator();
            PiecesEnumerator = PiecesSequence.GetEnumerator();
            UsedCells = ImmutableDictionary<int, ImmutableHashSet<Point>>.Empty;
            
//            Start();

        }

//        private void Start()
//        {
//            foreach (var command in CommandsSequence)
//            {
//                CurrentPiece = PiecesEnumerator.Current;
//
//            }
//        }

        private void PutNextPiece()
        {
            CurrentPiece = PiecesEnumerator.Current;
            PiecesEnumerator.MoveNext();
        }

        private void PerformCommand(string command)
        {
            //здесь надо как-то организовать ветвление. Я пока не придумал как это удачно сделать

        }


    }
}
