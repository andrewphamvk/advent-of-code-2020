using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _19
{
    class Program
    {
        private static int p1 = default;
        private static int p2 = default;

        static void Main(string[] args)
        {
            var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt"));

            PartOne(input);
            PartTwo(input);

            Console.WriteLine($"{p1} {p2}");
        }

        static void PartOne(string[] input)
        {
            int lineNum = 0;
            var ruleMap = new Dictionary<int, Rule>();
            while (!string.IsNullOrWhiteSpace(input[lineNum]))
            {
                var split1 = input[lineNum++].Split(": ");
                var split2 = split1[1].Split(" | ");

                int ruleNum = int.Parse(split1[0]);
                Rule rule = GetOrAdd(ruleMap, ruleNum);

                for (int i = 0; i < split2.Length; i++)
                {
                    var ruleSplit = split2[i].Split(" ");
                    if (ruleSplit[0] == "\"a\"" )
                    {
                        rule.Word = "a";
                    }
                    else if (ruleSplit[0] == "\"b\"")
                    {
                        rule.Word = "b";
                    }
                    else
                    {
                        var dependsOn = new List<Rule>();
                        foreach (var r in ruleSplit)
                        {
                            dependsOn.Add(GetOrAdd(ruleMap, int.Parse(r)));
                        }
                        rule.DependsOn.Add(dependsOn);
                    }
                }
            }

            lineNum++;
            var rule0 = ruleMap[0];
            var valid = new HashSet<string>(rule0.GetMessages());

            Console.Write($"42: ");
            foreach (var r in ruleMap[42].GetMessages())
            {
                Console.Write($"{r} ");
            }
            Console.WriteLine();

            Console.Write($"31: ");
            foreach (var r in ruleMap[31].GetMessages())
            {
                Console.Write($"{r} ");
            }
            Console.WriteLine();

            while (lineNum < input.Length)
            {
                var toValidate = input[lineNum++];
                if (valid.Contains(toValidate)) p1++;
            }
        }

        static Rule GetOrAdd(Dictionary<int, Rule> ruleMap, int ruleNum)
        {
            Rule rule = null;
            if (ruleMap.ContainsKey(ruleNum))
            {
                rule = ruleMap[ruleNum];
            }
            else
            {
                rule = new Rule { Number = ruleNum };
                ruleMap.Add(ruleNum, rule);
            }

            return rule;
        }

        static void PartTwo(string[] input)
        {

        }
    }

    class Rule
    {
        public int Number;

        public List<List<Rule>> DependsOn = new List<List<Rule>>();

        public string Word = null;

        private List<string> messages = null;

        public List<string> GetMessages()
        {
            if (this.messages != null) return this.messages;

            if (this.Word != null)
            {
                this.messages = new List<string> { this.Word };
                return this.messages;
            }

            this.messages = new List<string>();
            foreach (var group in DependsOn)
            {
                if (group.Count == 2)
                {
                    this.messages.AddRange(Merge(group[0].GetMessages(), group[1].GetMessages()));
                }
                else
                {
                    this.messages.AddRange(group[0].GetMessages());
                }
            }

            return this.messages;
        }

        public static List<string> Merge(List<string> a, List<string> b)
        {
            var res = new List<string>();

            foreach (var na in a)
            {
                foreach (var nb in b)
                {
                    res.Add(na + nb);
                }
            }

            return res;
        }
    }
}
