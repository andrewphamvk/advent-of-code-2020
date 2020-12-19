using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CustomCustomers
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt");
            var inputLines = File.ReadAllLines(inputFile);

            int ans = 0;
            var totalParticipants = 0;
            var questions = new int[26];
            foreach (var line in inputLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    foreach (var question in questions)
                    {
                        if (question == totalParticipants) ans++;
                    }
                    ClearQuestions(questions);
                    totalParticipants = 0;
                }
                else
                {
                    totalParticipants++;
                    foreach (var ch in line) questions[ch - 'a']++;
                }
            }

            foreach (var question in questions)
            {
                if (question == totalParticipants) ans++;
            }

            Console.WriteLine(ans);

        }

        static void ClearQuestions(int[] questions)
        {
            for (int i = 0; i < questions.Length; i++)
            {
                questions[i] = 0;
            }
        }

        static void PartOne()
        {
            var inputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt");
            var inputLines = File.ReadAllLines(inputFile);

            var ans = 0;
            var questionsAnswered = new HashSet<char>();
            foreach (var line in inputLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    ans += questionsAnswered.Count;
                    questionsAnswered.Clear();
                }
                else
                {
                    questionsAnswered.UnionWith(line);
                }
            }

            ans += questionsAnswered.Count;
            Console.WriteLine(ans);
        }
    }
}
