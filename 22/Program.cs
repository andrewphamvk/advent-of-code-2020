using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace _22
{
    class Program
    {
        private static int p1 = default;
        private static int p2 = default;

        static void Main(string[] args)
        {
            var input = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt"));

            //PartOne(input);
            PartTwo(input);

            Console.WriteLine($"{p1} {p2}");
        }

        static void PartOne(string[] input)
        {
            int i = 1;
            var queue1 = new Queue<int>();
            var queue2 = new Queue<int>();

            while (!string.IsNullOrWhiteSpace(input[i]))
            {
                queue1.Enqueue(int.Parse(input[i++]));
            }
            i += 2;
            while (i < input.Length)
            {
                queue2.Enqueue(int.Parse(input[i++]));
            }

            while (queue1.Count != 0 && queue2.Count != 0)
            {
                int a = queue1.Dequeue();
                int b = queue2.Dequeue();

                if (a > b)
                {
                    queue1.Enqueue(a);
                    queue1.Enqueue(b);
                }
                else if (a < b)
                {
                    queue2.Enqueue(b);
                    queue2.Enqueue(a);
                }
                else
                {
                    Console.WriteLine("unexpected");
                }
            }

            Queue<int> winningDeck = queue1.Count > 0 ? queue1 : queue2;

            int multiplier = winningDeck.Count;
            while (winningDeck.Any())
            {
                p1 += multiplier * winningDeck.Dequeue();
                multiplier--;
            }
        }

        static void PartTwo(string[] input)
        {
            int i = 1;
            var queue1 = new Queue<int>();
            var queue2 = new Queue<int>();

            while (!string.IsNullOrWhiteSpace(input[i]))
            {
                queue1.Enqueue(int.Parse(input[i++]));
            }
            i += 2;
            while (i < input.Length)
            {
                queue2.Enqueue(int.Parse(input[i++]));
            }

            int winner = RecursiveCombat(queue1, queue2);
            Queue<int> winningDeck = winner == 1 ? queue1 : queue2;
            int multiplier = winningDeck.Count;
            while (winningDeck.Any())
            {
                p2 += multiplier * winningDeck.Dequeue();
                multiplier--;
            }
        } // 32443

        static Queue<int> Copy(Queue<int> deck, int cardsToCopy)
        {
            return new Queue<int>(deck.Take(cardsToCopy));
        }

        static int RecursiveCombat(Queue<int> player1, Queue<int> player2)
        {
            int winner = 0;
            //var previousWins = new Dictionary<(int, long), int>();
            var previousWins = new HashSet<(int, string)>();
            //var previousWins = new Dictionary<(long, long), int>();
            while (player1.Count != 0 && player2.Count != 0)
            {
                //var hash = CreateHash(player1, player2);
                var hash1 = CreateHash2(player1);
                var hash2 = CreateHash2(player2);

                if (previousWins.Contains((1, hash1)))
                {
                    return 1;
                }
                else if (previousWins.Contains((2, hash2)))
                {
                    return 1;
                }
                else
                {
                    int a = player1.Dequeue();
                    int b = player2.Dequeue();

                    if (player1.Count >= a && player2.Count >= b)
                    {
                        winner = RecursiveCombat(Copy(player1, a), Copy(player2, b));
                    }
                    else if (a < b)
                    {
                        winner = 2;
                    }
                    else
                    {
                        winner = 1;
                    }

                    if (winner == 1)
                    {
                        player1.Enqueue(a);
                        player1.Enqueue(b);
                    }
                    else
                    {
                        player2.Enqueue(b);
                        player2.Enqueue(a);
                    }
                }

                previousWins.Add((1, hash1));
                previousWins.Add((2, hash2));
            }

            winner = player1.Any() ? 1 : 2;

            return winner;
        }

        static long CreateHash(Queue<int> player1)
        {
            int hc1 = player1.Count;
            foreach (var card in player1)
            {
                hc1 = unchecked(hc1 * 17 + card);
                //hc = unchecked(hc * 314159 + card);
            }

            return hc1;
        }

        static string CreateHash2(Queue<int> player1)
        {
            var sb = new StringBuilder();
            foreach (var card in player1)
            {
                sb.Append($"{card},");
            }

            return sb.ToString();
        }

        //static (long, long) CreateHash(Queue<int> player1, Queue<int> player2)
        //{
        //    int hc1 = player1.Count;
        //    foreach (var card in player1)
        //    {
        //        hc1 = unchecked(hc1 * 17 + card);
        //        //hc = unchecked(hc * 314159 + card);
        //    }

        //    int hc2 = player2.Count;
        //    foreach (var card in player2)
        //    {
        //        hc2 = unchecked(hc2 * 17 + card);
        //        //hc = unchecked(hc * 314159 + card);
        //    }

        //    return (hc1, hc2);
        //}

        //static string CreateHash(Queue<int> player1, Queue<int> player2)
        //{
        //    var sb = new StringBuilder();
        //    foreach (var card in player1)
        //    {
        //        sb.Append($"{card},");
        //    }

        //    sb.Append("|");

        //    foreach (var card in player2)
        //    {
        //        sb.Append($"{card},");
        //        //hc = unchecked(hc * 314159 + card);
        //    }

        //    return sb.ToString();
        //}
    }
}
