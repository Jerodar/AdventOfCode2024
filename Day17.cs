using System.Runtime.InteropServices;
using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day17
{
    public static void Run()
    {
        Console.WriteLine("Day 15 Part One");

        (int a, int b, int c) registers = PuzzleData.GetDay17Registers();
        List<int> program = PuzzleData.GetDay17Program();

        var computer = new Computer(registers.a, registers.b, registers.c);
        Console.WriteLine("Running program...");
        var output = computer.Run(program);
        Console.WriteLine(output);
        
        Console.WriteLine("Day 15 Part Two");
        
        var expected = "";
        foreach (var i in program)
            expected += i + ",";
        expected = expected[..^1];
        
        // rerun for each 3 bits,filling in the previous answer everytime
        long prevA = 4652658759614; 
        for (long a = 0; a < 8; a++)
        {
            var newA = (prevA << 3) + a;
            computer.SetA(newA);
            output = computer.Run(program);
            Console.WriteLine($"{a} {newA} {expected} - {output}");
            if (string.Compare(output, expected) == 0)
            {
                Console.WriteLine($"result: {newA}");
                break;
            }
        }
    }
}

public class Computer
{
    private long _a;
    private long _b;
    private long _c;

    public Computer(long a, long b, long c)
    {
        _a = a;
        _b = b;
        _c = c;
    }

    public void SetA(long a)
    {
        _a = a;
    }

    public string Run(List<int> program)
    {
        int pointer = 0;
        string output = "";
        
        while (pointer < program.Count)
        {
            var opcode = program[pointer];
            var operand = program[pointer + 1];
            switch (opcode)
            {
                case 0:
                    // adv: A=A/2^{com}
                    _a = (long)(_a / Math.Pow(2, Combo(operand)));
                    pointer += 2;
                    break;
                case 1:
                    // bxl: B = B XOR {lit}
                    _b = _b ^ operand;
                    pointer += 2;
                    break;
                case 2:
                    // bst: B = {com} % 8
                    _b = Combo(operand) % 8;
                    pointer += 2;
                    break;
                case 3:
                    // jnz: iptr = {lit} if A!=0
                    if (_a != 0) pointer = operand;
                    else pointer += 2;
                    break;
                case 4:
                    // bxc: B = B XOR C (ignore operand)
                    _b = _b ^ _c;
                    pointer += 2;
                    break;
                case 5:
                    // out: stdout = {com} % 8 (multiple outputs seperated by comma)
                    output += (Combo(operand) % 8).ToString() + ',';
                    pointer += 2;
                    break;
                case 6:
                    // bdv: B=A/2^{com}
                    _b = (long)(_a / Math.Pow(2, Combo(operand)));
                    pointer += 2;
                    break;
                case 7:
                    // cdv: C=A/2^{com}
                    _c = (long)(_a / Math.Pow(2, Combo(operand)));
                    pointer += 2;
                    break;
            }
        }
        return output[..^1];
    }
    
    private long Combo(int input)
    {
        return input switch
        {
            4 => _a,
            5 => _b,
            6 => _c,
            7 => throw new Exception("Combo operand 7 is not supported!"),
            _ => input
        };
    }
}