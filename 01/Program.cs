using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ReportRepair
{
    class Program
    {
        private static int TargetSum = 2020;

        static void Main(string[] args)
        {
            var exeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var inputFile = Path.Combine(exeFolder, "input.txt");

            var lines = File.ReadLines(inputFile);
            var arr = lines.Select(x => int.Parse(x)).ToArray<int>();
            PartOne(arr);
            PartTwo(arr);
        }

        static void PartOne(int[] arr)
        {
            var seenSoFar = new HashSet<int>();

            foreach (var val in arr)
            {
                if (seenSoFar.Contains(TargetSum - val))
                {
                    Console.WriteLine($"{val} * {TargetSum - val} = {val * (TargetSum - val)}");
                    return;
                }
                seenSoFar.Add(val);
            }
        }

        static void PartTwo(int[] arr)
        {
            Array.Sort(arr);
            for (int i = 0; i < arr.Length; i++)
            {
                int newTargetSum = TargetSum - arr[i];
                int left = 0, right = arr.Length - 1;
                while (left < right)
                {
                    if (arr[left] + arr[right] > newTargetSum)
                    {
                        right--;
                    }
                    else if (arr[left] + arr[right] < newTargetSum)
                    {
                        left++;
                    }
                    else
                    {
                        int res = arr[i] * arr[left] * arr[right];
                        Console.WriteLine($"{arr[i]} * {arr[left]} * {arr[right]} = {res}");
                        return;
                    }
                }
            }
        }
    }
}
