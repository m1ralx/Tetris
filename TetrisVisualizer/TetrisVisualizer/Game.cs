using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisVisualizer
{
    public class Game
    {
        private Piece currentPiece { get; set; }
        private GameInfo gameInfo { get; set; }
        private List<Func<Piece, Piece>> commands { get; set; }

        public Game()
        {
            currentPiece = new Piece(null);
            //comm
        }

        private void PerformCommand(string command)
        {
            //здесь надо как-то организовать ветвление. Я пока не придумал как это удачнее сделать

        }


    }
}
