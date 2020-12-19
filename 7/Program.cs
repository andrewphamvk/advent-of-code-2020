using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace _7
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt");
            var inputLines = File.ReadAllLines(inputFile);
            int ans = 0;

            var nodes = new Dictionary<string, BagNode>();

            foreach (var line in inputLines)
            {
                var split = line.Split(new[] { "contain", "," }, StringSplitOptions.RemoveEmptyEntries);
                InsertBags(split.Select(x => GetBag(x)).ToArray(), nodes);
            }

            // var bagsVisited = new HashSet<string>();
            // Dfs(nodes["shiny gold"], bagsVisited);
            // Console.WriteLine(bagsVisited.Count - 1);

            var res2 = Dfs2(nodes["shiny gold"], 1); // This returns the answer + 1.
            Console.WriteLine(res2);
        }

        static void Dfs(BagNode node, HashSet<string> bagsVisited)
        {
            bagsVisited.Add(node.Name);
            foreach (var child in node.Upstream)
            {
                bagsVisited.Add(child.bag.Name);
                Dfs(child.bag, bagsVisited);
            }
        }

        static int Dfs2(BagNode node, int acc)
        {
            Console.WriteLine(node.Name);
            int sum = acc;
            foreach (var child in node.Downstream)
            {
                sum += Dfs2(child.bag, child.num * acc);
            }

            return sum;
        }

        static (int num, string bag) GetBag(string input)
        {
            Regex rx = new Regex(@"(?<num>\d*) *(?<bag>([a-z])\w+ [a-z]\w+) bags*");
            var match = rx.Match(input);
            int.TryParse(match.Groups["num"].Value, out var num);
            var bag = match.Groups["bag"].Value;


            return (num, bag);
        }

        // A depends on B and C
        // Add entries in B and C that point to A
        // So that we can search using A as the key and find B and C
        static void InsertBags((int num, string bag)[] bagInfos, Dictionary<string, BagNode> nodes)
        {
            foreach (var bagInfo in bagInfos)
            {
                if (bagInfo.bag.Contains("no other")) return;
                if (!nodes.ContainsKey(bagInfo.bag)) nodes.Add(bagInfo.bag, new BagNode { Name = bagInfo.bag });
            }

            var mainBag = nodes[bagInfos[0].bag];
            for (int i = 1; i < bagInfos.Length; i++)
            {
                var otherBag = nodes[bagInfos[i].bag];
                mainBag.Downstream.Add((bagInfos[i].num, otherBag));
                otherBag.Upstream.Add((bagInfos[i].num, mainBag));
            }
        }
    }

    class BagNode
    {
        public string Name { get; set; } = "No name";

        // The other bags that depend on this. I.e. if A depends on B. B.Upstream contains A.
        public List<(int num, BagNode bag)> Upstream { get; set; } = new List<(int, BagNode)>();

        // The other bags that this depends on. I.e if A depends on B. A.Downstream contains B.
        public List<(int num, BagNode bag)> Downstream { get; set; } = new List<(int, BagNode)>();
    }
}
