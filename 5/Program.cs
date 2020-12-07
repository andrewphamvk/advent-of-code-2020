using System;
using System.IO;
using System.Reflection;

namespace BinaryBoarding
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt");
            Console.WriteLine("Hello World!");
        }
    }
}
