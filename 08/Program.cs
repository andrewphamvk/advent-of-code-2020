using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace _8
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "input.txt");
            var inputLines = File.ReadAllLines(inputFile);
            var instructions = inputLines.Select(x => ParseInstruction(x)).ToArray();

            var res = Simulate(instructions);
            if (res.success)
            {
                Console.WriteLine(res.res);
                return;
            }

            for (int i = 0; i < instructions.Length; i++)
            {
                if (instructions[i].Type == InstructionType.Jmp)
                {
                    instructions[i].Type = InstructionType.Nop;
                    res = Simulate(instructions);
                    if (res.success)
                    {
                        Console.WriteLine(res.res);
                        return;
                    }
                    instructions[i].Type = InstructionType.Jmp;
                }
                else if (instructions[i].Type == InstructionType.Nop)
                {
                    instructions[i].Type = InstructionType.Jmp;
                    res = Simulate(instructions);
                    if (res.success)
                    {
                        Console.WriteLine(res.res);
                        return;
                    }
                    instructions[i].Type = InstructionType.Nop;
                }
            }
        }

        static (bool success, int res) Simulate(Instruction[] instructions)
        {
            int res = 0;
            var success = true;
            var seen = new HashSet<int>();
            for (int i = 0; i < instructions.Length;)
            {
                if (seen.Contains(i))
                {
                    success = false;
                    break;
                }

                seen.Add(i);

                var typ = instructions[i].Type;
                var val = instructions[i].Value;
                if (typ == InstructionType.Acc)
                {
                    res += val;
                    i++;
                }
                else if (typ == InstructionType.Jmp)
                {
                    i += val;
                }
                else
                {
                    i++;
                }
            }

            return (success, res);
        }

        static Instruction ParseInstruction(string input)
        {
            var instruction = new Instruction();
            var split = input.Split(" ");

            if (split[0] == "jmp")
            {
                instruction.Type = InstructionType.Jmp;
            }
            else if (split[0] == "acc")
            {
                instruction.Type = InstructionType.Acc;
            }

            instruction.Value = int.Parse(split[1]);

            return instruction;
        }
    }

    class Instruction
    {
        public InstructionType Type;

        public int Value;
    }

    enum InstructionType
    {
        Nop = 0,

        Acc = 1,

        Jmp = 2
    }
}
