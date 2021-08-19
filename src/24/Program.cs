using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _24
{
    class Program
    {
        private static int p1 = default;
        private static int p2 = default;
        private static HashSet<(double x, double y)> _tiles = new HashSet<(double, double)>();
        private static List<double[]> dirs = new List<double[]> 
        {
            new [] { -1.0, 0 },
            new [] { 1.0, 0 },
            new [] { -0.5, 1 },
            new [] { -0.5, -1 },
            new [] { 0.5, 1 },
            new [] { 0.5, -1 },
        };

        private static double minX = 10000;
        private static double maxX = -10000;
        private static double minY = 10000;
        private static double maxY = -10000;

        static void Main(string[] args)
        {
            var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt"));

            PartOne(input);
            PartTwo(input);

            Console.WriteLine($"{p1} {p2}");
        }

        static void PartOne(string[] input)
        {
            foreach (var line in input)
            {
                var moves = GetMoves(line);
                double x = 0, y = 0;
                foreach (var move in moves)
                {
                    switch (move)
                    {
                        case Move.West:
                            x--;
                            break;
                        case Move.East:
                            x++;
                            break;
                        case Move.NorthWest:
                            y--;
                            x -= 0.5;
                            break;
                        case Move.NorthEast:
                            y--;
                            x += 0.5;
                            break;
                        case Move.SouthWest:
                            y++;
                            x -= 0.5;
                            break;
                        case Move.SouthEast:
                            y++;
                            x += 0.5;
                            break;
                    }
                }

                if (_tiles.Contains((x, y)))
                {
                    _tiles.Remove((x, y));
                }
                else
                {
                    _tiles.Add((x, y));
                }

                SetMinMax(x, y);
            }

            p1 = _tiles.Count;
        }

        static List<Move> GetMoves(string line)
        {
            var moves = new List<Move>();
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == 'e')
                {
                    moves.Add(Move.East);
                }
                else if (line[i] == 'w')
                {
                    moves.Add(Move.West);
                }
                else if (line[i] == 's')
                {
                    i++;
                    if (line[i] == 'e') moves.Add(Move.SouthEast);
                    else if (line[i] == 'w') moves.Add(Move.SouthWest);
                }
                else if (line[i] == 'n')
                {
                    i++;
                    if (line[i] == 'e') moves.Add(Move.NorthEast);
                    else if (line[i] == 'w') moves.Add(Move.NorthWest);
                }
            }

            return moves;
        }

        static void PartTwo(string[] input)
        {
            for (int i = 0; i < 100; i++)
            {
                var toFlip = new List<(double x, double y)>();
                double tempMinX = minX - 1;
                double tempMaxX = maxX + 1;
                double tempMinY = minY - 1;
                double tempMaxY = maxY + 1;

                for (double y = tempMinY; y <= tempMaxY; y++)
                {
                    double x = tempMinX;
                    // If its an even row and the minX is off by 0.5
                    if (y % 2 == 0 && x != (int) x)
                    {
                        x -= 0.5;
                    }
                    else if (y % 2 != 0 && x == (int) x)
                    {
                        x -= 0.5;
                    }

                    while (x < tempMaxX)
                    {
                        var neighbours = GetNeighbours((x, y));
                        var blackNeighbours = neighbours.Count(n => _tiles.Contains(n));
                        if (_tiles.Contains((x, y)) && (blackNeighbours == 0 || blackNeighbours > 2))
                        {
                            toFlip.Add((x, y));
                        }
                        else if (!_tiles.Contains((x, y)) && blackNeighbours == 2)
                        {
                            toFlip.Add((x, y));
                        }

                        SetMinMax(x, y);

                        x++;
                    }
                }

                foreach (var f in toFlip)
                {
                    if (_tiles.Contains(f))
                    {
                        _tiles.Remove(f);
                    }
                    else
                    {
                        _tiles.Add(f);
                    }
                }
            }

            p2 = _tiles.Count;
        }

        private static List<(double x, double y)> GetNeighbours((double x, double y) tile)
        {
            var neighours = new List<(double x, double y)>();
            foreach (var dir in dirs)
            {
                var nx = tile.x + dir[0];
                var ny = tile.y + dir[1];
                neighours.Add((nx, ny));
            }

            return neighours;
        }

        private static void SetMinMax(double x, double y)
        {
            minX = Math.Min(minX, x);
            maxX = Math.Max(maxX, x);
            minY = Math.Min(minY, y);
            maxY = Math.Max(maxY, y);
        }
    }

    internal enum Move
    {
        East = 0,
        SouthEast = 1,
        SouthWest = 2,
        West = 3,
        NorthWest = 4,
        NorthEast = 5

    }
}
