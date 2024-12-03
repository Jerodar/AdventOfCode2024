using AdventOfCode2024Input;
using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public static class Day3
{
    public static void Run()
    {
        string corruptedText = PuzzleData.GetDay3Input();
        string pattern = @"mul\((\d+),(\d+)\)";
        
        Regex regex = new Regex(pattern);
        
        Match match = regex.Match(corruptedText);
        int resultSum = 0;
        while (match.Success)
        {
                int firstNumber = int.Parse(match.Groups[1].Value);
                int secondNumber = int.Parse(match.Groups[2].Value);
                Console.WriteLine($"{firstNumber} * {secondNumber}");
                resultSum += firstNumber * secondNumber;
                
                match = match.NextMatch();
        }
        Console.WriteLine($"result: {resultSum}");
    }
}