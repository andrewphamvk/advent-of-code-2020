using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace _16
{
    class Program
    {
        private static int p1;
        private static long p2 = 1;

        static void Main(string[] args)
        {
            var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt"));

            PartOne(input);

            Console.WriteLine($"{p1} {p2}");
        }

        static void PartOne(string[] input)
        {
            int lineIdx = 0;
            var fields = new List<Field>();
            while (!string.IsNullOrWhiteSpace(input[lineIdx]))
            {
                var split1 = input[lineIdx++].Split(": ");
                var field = new Field();
                field.Name = split1[0];
                var split2 = split1[1].Split(" or ");
                var range1 = split2[0].Split("-").Select(x => int.Parse(x)).ToArray();
                var range2 = split2[1].Split("-").Select(x => int.Parse(x)).ToArray();
                field.Validations.Add((val) => Check(range1[0], range1[1], val));
                field.Validations.Add((val) => Check(range2[0], range2[1], val));

                fields.Add(field);
            }

            lineIdx++;
            lineIdx++;
            var myTicket = input[lineIdx++].Split(",").Select(x => int.Parse(x)).ToArray();
            // Console.WriteLine($"myticket {myTicket[0]}");

            lineIdx++;
            lineIdx++;
            var tickets = new List<int[]>();

            System.Console.WriteLine(input.Length);
            while (lineIdx < input.Length)
            {
                var ticket = input[lineIdx].Split(",").Select(x => int.Parse(x)).ToArray();

                bool isTicketValid = true;
                foreach (var ticketVal in ticket)
                {
                    bool isValid = false;
                    foreach (var field in fields)
                    {
                        if (field.IsValid(ticketVal)) isValid = true;
                    }

                    if (!isValid)
                    {
                        p1 += ticketVal;
                        isTicketValid = false;
                    }
                }

                if (isTicketValid)
                {
                    tickets.Add(ticket);
                }

                lineIdx++;
            }

            // System.Console.WriteLine(tickets.Count);
            // System.Console.WriteLine("DONE LENGTH");

            var possible = new bool[fields.Count][];
            for (int i = 0; i < fields.Count; i++)
            {
                possible[i] = new bool[20];
            }

            for (int f = 0; f < fields.Count; f++)
            {

                // Check against one column
                for (int i = 0; i < fields.Count; i++)
                {
                    bool valid = true;
                    foreach (var ticket in tickets)
                    {
                        if (!fields[f].IsValid(ticket[i])) valid = false;
                    }

                    possible[f][i] = valid;
                }
            }

            for (int f = 0; f < fields.Count; f++)
            {
                System.Console.WriteLine($"{fields[f].Name} - {f}");
                var sb = new StringBuilder();
                for (int i = 0; i < possible[f].Length; i++)
                {
                    if (possible[f][i]) sb.Append($"{i} ");
                }

                System.Console.WriteLine(sb);
            }

            System.Console.WriteLine("DONE");

            var used = new bool[20];
            used[9] = true;
            used[10] = true;
            used[12] = true;
            used[17] = true;
            used[14] = true;

            Field[] curr = new Field[20];
            curr[3] = fields[9];
            curr[7] = fields[10];
            curr[13] = fields[12];
            curr[4] = fields[17];
            curr[2] = fields[14];

            var res = Backtrack(fields.ToArray(), tickets, curr, 0, used);
            System.Console.WriteLine(res);
            if (!res) return;

            // var res = Permutate(fields.ToArray(), tickets.ToList(), 0);
            // System.Console.WriteLine($"perm res: {res}");

            var cols = new List<int>();
            System.Console.WriteLine($"Correct order count: {correctOrder.Count}");
            for (int i = 0; i < correctOrder.Count; i++)
            {
                System.Console.WriteLine(correctOrder[i].Name);
                if (correctOrder[i].Name.Contains("departure"))
                {
                    // System.Console.WriteLine(correctOrder[i].Name);
                    cols.Add(i);
                }
            }

            System.Console.WriteLine($"count: {cols.Count}");
            foreach (var col in cols)
            {
                p2 *= myTicket[col];
            }
        }

        static List<Field> correctOrder = new List<Field>();

        // Returns true if found
        static bool Backtrack(Field[] fields, List<int[]> tickets, Field[] curr, int idx, bool[] used)
        {
            if (idx == fields.Length)
            {
                correctOrder = new List<Field>(curr);
                return true;
            }

            if (curr[idx] != null)
            {
                return Backtrack(fields, tickets, curr, idx + 1, used);
            }
            else
            {
                for (int i = 0; i < used.Length; i++)
                {
                    if (used[i]) continue;
                    used[i] = true;
                    curr[idx] = fields[i];
                    if (ValidForAllColumns(curr[idx], tickets, idx))
                    {
                        if (Backtrack(fields, tickets, curr, idx + 1, used)) return true;
                    }
                    used[i] = false;
                    curr[idx] = null;
                }
            }

            return false;
        }

        static bool ValidForAllColumns(Field field, List<int[]> tickets, int col)
        {
            foreach (var ticket in tickets) if (!field.IsValid(ticket[col])) return false;
            return true;
        }


        // static bool Permutate(Field[] fields, List<int[]> tickets, int start)
        // {
        //     if (start == fields.Length)
        //     {
        //         if (CheckFieldOrder(fields, tickets))
        //         {
        //             return true;
        //         }

        //         // System.Console.WriteLine("FALSE");
        //         return false;
        //     }

        //     for (int i = start; i < fields.Length; i++)
        //     {
        //         // var sb = new StringBuilder();
        //         // sb.Append($"{fields[start].Name} ");
        //         // sb.Append($"{fields[i].Name} |");

        //         var temp = fields[start];

        //         fields[start] = fields[i];
        //         fields[i] = temp;

        //         // bool validForCol = true;
        //         // for (int j = 0; j < tickets.Count; j++)
        //         // {
        //         //     if (!fields[start].IsValid(tickets[j][start]))
        //         //     {
        //         //         validForCol = false;
        //         //         break;
        //         //     }

        //         //     if (!fields[i].IsValid(tickets[j][i]))
        //         //     {
        //         //         validForCol = false;
        //         //         break;
        //         //     }
        //         // }

        //         // sb.Append($"{fields[start].Name} ");
        //         // sb.Append($"{fields[i].Name} ");

        //         // System.Console.WriteLine(sb.ToString());


        //         if (Permutate(fields, tickets, start + 1))
        //         {
        //             correctOrder = new List<Field>();
        //             foreach (var f in fields) correctOrder.Add(f);
        //             return true;
        //         }

        //         temp = fields[start];
        //         fields[start] = fields[i];
        //         fields[i] = temp;
        //     }

        //     return false;
        // }

        static bool Check(int x, int y, int val)
        {
            return (val >= x && val <= y);
        }

        static bool CheckFieldOrder(Field[] fields, List<int[]> tickets)
        {
            foreach (var ticket in tickets)
            {
                for (int i = 0; i < ticket.Length; i++)
                {
                    if (!fields[i].IsValid(ticket[i])) return false;
                }
            }

            return true;
        }
    }

    class Field
    {
        public string Name;

        public List<Func<int, bool>> Validations = new List<Func<int, bool>>();

        public bool IsValid(int val)
        {
            return this.Validations.Any(x => x.Invoke(val));
        }
    }
}
