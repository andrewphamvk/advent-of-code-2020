using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _21
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
            var ingredientSet = new Dictionary<string, HashSet<string>>(); // The list of possible allergens the ingredient has
            var allergenSet = new Dictionary<string, List<HashSet<string>>>(); // The allergen and then list of ingredients
            var ingredientCount = new Dictionary<string, int>();

            foreach (var line in input)
            {
                string[] ingredients = Array.Empty<string>();
                string[] allergens = Array.Empty<string>();

                int startOfAllergens = line.IndexOf("(");
                if (startOfAllergens == -1)
                {
                    ingredients = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    ingredients = line.Substring(0, startOfAllergens).Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    startOfAllergens = startOfAllergens + "(contains ".Length;
                    allergens = line.Substring(startOfAllergens, line.Length - startOfAllergens - 1).Split(", ");
                }

                // Initialize if not exists
                foreach (var ingredient in ingredients)
                {
                    if (!ingredientCount.ContainsKey(ingredient)) ingredientCount.Add(ingredient, 0);
                    ingredientCount[ingredient]++;

                    if (!ingredientSet.ContainsKey(ingredient)) ingredientSet.Add(ingredient, new HashSet<string>());
                    foreach (var allergen in allergens)
                    {
                        if (!allergenSet.ContainsKey(allergen)) allergenSet.Add(allergen, new List<HashSet<string>>());
                        ingredientSet[ingredient].Add(allergen);
                    }
                }


                // Add the possible ingredients that might contain allergen
                foreach (var allergen in allergens)
                {
                    allergenSet[allergen].Add(new HashSet<string>(ingredients));
                }

            }

            var notAllergenic = new List<string>();
            foreach (var ingredientKV in ingredientSet)
            {
                var ingredient = ingredientKV.Key;
                bool isAllergenic = false;

                foreach (var possibleAllergen in ingredientKV.Value)
                {
                    // Is it possible for ingredient to contain this allergen?
                    if (allergenSet[possibleAllergen].All(x => x.Contains(ingredient)))
                    {
                        Console.WriteLine($"ingredient: {ingredient}, allergen: {possibleAllergen}");
                        isAllergenic = true;
                    }
                }

                if (!isAllergenic)
                {
                    notAllergenic.Add(ingredient);
                    p1 += ingredientCount[ingredient];
                }
                else
                {

                }
            }



        }

        static void PartTwo(string[] input)
        {

        }
    }
}
