using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;

namespace _20
{
    class Program
    {
        private static int p1 = default;
        private static int p2 = default;

        static void Main(string[] args)
        {
            var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt"));

            PartOne(input);
            PartTwo(input);

            Console.WriteLine($"{p1} {p2}");
        }

        static void PartOne(string[] input)
        {
            var tiles = new List<Tile>();
            for (int i = 0; i < input.Length; i++)
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

            var tile0 = tiles[0];
            Print(tile0.Grid);
            tile0.Rotate(0);
            Console.WriteLine(tile0.Get(Dir.Top));
            tile0.Rotate(1);
            Console.WriteLine(tile0.Get(Dir.Top));
            tile0.Rotate(2);
            Console.WriteLine(tile0.Get(Dir.Top));
            tile0.Rotate(3);
            Console.WriteLine(tile0.Get(Dir.Top));

            Console.WriteLine();
            tile0.Flip(true);

            tile0.Rotate(0);
            Console.WriteLine(tile0.Get(Dir.Top));
            tile0.Rotate(1);
            Console.WriteLine(tile0.Get(Dir.Top));
            tile0.Rotate(2);
            Console.WriteLine(tile0.Get(Dir.Top));
            tile0.Rotate(3);
            Console.WriteLine(tile0.Get(Dir.Top));

            bool[] used = new bool[tiles.Count];
            var curr = new Tile[tiles.Count];
            Backtrack(tiles, used, curr, 0);

        }

        static void Print(char[][] grid)
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

        static bool Backtrack(List<Tile> tiles, bool[] used, Tile[] curr, int idx) 
        {
            if (idx == tiles.Count) {
                System.Console.WriteLine($"Found {curr[0].Id} {curr[11].Id} {curr[132].Id} {curr[143].Id}");
                return true;
            }

            p2 = Math.Max(p2, idx);

            for (int i = 0; i < tiles.Count; i++)
            {
                var tile = tiles[i];
                if (!used[i])
                {
                    used[i] = true;
                    curr[idx] = tile;
                    // Try rotating
                    for (int r = 0; r < 4; r++)
                    {
                        tile.Rotate(r);
                        for (int f = 0; f < 2; f++)
                        {
                            bool flip = f == 1;
                            tile.Flip(flip);

                            if (idx % 12 != 0 && curr[idx - 1].Get(Dir.Right) != tile.Get(Dir.Left))
                            {
                                continue;
                            }

                            if (idx > 11 && curr[idx - 12].Get(Dir.Bot) != tile.Get(Dir.Top))
                            {
                                continue;
                            }


                            if (Backtrack(tiles, used, curr, idx + 1)) return true;

                        }
                    }

                    tile.Rotate(0);
                }

                curr[idx] = null;
                used[i] = false;
            }

            return false;
        }

        static void PartTwo(string[] input)
        {

        }
    }

    public class Tile
    {
        public int Id;
        public char[][] Grid;
        private int Rotation = 0;
        private bool flip = false;
        private string[] borders = new string[4];

        public void Init()
        {
            borders[(int)Dir.Top] = new String(this.Grid[0]);
            borders[(int)Dir.Bot] = new String(this.Grid[^1]);

            var sbLeft = new StringBuilder();
            var sbRight = new StringBuilder();
            foreach (var r in this.Grid)
            {
                sbLeft.Append(r[0]);
                sbRight.Append(r[^1]);
            }

            borders[(int)Dir.Left] = sbLeft.ToString();
            borders[(int)Dir.Right] = sbRight.ToString();
        }

        public void Rotate(int num)
        {
            Rotation = num;
            //Rotation = ((Rotation + 4) + num) % 4;
        }

        public void Flip(bool flip)
        {
            this.flip = flip;
        }

        public string Get(Dir dir)
        {
            int tempRotation = Rotation;
            if (this.flip) tempRotation += 2;
            int dirInt = (int)dir;
            dirInt += tempRotation;

            string res;
            dirInt = dirInt % 4;

            if (tempRotation == 1 && (dirInt == (int)Dir.Top || dirInt == (int)Dir.Bot))
            {
                res = new string(borders[dirInt].Reverse().ToArray());
            }
            else if (tempRotation == 2)
            {
                res = new string(borders[dirInt].Reverse().ToArray());
            }
            else if (tempRotation == 3 && (dirInt == (int)Dir.Left || dirInt == (int)Dir.Right))
            {
                res = new string(borders[dirInt].Reverse().ToArray());
            }
            else
            {
                res = borders[dirInt];
            }

            if (this.flip) res = new string(res.Reverse().ToArray());

            return res;
        }
    }

    public enum Dir
    {
        Top = 0,
        Right = 1,
        Bot = 2,
        Left = 3
    }
}
