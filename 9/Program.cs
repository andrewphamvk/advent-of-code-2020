using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _9
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt");
            var inputLines = File.ReadAllLines(inputFile);

            var nums = inputLines.Select(x => long.Parse(x)).ToArray();
            var window = new Dictionary<long, int>();

            long target = 0;
            int maxTargetIdx = 0;
            int preambleSize = 25;
            for (int i = 0; i < preambleSize; i++)
            {
                if (!window.ContainsKey(nums[i])) window.Add(nums[i], 0);
                window[nums[i]]++;
            }

            for (int i = preambleSize; i < nums.Length; i++)
            {
                if (!WindowContainsTwoNums(window, nums[i]))
                {
                    target = nums[i];
                    maxTargetIdx = i;
                    break;
                }

                int indexToRemove = i - preambleSize;
                window[nums[indexToRemove]]--;
                if (window[nums[indexToRemove]] == 0) window.Remove(nums[indexToRemove]);

                window.Add(nums[i], 1);
            }

            long currsum = 0;
            int idx = 0;
            int left = 0, right = 0;
            for (int i = 0; i < maxTargetIdx; i++)
            {
                currsum += nums[i];
                if (currsum == target)
                {
                    left = idx;
                    right = i;
                    break;
                }
                while (currsum > target)
                {
                    currsum -= nums[idx];
                    idx++;
                }
            }


            long min = int.MaxValue;
            long max = int.MinValue;
            for (int i = left; i <= right; i++)
            {
                min = Math.Min(min, nums[i]);
                max = Math.Max(max, nums[i]);
            }

            Console.WriteLine($"{min + max}");
        }

        static bool WindowContainsTwoNums(Dictionary<long, int> window, long k)
        {
            foreach (var w in window.Keys)
            {
                if (k == 55)
                {
                    Console.WriteLine(w);
                }
                if (w > k) continue;
                if (window.ContainsKey(k - w)) return true;
            }

            return false;
        }
    }
}
