using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _10
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt");
            var inputLines = File.ReadAllLines(inputFile);

            var adapters = inputLines.Select(x => int.Parse(x)).ToArray();
            Array.Sort(adapters);
            nums = adapters;

            int[] c = new int[4];
            c[3]++;
            int last = 0;
            for (int i = 0; i < adapters.Length; i++)
            {
                int diff = adapters[i] - last;
                last = adapters[i];
                c[diff]++;
            }

            int ans1 = c[1] * c[3];
            Fib();
        }

        static void Fib()
        {
            var fib = new long[nums.Length];
            fib[0] = 1;

            for (int i = 1; i < fib.Length; i++)
            {
                long sum = 0;
                for (int j = i - 3; j < i; j++)
                {
                    if (j < -1) continue;
                    if (j == -1 && nums[i] <= 3)
                    {
                        sum++;
                        continue;
                    }

                    int diff = nums[i] - nums[j];
                    if (diff <= 3)
                    {
                        sum += fib[j];
                    }
                }
                fib[i] = sum;
            }

            Console.WriteLine(fib[fib.Length - 1]);
        }

        static bool Check(int idx, int last)
        {
            if (memo.ContainsKey((idx, last)))
            {
                if (memo[(idx, last)]) ans2++;
                return memo[(idx, last)];
            }

            if (idx == nums.Length)
            {
                if (last == nums[nums.Length - 1])
                {
                    memo.Add((idx, last), true);
                    ans2++;
                    return true;
                }
                memo.Add((idx, last), false);
                return false;
            }


            int diff = nums[idx] - last;
            if (diff > 3)
            {
                memo.Add((idx, last), false);
                return false;
            }

            bool res = Check(idx + 1, nums[idx]);
            if (diff < 3)
            {
                res = Check(idx + 1, last) ? true : res;
            }
            return res;
        }

        static int[] nums;

        static long ans2 = 0;

        static Dictionary<(int, int), bool> memo = new Dictionary<(int, int), bool>();
    }
}
