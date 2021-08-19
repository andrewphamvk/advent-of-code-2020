using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _12
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt");
            var inputLines = File.ReadAllLines(inputFile);

            int dir = 1;
            (int x, int y) point = (0, 0);
            (int x, int y) waypoint = (-1, 10);
            foreach (var line in inputLines)
            {
                char action = line[0];
                int val = int.Parse(line.Substring(1));

                if (action == 'F')
                {
                    point = (point.x + waypoint.x * val, point.y + waypoint.y * val);
                }
                else if (action == 'N')
                {
                    waypoint = (waypoint.x - val, waypoint.y);
                }
                else if (action == 'S')
                {
                    waypoint = (waypoint.x + val, waypoint.y);
                }
                else if (action == 'E')
                {
                    waypoint = (waypoint.x, waypoint.y + val);
                }
                else if (action == 'W')
                {
                    waypoint = (waypoint.x, waypoint.y - val);
                }
                else if (action == 'L')
                {
                    int turns = val / 90;
                    if (turns == 2)
                    {
                        waypoint = (-waypoint.x, -waypoint.y); //
                    }
                    else if (turns == 1)
                    {
                        waypoint = (-waypoint.y, waypoint.x); //
                    }
                    else if (turns == 3)
                    {
                        waypoint = (waypoint.y, -waypoint.x);
                    }
                    dir = (dir + 4 - turns) % 4;
                }
                else if (action == 'R')
                {
                    int turns = val / 90;
                    if (turns == 2)
                    {
                        waypoint = (-waypoint.x, -waypoint.y);
                    }
                    else if (turns == 1)
                    {
                        waypoint = (waypoint.y, -waypoint.x);
                    }
                    else if (turns == 3)
                    {
                        waypoint = (-waypoint.y, waypoint.x);
                    }
                }

                Console.WriteLine($"{point} {dir}");

            }

            // Console.WriteLine(point);
            Console.WriteLine($"{Math.Abs(point.x) + Math.Abs(point.y)}");
        }

        static void PartOne()
        {
            var inputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt");
            var inputLines = File.ReadAllLines(inputFile);
            int dir = 1;
            (int x, int y) point = (0, 0);
            foreach (var line in inputLines)
            {
                char action = line[0];
                int val = int.Parse(line.Substring(1));

                if (action == 'F')
                {
                    if (dir == 0) point = (point.x - val, point.y);
                    else if (dir == 1) point = (point.x, point.y + val);
                    else if (dir == 2) point = (point.x + val, point.y);
                    else if (dir == 3) point = (point.x, point.y - val);
                }
                else if (action == 'N')
                {
                    point = (point.x - val, point.y);
                }
                else if (action == 'S')
                {
                    point = (point.x + val, point.y);
                }
                else if (action == 'E')
                {
                    point = (point.x, point.y + val);
                }
                else if (action == 'W')
                {
                    point = (point.x, point.y - val);
                }
                else if (action == 'L')
                {
                    int turns = val / 90;
                    dir = (dir + 4 - turns) % 4;
                }
                else if (action == 'R')
                {
                    int turns = val / 90;
                    dir = (dir + turns) % 4;
                }

                // Console.WriteLine($"{point} {dir}");

            }

            // Console.WriteLine(point);
            Console.WriteLine($"{Math.Abs(point.x) + Math.Abs(point.y)}");
        }
    }
}
