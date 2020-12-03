using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PasswordPhilosophy
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
            bool IsValid(int left, int right, char letter, string password)
            {
                int countLetter = 0;
                foreach (var ch in password)
                {
                    if (ch == letter) countLetter++;
                }

                if (countLetter >= left && countLetter <= right) return true;
                return false;
            }

            ParseAndValidate(input, IsValid);
        }

        static void PartTwo(string[] input)
        {
            bool IsValid(int left, int right, char letter, string password)
            {
                char char1 = password[left - 1];
                char char2 = password[right - 1];

                int count = 0;
                if (char1 == letter) count++;
                if (char2 == letter) count++;

                return count == 1;
            }

            ParseAndValidate(input, IsValid);
        }

        static void ParseAndValidate(string[] input, Func<int, int, char, string, bool> validation)
        {
            int ans = 0;
            foreach (var line in input)
            {
                var lineSplit = line.Split(": ");

                var password = lineSplit[1];
                var rangeAndLetter = lineSplit[0].Split(" ");
                var letter = rangeAndLetter[1][0];

                var minMax = rangeAndLetter[0].Split("-").Select(x => int.Parse(x));
                var min = minMax.First();
                var max = minMax.Last();

                if (validation(min, max, letter, password)) ans++;
            }

            Console.WriteLine(ans);
        }
    }
}
