using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PassportProcessing
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt");
            var inputLines = File.ReadAllLines(inputFile);
            
            int validPassports = 0;
            var passport = new Dictionary<string, string>();
            foreach (var line in inputLines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var keyValuePairs = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var keyValuePair in keyValuePairs)
                    {
                        var tokens = keyValuePair.Split(":", 2);
                        // Console.WriteLine($"{tokens[0]} {tokens[1]}");
                        passport.Add(tokens[0], tokens[1]);
                    }
                }
                else
                {
                    if (ValidatePassportStrict(passport)) validPassports++;;
                    passport.Clear();
                }
            }

            if (ValidatePassportStrict(passport)) validPassports++;;

            Console.WriteLine(validPassports);
        }

        static bool ValidatePassport(Dictionary<string, string> passport)
        {
            var requiredFields = new string[] { "byr", "iyr", "eyr", "hgt", "hcl",  "ecl", "pid" }; // ignore cid
            foreach (var field in requiredFields)
            {
                if (!passport.ContainsKey(field)) return false;
            }

            return true;
        }

        static bool ValidatePassportStrict(Dictionary<string, string> passport)
        {
            var requiredFields = new string[] { "byr", "iyr", "eyr", "hgt", "hcl",  "ecl", "pid" }; // ignore cid
            foreach (var field in requiredFields)
            {
                if (!passport.ContainsKey(field)) return false;
            }

            var byr = int.Parse(passport["byr"]);
            if (byr < 1920 || byr > 2002) return false;

            var iyr = int.Parse(passport["iyr"]);
            if (iyr < 2010 || iyr > 2020) return false;

            var eyr = int.Parse(passport["eyr"]);
            if (eyr < 2020 || eyr > 2030) return false;

            var hgt = passport["hgt"];
            var hgtInt = int.Parse(hgt.Substring(0, hgt.Length - 2));
            if (!hgt.EndsWith("cm") && !hgt.EndsWith("in")) return false;
            if (hgt.EndsWith("cm") && hgtInt < 150 || hgtInt > 193) return false;
            if (hgt.EndsWith("in") && hgtInt < 59 || hgtInt > 76) return false;

            var hcl = passport["hcl"];
            if (hcl.Length != 7 || hcl[0] != '#') return false;
            foreach (var c in hcl)
            {
                if ((c < 'a' || c > 'f') && (c  < '0' || c > '9')) return false;
            }

            var ecl = passport["ecl"];
            if (ecl == "amb" && ecl == "blu" && ecl == "brn" && ecl == "gry" && ecl == "grn" && ecl == "hzl" && ecl == "oth") return false;

            var pid = passport["pid"];
            if (pid.Length != 9) return false;
            foreach (var c in hcl)
            {
                if ((!char.IsDigit(c))) return false;
            }

            return true;
        }
    }
}
