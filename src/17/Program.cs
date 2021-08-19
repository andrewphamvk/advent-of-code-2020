using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _17
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
            int xyMin = 0;
            int xyMax = input.Length - 1;
            int zMin = 0;
            int zMax = 0;

            var cube = new Dictionary<(int x, int y, int z), bool>();
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    bool power = input[y][x] == '#';
                    cube.Add((x, y, 0), power);
                }
            }

            for (int c = 0; c < 6; c++)
            {
                xyMin--;
                xyMax++;
                zMin--;
                zMax++;

                // System.Console.WriteLine($"xyMin {xyMin} xyMax {xyMax} zMin {zMin} zMax {zMax}");
                var pointsToSet = new List<(int x, int y, int z, bool power)>();

                for (int y = xyMin; y <= xyMax; y++)
                {
                    for (int x = xyMin; x <= xyMax; x++)
                    {
                        for (int z = zMin; z <= zMax; z++)
                        {
                            if (ShouldFlip(cube, x, y, z))
                            {
                                if (!cube.ContainsKey((x, y, z))) cube.Add((x, y, z), false);
                                pointsToSet.Add((x, y, z, !cube[(x, y, z)]));
                            }
                        }
                    }
                }

                foreach (var point in pointsToSet)
                {
                    cube[(point.x, point.y, point.z)] = point.power;
                }


            }

            foreach (var c in cube.Values)
            {
                p1 += c ? 1 : 0;
            }
        }

        private static bool ShouldFlip(Dictionary<(int x, int y, int z), bool> cube, int x, int y, int z)
        {
            int activeNeighbors = 0;
            if (cube.ContainsKey((x, y, z)) && cube[(x, y, z)]) activeNeighbors--;

            for (int nx = x - 1; nx <= x + 1; nx++)
            {
                for (int ny = y - 1; ny <= y + 1; ny++)
                {
                    for (int nz = z - 1; nz <= z + 1; nz++)
                    {
                        if (cube.ContainsKey((nx, ny, nz)) && cube[(nx, ny, nz)])
                        {
                            activeNeighbors++;
                        }
                    }
                }
            }

            bool shouldFlip = false;
            if (cube.ContainsKey((x, y, z)) && cube[(x, y, z)])
            {
                if (activeNeighbors != 2 && activeNeighbors != 3)
                {
                    shouldFlip = true;
                }
            }
            else
            {
                if (activeNeighbors == 3)
                {
                    shouldFlip = true;
                }
            }

            return shouldFlip;
        }

        static void PartTwo(string[] input)
        {
            int xyMin = 0;
            int xyMax = input.Length - 1;
            int zMin = 0;
            int zMax = 0;
            int wMin = 0;
            int wMax = 0;

            var cube = new Dictionary<(int x, int y, int z, int w), bool>();
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    bool power = input[y][x] == '#';
                    cube.Add((x, y, 0, 0), power);
                }
            }

            for (int c = 0; c < 6; c++)
            {
                xyMin--;
                xyMax++;
                zMin--;
                zMax++;
                wMin--;
                wMax++;

                // System.Console.WriteLine($"xyMin {xyMin} xyMax {xyMax} zMin {zMin} zMax {zMax}");
                var pointsToSet = new List<(int x, int y, int z, int w, bool power)>();

                for (int y = xyMin; y <= xyMax; y++)
                {
                    for (int x = xyMin; x <= xyMax; x++)
                    {
                        for (int z = zMin; z <= zMax; z++)
                        {
                            for (int w = wMin; w <= wMax; w++)
                            {
                                if (ShouldFlip2(cube, x, y, z, w))
                                {
                                    if (!cube.ContainsKey((x, y, z, w))) cube.Add((x, y, z, w), false);
                                    pointsToSet.Add((x, y, z, w, !cube[(x, y, z, w)]));
                                }
                            }
                        }
                    }
                }

                foreach (var point in pointsToSet)
                {
                    cube[(point.x, point.y, point.z, point.w)] = point.power;
                }


            }

            foreach (var c in cube.Values)
            {
                p2 += c ? 1 : 0;
            }
        }


        private static bool ShouldFlip2(Dictionary<(int x, int y, int z, int w), bool> cube, int x, int y, int z, int w)
        {
            int activeNeighbors = 0;
            if (cube.ContainsKey((x, y, z, w)) && cube[(x, y, z, w)]) activeNeighbors--;

            for (int nx = x - 1; nx <= x + 1; nx++)
            {
                for (int ny = y - 1; ny <= y + 1; ny++)
                {
                    for (int nz = z - 1; nz <= z + 1; nz++)
                    {
                        for (int nw = w - 1; nw <= w + 1; nw++)
                        {
                            if (cube.ContainsKey((nx, ny, nz, nw)) && cube[(nx, ny, nz, nw)])
                            {
                                activeNeighbors++;
                            }

                        }
                    }
                }
            }

            bool shouldFlip = false;
            if (cube.ContainsKey((x, y, z, w)) && cube[(x, y, z, w)])
            {
                if (activeNeighbors != 2 && activeNeighbors != 3)
                {
                    shouldFlip = true;
                }
            }
            else
            {
                if (activeNeighbors == 3)
                {
                    shouldFlip = true;
                }
            }

            return shouldFlip;
        }
    }
}
