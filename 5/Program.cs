<<<<<<< HEAD
﻿using System;

namespace BinaryBoarding
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt");
            var inputLines = File.ReadAllLines(inputFile);
            
            Console.WriteLine("Hello World!");
        }
    }
}
=======
﻿using System;
using System.IO;
using System.Reflection;

namespace BinaryBoarding
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt");
            var inputLines = File.ReadAllLines(inputFile);
            int max = 0;
            bool[] taken = new bool[1016];
            foreach (var line in inputLines)
            {
                int row = GetIndex(line.Substring(0, 7));
                int col = GetIndex(line.Substring(7, 3));
                int seatIndex = (row * 8) + col;
                max = Math.Max(max, seatIndex);
                taken[seatIndex] = true;
            }

            Console.WriteLine(max);

            for (int i = 0; i < 1016; i++)
            {
                if (!taken[i]) Console.WriteLine(i);
            }
        }

        static int GetIndex(string input)
        {
            int res = 0;
            int offset = (int)Math.Pow(2, input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                offset /= 2;
                if (input[i] == 'B' || input[i] == 'R')
                {
                    res += offset;
                }
            }

            return res;
        }
    }
}
>>>>>>> 30dab8c9bbb8584faa402f63b4a87cc51d283d70
