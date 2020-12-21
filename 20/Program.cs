using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace _20
{
    internal class Program
    {
        private static long p1;
        private static long p2;

        private static int N;

        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "input.txt"));

            PartOne(input);
            PartTwo(input);

            Console.WriteLine($"{p1} {p2}");
        }

        private static void PartOne(string[] input)
        {
            var tiles = new List<Tile>();
            for (var i = 0; i < input.Length; i++)
            {   
                var tileLine = input[i++];
                var match = Regex.Match(tileLine, @"Tile (?<Id>\d+):");

                var tile = new Tile();
                tile.Id = int.Parse(match.Groups["Id"].Value);
                var grid = new List<char[]>();
                while (!string.IsNullOrWhiteSpace(input[i]))
                {
                    grid.Add(input[i].ToCharArray());
                    i++;
                }

                tile.Grid = grid.ToArray();
                tile.Init();
                tiles.Add(tile);
            }

            //var tile0 = tiles[0];
            //Print(tile0.Grid);
            //tile0.Rotate();
            //Console.WriteLine(tile0.Get(Dir.Top));
            //tile0.Rotate();
            //Console.WriteLine(tile0.Get(Dir.Top));
            //tile0.Rotate();
            //Console.WriteLine(tile0.Get(Dir.Top));
            //tile0.Rotate();
            //Console.WriteLine(tile0.Get(Dir.Top));

            //Console.WriteLine();
            //tile0.Flip();

            //Console.WriteLine(tile0.Get(Dir.Top));
            N = (int)Math.Sqrt(tiles.Count);
            var used = new bool[tiles.Count];
            var curr = new Tile[tiles.Count];
            Backtrack(tiles, used, curr, 0);
        }

        private static void Print(char[][] grid)
        {
            var sb = new StringBuilder();
            foreach (var g in grid)
            {
                sb.Append(g);
                sb.AppendLine();
            }

            Console.WriteLine(sb);
            Console.WriteLine();
        }

        private static bool Backtrack(List<Tile> tiles, bool[] used, Tile[] curr, int idx)
        {
            if (idx == tiles.Count)
            {
                Console.WriteLine($"Found {curr[0].Id} {curr[N - 1].Id} {curr[N * (N - 1)].Id} {curr[N * N - 1].Id}");
                p1 = curr[0].Id * (long)curr[N - 1].Id * curr[N * (N - 1)].Id * curr[N * N - 1].Id;
                return true;
            }

            for (var i = 0; i < tiles.Count; i++)
            {
                var tile = tiles[i];
                if (!used[i])
                {
                    used[i] = true;
                    curr[idx] = tile;

                    for (int f = 0; f < 2; f++)
                    {
                        for (int r = 0; r < 4; r++)
                        {
                            bool valid = true;

                            if (idx % N != 0 && curr[idx - 1].Get(Dir.Right) != tile.Get(Dir.Left)) valid = false;
                            if (idx > (N - 1) && curr[idx - N].Get(Dir.Bot) != tile.Get(Dir.Top)) valid = false;
                            if (valid)
                            {
                                if (Backtrack(tiles, used, curr, idx + 1)) return true;
                            }

                            tile.Rotate();
                        }
                        tile.FlipVertical();
                    }


                    curr[idx] = null;
                    used[i] = false;
                }
            }

            return false;
        }

        private static void PartTwo(string[] input)
        {
        }
    }

    public class Tile
    {
        private readonly string[] borders = new string[4];
        public char[][] Grid;
        public int Id;

        public void Init()
        {
            borders[(int) Dir.Top] = new string(Grid[0]);
            borders[(int) Dir.Bot] = new string(Grid[^1]);

            var sbLeft = new StringBuilder();
            var sbRight = new StringBuilder();
            foreach (var r in Grid)
            {
                sbLeft.Append(r[0]);
                sbRight.Append(r[^1]);
            }

            borders[(int) Dir.Left] = sbLeft.ToString();
            borders[(int) Dir.Right] = sbRight.ToString();
        }

        public void Rotate()
        {
            var newTop = Reverse(borders[(int) Dir.Left]);
            var newRight = borders[(int) Dir.Top];
            var newBot = Reverse(borders[(int) Dir.Right]);
            var newLeft = borders[(int) Dir.Bot];

            borders[(int) Dir.Top] = newTop;
            borders[(int) Dir.Right] = newRight;
            borders[(int) Dir.Bot] = newBot;
            borders[(int) Dir.Left] = newLeft;
        }

        private static string Reverse(string s)
        {
            if (!ReverseMap.ContainsKey(s))
            {
                ReverseMap.Add(s, new string(s.Reverse().ToArray()));
            }

            return ReverseMap[s];
        }


        public void FlipHorizontal()
        {
            var newTop = borders[(int) Dir.Bot];
            var newRight = Reverse(borders[(int) Dir.Right]);
            var newBot = borders[(int) Dir.Top];
            var newLeft = Reverse(borders[(int)Dir.Left]);

            borders[(int) Dir.Top] = newTop;
            borders[(int) Dir.Right] = newRight;
            borders[(int) Dir.Bot] = newBot;
            borders[(int) Dir.Left] = newLeft;
        }

        public void FlipVertical()
        {
            var newTop = Reverse(borders[(int)Dir.Top]);
            var newRight = borders[(int)Dir.Left];
            var newBot = Reverse(borders[(int)Dir.Bot]);
            var newLeft = borders[(int)Dir.Right];

            borders[(int)Dir.Top] = newTop;
            borders[(int)Dir.Right] = newRight;
            borders[(int)Dir.Bot] = newBot;
            borders[(int)Dir.Left] = newLeft;
        }

        public string Get(Dir dir)
        {
            return borders[(int) dir];
        }

        private static Dictionary<string, string> ReverseMap = new Dictionary<string, string>();
    }

    public enum Dir
    {
        Top = 0,
        Right = 1,
        Bot = 2,
        Left = 3
    }
}