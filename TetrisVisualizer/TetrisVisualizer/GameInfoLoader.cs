using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TetrisVisualizer
{
    public class GameInfoLoader
    {
        public string FilePath { get; private set; }

        public GameInfoLoader(string filePathParam)
        {
            FilePath = filePathParam;
        }

        public Tuple<int, int> GetWidthAndHeight()
        {
            var sizes = new List<int>();
            using (var fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            using (JsonTextReader reader = new JsonTextReader(sr))
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.Integer)
                    {
                        sizes.Add((int.Parse(reader.Value.ToString())));
                    }
                    if (sizes.Count == 2)
                        break;
                }
            }
            var width = sizes[0];
            var height = sizes[1];
            return Tuple.Create(width, height);
        }

        public IEnumerable<Piece> GetPiecesSequence(int width, int height)
        {
            int startObjectCount = 0;
            JsonTextReader jReader;
            StreamReader streamReader;
            using (var fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            using (streamReader = new StreamReader(fileStream))
            using (jReader = new JsonTextReader(streamReader))
            {
                while (jReader.Read())
                {
                    if (jReader.TokenType == JsonToken.StartObject)
                    {
                        startObjectCount++;
                        //Первый токен StartObject не нужен т.к. он относится ко всему объекту полностью.
                        if (startObjectCount > 1)
                        {
                            JObject obj = JObject.Load(jReader);
//                            ImmutableArray<Point> points = obj["Cells"]
                            ImmutableHashSet<Point> points = obj["Cells"]
                                .Select(cell => new Point((int) cell["X"], (int) cell["Y"]))
//                                .ToImmutableArray();
                                .ToImmutableHashSet();
                            yield return new Piece(points, width, height);
                        }
                    }
                    //откатываемся в начало файла
                    if (jReader.TokenType == JsonToken.EndArray)
                    {
                        fileStream.Position = 0;
                        streamReader = new StreamReader(fileStream);
                        jReader = new JsonTextReader(streamReader);
                        startObjectCount = 0;
                    }
                }
            }
        }

        public IEnumerable<string> GetCommandsSequence()
        {
            using (FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            using (StreamReader streamReader = new StreamReader(fileStream))
            using (JsonTextReader jReader = new JsonTextReader(streamReader))
            {
                while (jReader.Read())
                {
                    if (jReader.TokenType == JsonToken.PropertyName && (string) jReader.Value == "Commands")
                    {
                        // чтение происходит кусками по 1024 байта (буфферизация в FileStream и StreamReader)
                        // найдем в последнем считанном 
                        // куске длиной Min(1024, fileStream.Position) самое правое двоеточие
                        // после него (и возможных пробелов) начинается 
                        // токен строки с командами
                        SetStreamPositionOnBeginningStringOfCommands(fileStream);
                        while (true)
                        {
                            var currentByte = fileStream.ReadByte();
                            var currentChar = (char) currentByte;
                            if (currentChar == '"')
                                yield break;
                            yield return currentChar.ToString();
                        }
                    }
                }
            }
        }

        private static void SetStreamPositionOnBeginningStringOfCommands(FileStream fileStream)
        {
            var blockSize = Math.Min(1024, fileStream.Position);
            fileStream.Position -= blockSize;
            var bytesBlock = new byte[blockSize];
            fileStream.Read(bytesBlock, 0, bytesBlock.Length);
            fileStream.Position -= blockSize;
            int i;
            for (i = bytesBlock.Length - 1; i >= 0; i--)
            {
                if ((char) bytesBlock[i] == ':')
                    break;
            }
            fileStream.Position += i;
            while ((char) fileStream.ReadByte() != '"') ;
        }
    }
}
