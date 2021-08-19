using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace _20
{
    internal class Program
    {
        private static long p1;
        private static long p2;

        private static int N;
        private static Tile[] _p1Tiles;

        private static void Main(string[] args)
        {
            var list = new List<int>();
            list.Add(1);
            
            int index = list.BinarySearch(2);
            if (index < 0)
            {
                list.Insert(~index, 1);
            }


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
                _p1Tiles = curr;
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
                        tile.FlipVertical();
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
                    }


                    curr[idx] = null;
                    used[i] = false;
                }
            }

            return false;
        }

        private static void PartTwo(string[] input)
        {
            var N = (int)Math.Sqrt(_p1Tiles.Length);

            var tileN = (_p1Tiles[0].Grid.Length - 2);
            var tileM = (_p1Tiles[0].Grid[0].Length - 2);

            var gridN = N * tileN;
            var gridM = N * tileM;

            var tileGrid = new Tile[N][];
            for (int i = 0; i < tileGrid.Length; i++)
            {
                tileGrid[i] = new Tile[N];
            }

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    int idx = (i * N) + j;
                    var tile = _p1Tiles[idx];
                    tileGrid[i][j] = tile;
                }
            }

            var endGrid = new List<char[]>();

            for (int i = 0; i < tileGrid.Length; i++)
            {
                var sb = new StringBuilder();
                for (int l1 = 1; l1 < 9; l1++)
                {
                    for (int j = 0; j < tileGrid[0].Length; j++)
                    {
                        for (int l2 = 1; l2 < 9; l2++)
                        {
                            sb.Append(tileGrid[i][j].Grid[l1][l2]);
                        }
                    }

                    //Console.WriteLine(sb.ToString());
                    endGrid.Add(sb.ToString().ToCharArray());
                    sb.Clear();
                }
            }

            var grid = endGrid.ToArray();
            Print(grid);
            Console.WriteLine();
            Console.WriteLine();

            var toCheck = new List<char[][]>();
            for (int r = 0; r < 4; r++)
            {
                toCheck.Add(Rotate(grid, r));
            }
            grid = FlipX(grid);
            for (int r = 0; r < 4; r++)
            {
                toCheck.Add(Rotate(grid, r));
            }

            var monsterCount = 0;
            foreach (var toCheckGrid in toCheck)
            {
                Print(toCheckGrid);
                Console.WriteLine();

                monsterCount += GetMonsterCount(toCheckGrid);
            }

            var monsterCells = monsterCount * 15;
            var totalHashCount = 0;
            for (var x = 0; x < grid.Length; x++)
            {
                for (var y = 0; y < grid[0].Length; y++)
                {
                    totalHashCount += grid[y][x] == '#' ? 1 : 0;
                }
            }

            p2 = totalHashCount - monsterCells;
        }

        private static char[][] FlipX(char[][] grid)
        {
            var newGrid = CloneGrid(grid);

            var height = grid.GetLength(0);
            var width = grid[0].Length;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width / 2; x++)
                {
                    var temp = newGrid[y][ (width - 1) - x];
                    newGrid[y][ (width - 1) - x] = newGrid[y][x];
                    newGrid[y][ x] = temp;
                }
            }
            return newGrid;
        }

        private static char[][] CloneGrid(char[][] grid)
        {
            var newGrid = new char[grid.Length][];
            for (int i = 0; i < grid.Length; i++)
            {
                newGrid[i] = new char[grid[0].Length];
                for (int j = 0; j < grid[0].Length; j++)
                {
                    newGrid[i][j] = grid[i][j];
                }
            }

            return newGrid;
        }

        private static char[][] Rotate(char[][] grid, int rotateCount)
        {
            var newGrid = CloneGrid(grid);
            var N = newGrid.GetLength(0);
            for (var z = 0; z < rotateCount; z++)
            {
                // Consider all 
                // squares one by one 
                for (int x = 0; x < N / 2; x++)
                {
                    // Consider elements 
                    // in group of 4 in 
                    // current square 
                    for (int y = x; y < N - x - 1; y++)
                    {
                        // store current cell 
                        // in temp variable 
                        char temp = newGrid[x][y];

                        // move values from 
                        // right to top 
                        newGrid[x][y] = newGrid[y][N - 1 - x];

                        // move values from 
                        // bottom to right 
                        newGrid[y][N - 1 - x] = newGrid[N - 1 - x][N - 1 - y];

                        // move values from 
                        // left to bottom 
                        newGrid[N - 1 - x][N - 1 - y] = newGrid[N - 1 - y][x];

                        // assign temp to left 
                        newGrid[N - 1 - y][x] = temp;
                    }
                }
            }

            return newGrid;
        }

        private static int GetMonsterCount(char[][] grid)
        {
            var monsterMask = new (int x, int y)[]
            {
                (18,0),
                (0,1),
                (5,1),
                (6,1),
                (11,1),
                (12,1),
                (17,1),
                (18,1),
                (19,1),
                (1,2),
                (4,2),
                (7,2),
                (10,2),
                (13,2),
                (16,2)
            };

            var monsterWidth = 20;
            var monsterHeight = 3;

            var height = grid.GetLength(0);
            var width = height;

            var monsterCount = 0;
            for (int x = 0; x < width - monsterWidth; x++)
            {
                for (int y = 0; y < height - monsterHeight; y++)
                {
                    monsterCount += monsterMask.All(p => grid[y + p.y][x + p.x] == '#') ? 1 : 0;
                }
            }

            return monsterCount;
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
            /*
            var newTop = Reverse(borders[(int) Dir.Left]);
            var newRight = borders[(int) Dir.Top];
            var newBot = Reverse(borders[(int) Dir.Right]);
            var newLeft = borders[(int) Dir.Bot];

            borders[(int) Dir.Top] = newTop;
            borders[(int) Dir.Right] = newRight;
            borders[(int) Dir.Bot] = newBot;
            borders[(int) Dir.Left] = newLeft;
            */

            var N = Grid.Length;
            // Consider all 
            // squares one by one 
            for (int x = 0; x < N / 2; x++)
            {
                // Consider elements 
                // in group of 4 in 
                // current square 
                for (int y = x; y < N - x - 1; y++)
                {
                    // store current cell 
                    // in temp variable 
                    char temp = Grid[x][y];

                    // move values from 
                    // right to top 
                    Grid[x][y] = Grid[y][N - 1 - x];

                    // move values from 
                    // bottom to right 
                    Grid[y][N - 1 - x] = Grid[N - 1 - x][N - 1 - y];

                    // move values from 
                    // left to bottom 
                    Grid[N - 1 - x][N - 1 - y] = Grid[N - 1 - y][x];

                    // assign temp to left 
                    Grid[N - 1 - y][x] = temp;
                }
            }

            Init();
        }

        private static string Reverse(string s)
        {
            if (!ReverseMap.ContainsKey(s))
            {
                ReverseMap.Add(s, new string(s.Reverse().ToArray()));
            }

            return ReverseMap[s];
        }

        public void FlipVertical()
        {
            /*
            var newTop = Reverse(borders[(int)Dir.Top]);
            var newRight = borders[(int)Dir.Left];
            var newBot = Reverse(borders[(int)Dir.Bot]);
            var newLeft = borders[(int)Dir.Right];

            borders[(int)Dir.Top] = newTop;
            borders[(int)Dir.Right] = newRight;
            borders[(int)Dir.Bot] = newBot;
            borders[(int)Dir.Left] = newLeft;
            */

            var height = Grid.Length;
            var width = Grid[0].Length;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width / 2; x++)
                {
                    var temp = Grid[y][(width - 1) - x];
                    Grid[y][(width - 1) - x] = Grid[y][x];
                    Grid[y][x] = temp;
                }
            }

            Init();
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