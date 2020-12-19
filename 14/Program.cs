using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


namespace _14
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt");
            var inputLines = File.ReadAllLines(inputFile);

            long p1 = 0;
            long p2 = 0;

            var memory = new Dictionary<long, long>();

            // Part 1
            string mask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            for (int i = 0; i < inputLines.Length; i++)
            {
                if (inputLines[i].StartsWith("mask"))
                {
                    mask = inputLines[i].Split(" = ")[1];
                }
                else
                {
                    var split = inputLines[i].Split(" = ");

                    int start = split[0].IndexOf('[') + 1;
                    int memLocation = int.Parse(split[0].Substring(start, split[0].Length - start - 1));

                    long val = long.Parse(split[1]);
                    long newValue = GetNewValue(val, mask);

                    memory[memLocation] = newValue;
                }
            }

            foreach (var val in memory.Values) p1 += val;

            // Part 2
            mask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            for (int i = 0; i < inputLines.Length; i++)
            {
                if (inputLines[i].StartsWith("mask"))
                {
                    mask = inputLines[i].Split(" = ")[1];
                }
                else
                {
                    var split = inputLines[i].Split(" = ");
                    int start = split[0].IndexOf('[') + 1;
                    int memLocation = int.Parse(split[0].Substring(start, split[0].Length - start - 1));

                    long val = long.Parse(split[1]);

                    var addresses = GetAddresses(memLocation, mask);
                    foreach (var address in addresses)
                    {
                        memory[address] = val;
                    }
                }
            }

            foreach (var val in memory.Values) p2 += val;

            Console.WriteLine($"{p1} {p2}");
        }

        static List<long> GetAddresses(long address, string mask)
        {
            var floats = new List<int>();
            for (int i = 0; i < mask.Length; i++)
            {
                int pos = mask.Length - 1 - i;
                if (mask[i] == 'X')
                {
                    floats.Add(pos);
                }
                else if (mask[i] == '1')
                {
                    address |= ((long)1 << pos);
                }
            }

            var res = new List<long>();
            Backtrack(address, floats, 0, res);
            return res;
        }

        static void Backtrack(long address, List<int> floats, int start, List<long> res)
        {
            if (start == floats.Count)
            {
                res.Add(address);
                return;
            }

            // 1
            Backtrack(address | ((long)1 << floats[start]), floats, start + 1, res);
            // 0
            Backtrack(address & ~((long)1 << floats[start]), floats, start + 1, res);
        }

        static long GetNewValue(long val, string mask)
        {
            long newValue = 0;
            for (int i = 0; i < mask.Length; i++)
            {
                int pos = mask.Length - 1 - i;
                if (mask[i] == 'X')
                {
                    newValue |= val & ((long)1 << pos);
                }
                else if (mask[i] == '1')
                {
                    newValue |= ((long)1 << pos);
                }
                else if (mask[i] == '0')
                {
                    newValue &= ~((long)1 << pos);
                }
            }

            return newValue;
        }
    }
}
