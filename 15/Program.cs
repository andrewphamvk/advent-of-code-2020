using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _15
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt"));
            var initNums = input.Split(",").Select(x => int.Parse(x)).ToArray();

            var mem = new Dictionary<int, List<int>>();
            for (int i = 0; i < initNums.Length; i++)
            {
                mem.Add(initNums[i], new List<int> { i });
            }

            if (!mem.ContainsKey(0))
            {
                mem.Add(0, new List<int>());
            }

            int lastVal = initNums.Last();
            for (int i = initNums.Length; i < 30000000; i++)
            {
                lastVal = AddNext(mem, lastVal, i);
            }

            System.Console.WriteLine($"{lastVal}");
        }

        static int AddNext(Dictionary<int, List<int>> mem, int lastVal, int idx)
        {
            if (!mem.ContainsKey(lastVal))
            {
                mem.Add(lastVal, new List<int>());
            }

            if (mem[lastVal].Count < 2)
            {
                mem[0].Add(idx);
                return 0;
            }

            int res = mem[lastVal][mem[lastVal].Count - 1] - mem[lastVal][mem[lastVal].Count - 2];
            if (!mem.ContainsKey(res))
            {
                mem.Add(res, new List<int>());
            }
            mem[res].Add(idx);
            return res;
        }
    }
}
