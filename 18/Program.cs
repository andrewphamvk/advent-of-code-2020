using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FParsec.CSharp;
using Microsoft.FSharp.Core;
using static FParsec.CSharp.PrimitivesCS; // combinator functions
using static FParsec.CSharp.CharParsersCS; // pre-defined parsers
using Soukoku.ExpressionParser;

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
            PartTwo(input);

            Console.WriteLine($"{p1} {p2}");
        }

        static void PartOne(string[] input)
        {
            var context = new EvaluationContext();
            var evaluator = new Evaluator(context);
            var result = evaluator.Evaluate("33 + 55 * 2");
            Console.WriteLine(result.ToString()); // should be "88"

            // var basicExprParser = new OPPBuilder<Unit, long, Unit>()
            //     .WithOperators(ops => ops
            //     .AddInfix("+", 1, (x, y) => x + y)
            //     .AddInfix("*", 1, (x, y) => x * y))
            //     .WithTerms(term => Choice(Long, Between('(', term, ')')))
            //     .Build()
            //     .ExpressionParser;

            // foreach (var line in input)
            // {
            //     var expr = line.Replace(" ", string.Empty);
            //     var res = basicExprParser.Run(expr).GetResult();
            //     p1 += res;
            // }
        }

        // 0 = NoOp
        // 1 = Addition
        // 2 = Multiplication
        // After running this func, start will be the end of the expr
        static (long res, int end) Calculate(string expr, int start)
        {
            long val = 0;
            if (Char.IsDigit(expr[start]))
            {
                val = expr[start++] - '0';
            }
            else if (expr[start] == '(')
            {
                var (initRes, end) = Calculate(expr, start + 1);
                val = initRes;
                start = end;
            }

            if (start < expr.Length)
            {
                if (expr[start] == '+')
                {
                    start++;
                    var (intRes, end) = Calculate(expr, start);
                    val += intRes;
                    start = end;
                }
                else if (expr[start] == '*')
                {
                    start++;
                    var (intRes, end) = Calculate(expr, start);
                    val *= intRes;
                    start = end;
                }
            }

            if (start < expr.Length && expr[start] == ')')
            {
                start++;
            }

            // System.Console.WriteLine($"res: {val}");
            return (val, start);
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
