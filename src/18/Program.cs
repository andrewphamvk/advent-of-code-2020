using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FParsec.CSharp;
using Microsoft.FSharp.Core;
using static FParsec.CSharp.PrimitivesCS; // combinator functions
using static FParsec.CSharp.CharParsersCS; // pre-defined parsers
using Soukoku.ExpressionParser;
using System.Collections.Generic;

namespace _18
{
    class Program
    {
        private static long p1 = default;
        private static long p2 = default;

        static void Main(string[] args)
        {
            var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt"));

            PartOne(input);
            // PartTwo(input);

            Console.WriteLine($"{p1} {p2}");
        }

        static void PartOne(string[] input)
        {
            foreach (var line in input)
            {
                System.Console.WriteLine(Calculate(line));
            }
        }

        static int Calculate(string s)
        {
            s = s.Replace(" ", string.Empty);
            int res = 0;
            var stack = new Stack<int>();

            int curr = 0;
            char lastOp = '+';
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsDigit(s[i]))
                {
                    curr *= 10;
                    curr += s[i] - '0';
                }

                if (!char.IsDigit(s[i]) || i == s.Length - 1)
                {
                    if (lastOp == '+')
                    {
                        res += curr;
                    }
                    else if (lastOp == '-')
                    {
                        res -= curr;
                    }
                    else if (lastOp == '*')
                    {
                        res *= curr;
                    }
                    else if (lastOp == '/')
                    {
                        res /= curr;
                    }

                    if (s[i] == '(')
                    {
                        stack.Push(curr);
                        System.Console.WriteLine(curr);
                        System.Console.WriteLine(s[i]);
                        res = 0;
                    }
                    else if (s[i] == ')')
                    {
                        res += stack.Pop();
                    }
                    else
                    {
                        lastOp = s[i];
                        curr = 0;
                    }
                }
            }


            return res;
        }

        static int EvaluateExpr(Stack<object> stack)
        {
            int res = 0;
            if (stack.Any())
            {
                res = (int)stack.Pop();
            }

            while (stack.Any() && (char)stack.Peek() != ')')
            {
                char sign = (char)stack.Pop();
                if (sign == '+') res += (int)stack.Pop();
                if (sign == '-') res -= (int)stack.Pop();
            }

            return res;
        }

        static void PartTwo(string[] input)
        {
            var basicExprParser = new OPPBuilder<Unit, long, Unit>()
                .WithOperators(ops => ops
                    .AddInfix("+", 2, (x, y) => x + y)
                    .AddInfix("*", 1, (x, y) => x * y))
                    .WithTerms(term => Choice(Long, Between('(', term, ')')))
                .Build()
                .ExpressionParser;

            foreach (var line in input)
            {
                var expr = line.Replace(" ", string.Empty);
                var res = basicExprParser.Run(expr).GetResult();
                p2 += res;
            }
        }
    }
}
