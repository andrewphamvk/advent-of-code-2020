using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _23
{
    class Program
    {
        private static long p1 = default;
        private static long p2 = default;

        static void Main(string[] args)
        {
            var input = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt"));

            PartOne(input);
            PartTwo(input);

            Console.WriteLine($"{p1} {p2}");
        }

        static void PartOne(string input)
        {
            var list = new List<int>();
            foreach (var x in input)
            {
                var cup = x - '0';
                list.Add(cup);
            }

            int idx = 0;
            for (int t = 0; t < 100; t++)
            {
                (list, idx) = Reconstruct(list, idx);
                idx++;
                idx %= 9;
            }

            int idxOne = 0;
            for (int i = 0; i < 10; i++)
            {
                if (list[i] == 1)
                {
                    idxOne = i;
                    break;
                }
            }

            for (int i = 0; i < 8; i++)
            {
                idxOne++;
                idxOne %= 9;
                p1 *= 10;
                p1 += list[idxOne];
            }
        }

        static (List<int> newList, int newIdx) Reconstruct(List<int> list, int currIdx)
        {
            var newList = new int[list.Count];
            var nextThree = GetNextThree(list, currIdx);
            var newIdx = -1;

            int cup = list[currIdx];
            int dest = GetDest(cup, nextThree);

            int i = 0;
            int d = 0;
            while (i < list.Count) {
                if (nextThree.Contains(list[i]))
                {
                    i++;
                }
                else if (list[i] == dest)
                {
                    newList[d++] = list[i++];
                    foreach (var x in nextThree)
                    {
                        newList[d++] = x;
                    }
                }
                else
                {
                    if (i == currIdx) newIdx = d;
                    newList[d++] = list[i++];
                }
            }

            return (newList.ToList(), newIdx);
        }

        private static int GetDest(int cup, List<int> nextThree)
        {
            // Make it zero based
            cup--;

            for (int i = 0; i < 10; i++)
            {
                cup += 9;
                cup--;
                cup %= 9;

                if (!nextThree.Contains(cup + 1)) return (cup + 1);
            }

            return -1;
        }

        private static List<int> GetNextThree(List<int> list, int currIdx)
        {
            var nextThree = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                currIdx += 1;
                currIdx %= 9;
                nextThree.Add(list[currIdx]);
            }

            return nextThree;
        }

        static void PartTwo(string input)
        {
            var map = new Dictionary<int, Cup>();

            Cup head = null;
            Cup curr = null;
            foreach (var cupVal in input)
            {
                var cup = new Cup(cupVal - '0');
                map.Add(cupVal - '0', cup);

                if (head == null && curr == null)
                {
                    head = cup;
                    curr = cup;
                }
                else
                {
                    curr.Next = cup;
                    cup.Prev = curr;
                    curr = cup;
                }
            }

            int maxCups = 1000000;
            for (int i = 10; i <= maxCups; i++)
            {
                var cup = new Cup(i);
                curr.Next = cup;
                cup.Prev = curr;
                curr = cup;
                
                map.Add(i, cup);
            }

            curr.Next = head;
            head.Prev = curr;


            curr = head;

            // Play game
            var maxTurns = 10000000;
            for (int t = 0; t < maxTurns; t++)
            {
                int currCupValue = curr.Value;
                var (destValue, nextThree) = FindDestValue(currCupValue, curr, maxCups);

                var destCup = map[destValue];
                nextThree[2].Next = destCup.Next;
                nextThree[0].Prev = destCup;
                destCup.Next.Prev = nextThree[2];
                destCup.Next = nextThree[0];

                curr = curr.Next;
            }

            var p12 = 0;
            var p1Node = map[1];
            for (int i = 0; i < 8; i++)
            {
                p1Node = p1Node.Next;
                Console.WriteLine(p1Node.Value);
                p12 *= 10;
                p12 += p1Node.Value;
            }
            Console.WriteLine(p12);


            var prevValue = map[1].Next.Value;
            var nextValue = map[1].Next.Next.Value;

            Console.WriteLine(prevValue);
            Console.WriteLine(nextValue);

            p2 = prevValue * nextValue;

        }

        private static (int destValue, List<Cup> nextThree) FindDestValue(int currCupValue, Cup curr, int max)
        {
            var nextThree = new List<Cup> {curr.Next, curr.Next.Next, curr.Next.Next.Next};
            curr.Next = nextThree[2].Next;
            nextThree[2].Next.Prev = curr;

            nextThree[0].Prev = null;
            nextThree[2].Next = null;

            int destValue = curr.Value;
            destValue--; // zero based
            for (int i = 0; i < 4; i++)
            {
                if (destValue == 0)
                {
                    destValue += max;
                    destValue--;
                    destValue %= max;
                }
                else
                {
                    destValue--;
                }

                if (nextThree.All(x => x.Value != destValue + 1))
                {
                    return (destValue + 1, nextThree);
                }
            }

            throw new ApplicationException();
        }
    }

    internal class Cup
    {
        public Cup(int value)
        {
            this.Value = value;
        }

        public Cup Prev = null;
        public Cup Next = null;
        public int Value = 0;
    }
}
