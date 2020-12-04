using System;
using System.IO;
using System.Reflection;

namespace TobogganTrajectory
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "input.txt"
            );

            var input = File.ReadAllLines(inputFile);
            PartOne(input);
            PartTwo(input);
        }

        static void PartOne(string[] input)
        {
            Console.WriteLine(TreesHit(input, 1, 3));
        }

        static void PartTwo(string[] input)
        {
            var res = TreesHit(input, 1, 1)
                    * TreesHit(input, 1, 3)
                    * TreesHit(input, 1, 5)
                    * TreesHit(input, 1, 7)
                    * TreesHit(input, 2, 1);

            Console.WriteLine(res);
        }

        static long TreesHit(string[] input, int down, int right)
        {
            int treesHit = 0;
            int repeatLength = input[0].Length;
            int offset = 0;
            for (int i = down; i < input.Length; i += down)
            {
                offset += right;
                offset %= repeatLength;
                if (input[i][offset] == '#')
                {
                    treesHit++;
                }
            }

            return treesHit;
        }
    }
}
