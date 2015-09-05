using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace TetrisVisualizer
{
    public class Program
    {
        static void Main(string[] args)
        {
            FileStream stream = File.Open("largeTest.json", FileMode.Open);
            StreamReader reader = new StreamReader(stream);

            JsonReader jReader = new JsonTextReader(reader);
            JToken jToken = JObject.ReadFrom(jReader);

            JsonSerializer = new JsonSerializer
            //JObject search = JObject.Load
        }
    }
}
