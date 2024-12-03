using AdventOfCode2024Input;
using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public static class Day3
{
    public static void Run()
    {
        Console.WriteLine("Day 3 Part One");
        string corruptedText = PuzzleData.GetDay3Input();
        string pattern = @"mul\((\d+),(\d+)\)";
        
        Regex regex = new Regex(pattern);
        
        Match match = regex.Match(corruptedText);
        int resultSum = 0;
        while (match.Success)
        {
                int firstNumber = int.Parse(match.Groups[1].Value);
                int secondNumber = int.Parse(match.Groups[2].Value);
                resultSum += firstNumber * secondNumber;
                
                match = match.NextMatch();
        }
        Console.WriteLine($"result: {resultSum}");
        
        Console.WriteLine("");
        Console.WriteLine("Day 3 Part Two");
        pattern = @"don't\(\)|do\(\)|mul\((\d+),(\d+)\)";
        
        regex = new Regex(pattern);
        
        match = regex.Match(corruptedText);
        resultSum = 0;
        bool mulEnabled = true;
        while (match.Success)
        {
            if (match.Value == "don't()")
                mulEnabled = false;
            else if (match.Value == "do()")
                mulEnabled = true;
            else if (mulEnabled)
            {
                int firstNumber = int.Parse(match.Groups[1].Value);
                int secondNumber = int.Parse(match.Groups[2].Value);
                resultSum += firstNumber * secondNumber;
            }
                
            match = match.NextMatch();
        }
        Console.WriteLine($"result: {resultSum}");
    }
}