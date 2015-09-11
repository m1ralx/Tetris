using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Remoting.Channels;

namespace TetrisVisualizer
{
    public class Program
    {
        static void Main(string[] args)
        {
//            var loader = new GameInfoLoader("medium.json");
            var loader = new GameInfoLoader("smallest.json");
//            var loader = new GameInfoLoader("veryLargeTest.json");
//            var loader = new GameInfoLoader("largeTest.json");
            var i = 0;
            foreach (var c in loader.ReadCommandsSequence())
            {
                Console.WriteLine(c);
                Console.ReadKey();
                i++;
//                if (i == 11)
//                    break;
//                Console.ReadKey();
            }
//            Console.WriteLine(i);
        }
    }
}
