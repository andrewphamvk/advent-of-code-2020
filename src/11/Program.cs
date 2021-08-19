using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _11
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt");
            var inputLines = File.ReadAllLines(inputFile);
            var grid = inputLines.Select(x => x.ToCharArray()).ToArray();
            var nextGrid = grid.Clone();
            int M = inputLines.Length;
            int N = inputLines[0].Length;
            int rounds = 0;

            while (Process(grid, M, N))
            {
                rounds++;
                Print(grid);
            }

            Print(grid);
        }

        static void Print(char[][] grid)
        {
            int count = 0;
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[0].Length; j++)
                {
                    if (grid[i][j] == '#') count++;
                }
                // Console.WriteLine(grid[i]);
            }

            Console.WriteLine($"Count {count}");
            Console.WriteLine();
        }

        static bool Process(char[][] grid, int M, int N)
        {
            bool changed = false;
            var changeList = new List<(int x, int y)>();
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (grid[i][j] == '.') continue;

                    int adj = CountAdj(grid, i, j);
                    // foreach (var dir in dirs)
                    // {
                    //     int r = i + dir[0];
                    //     int c = j + dir[1];
                    //     adj += IsAdj(grid, r, c);
                    // }

                    if (grid[i][j] == 'L' && adj == 0)
                    {
                        changed = true;
                        // grid[i][j] = '#';
                        changeList.Add((i, j));
                    }
                    else if (grid[i][j] == '#' && adj >= 5)
                    {
                        changed = true;
                        changeList.Add((i, j));
                        // grid[i][j] = 'L';
                    }
                }
            }

            foreach (var change in changeList)
            {
                if (grid[change.x][change.y] == 'L')
                {
                    grid[change.x][change.y] = '#';
                }
                else
                {
                    grid[change.x][change.y] = 'L';
                }
            }

            return changed;
        }

        static int IsAdj(char[][] grid, int r, int c)
        {
            if (r < 0 || r >= grid.Length || c < 0 || c >= grid[0].Length) return 0;

            if (grid[r][c] == '#') return 1;
            return 0;
        }

        static int CountAdj(char[][] grid, int row, int col)
        {
            int adj = 0;
            // Up
            int r = row - 1, c = col;
            char ch = 'z';
            while (r >= 0)
            {
                ch = grid[r--][c];
                if (ch != '.')
                {
                    if (ch == '#') adj++;

                    break;
                }
            }

            // Right
            r = row; c = col + 1;
            while (c < grid[0].Length)
            {
                ch = grid[r][c++];
                if (ch != '.')
                {
                    if (ch == '#') adj++;
                    break;
                }
            }

            // Down
            r = row + 1; c = col;
            while (r < grid.Length)
            {
                ch = grid[r++][c];
                if (ch != '.')
                {
                    if (ch == '#') adj++;
                    break;
                }
            }

            // Left
            r = row; c = col - 1;
            while (c >= 0)
            {
                ch = grid[r][c--];
                if (ch != '.')
                {
                    if (ch == '#') adj++;

                    break;
                }
            }

            // TopLeft
            r = row - 1; c = col - 1;
            while (r >= 0 && c >= 0)
            {
                ch = grid[r--][c--];
                if (ch != '.')
                {
                    if (ch == '#') adj++;

                    break;
                }
            }

            // TopRight
            r = row - 1; c = col + 1;
            while (r >= 0 && c < grid[0].Length)
            {
                ch = grid[r--][c++];
                if (ch != '.')
                {
                    if (ch == '#') adj++;

                    break;
                }
            }

            // BotRight
            r = row + 1; c = col + 1;
            while (r < grid.Length && c < grid[0].Length)
            {
                ch = grid[r++][c++];
                if (ch != '.')
                {
                    if (ch == '#') adj++;
                    break;
                }
            }

            // BotLeft
            r = row + 1; c = col - 1;
            while (r < grid.Length && c >= 0)
            {
                ch = grid[r++][c--];
                if (ch != '.')
                {
                    if (ch == '#') adj++;
                    break;
                }
            }

            return adj;
        }

        private static List<int[]> dirs = new List<int[]>
        {
            new [] { -1, -1 },
            new [] { -1, 0 },
            new [] { -1, 1 },
            new [] { 0, -1 },
            new [] { 0, 1 },
            new [] { 1, -1 },
            new [] { 1, 0 },
            new [] { 1, 1 },
        };
    }
}
