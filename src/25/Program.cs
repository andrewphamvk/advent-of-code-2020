using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _25
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

        }

        static void PartTwo(string[] input)
        {

        }
    }
}
