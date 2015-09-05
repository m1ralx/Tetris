using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TetrisVisualizer
{
    public class GameInfo
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public IEnumerable<Piece> Pieces { get; private set; }
        public IEnumerable<string> Commands { get; private set; }

        public GameInfo(string filePath)
        {
            
        }
    }
}
