using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _13
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt");
            var inputLines = File.ReadAllLines(inputFile);
            int arriveAt = int.Parse(inputLines[0]);
            var busInput = inputLines[1].Split(',', StringSplitOptions.RemoveEmptyEntries).ToArray();
            // Array.Sort(buses);

            // int minBus = -1;
            // int minsToWait = int.MaxValue;
            // foreach (var bus in buses)
            // {
            //     if (arriveAt % bus == 0)
            //     {
            //         minBus = bus;
            //         minsToWait = 0;
            //         break;
            //     }

            //     int wait = (((arriveAt / bus) + 1) * bus) - arriveAt;
            //     if (wait < minsToWait)
            //     {
            //         minBus = bus;
            //         minsToWait = wait;
            //     }
            // }

            // int res = minBus * minsToWait;
            // Console.WriteLine(minBus);
            // Console.WriteLine(minsToWait);
            // Console.WriteLine(res);

            // var buses = inputLines[1].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => int.TryParse(x, out _) ? int.Parse(x) : 0).ToArray();
            var buses = new List<(int bus, int offset)>();
            var n = new List<long>();
            var a = new List<long>();


            for (int i = 0; i < busInput.Length; i++)
            {
                if (int.TryParse(busInput[i], out var x))
                {
                    n.Add(x);
                    a.Add(x - i);
                    buses.Add((x, i));
                    // if (i == 0)
                    // {
                    //     buses.Add((x, i));
                    // }
                    // else
                    // {
                    //     buses.Add((x, i % buses[0].bus));
                    // }
                }
            }

            // foreach (var bus in buses)
            // {
            //     Console.WriteLine(bus);
            // }
            // Console.WriteLine();

            // long t = 0;
            // while (true)
            // {
            //     t += buses[0].bus;
            //     if (Check(t, buses))
            //     {
            //         Console.WriteLine(t);
            //         break;
            //     }
            // }

            var res = ChineseRemainderTheorem.Solve(n.ToArray(), a.ToArray());
            Console.WriteLine(res);
        }

        static bool Check(long t, List<(int bus, int offset)> buses)
        {
            for (int i = 1; i < buses.Count; i++)
            {
                long wait = (((t / buses[i].bus) + 1) * buses[i].bus) - t;
                // if (buses[i].bus == 59) Console.WriteLine(wait);
                if (wait != buses[i].offset)
                {
                    return false;
                }
            }

            return true;
        }
    }

    public static class ChineseRemainderTheorem
    {
        public static long Solve(long[] n, long[] a)
        {
            var prod = n.Aggregate(1, (long i, long j) => i * j);
            long p;
            long sm = 0;
            for (int i = 0; i < n.Length; i++)
            {
                p = prod / n[i];
                sm += a[i] * ModularMultiplicativeInverse(p, n[i]) * p;
            }
            return sm % prod;
        }

        private static int ModularMultiplicativeInverse(long a, long mod)
        {
            long b = a % mod;
            for (int x = 1; x < mod; x++)
            {
                if ((b * x) % mod == 1)
                {
                    return x;
                }
            }
            return 1;
        }
    }
}
