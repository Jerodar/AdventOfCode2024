using AdventOfCode2024;
using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day5
{
    public static void Run()
    {
        Console.WriteLine($"Day 5 Part One");
        List<int>[] manuals = PuzzleData.GetDay5Manuals();
        List<(int,int)> rules = PuzzleData.GetDay5Rules();

        int middleSum = 0;
        foreach (var manual in manuals)
        {
            bool isInvalid = false;
            for (int i = manual.Count - 2; i >= 0; i--)
            {
                int numberToCheck = manual[i];
                List<int> numbersAfter = manual.GetRange(i + 1, manual.Count - i - 1);
                var brokenRules = (from rule in rules where rule.Item2 == numberToCheck && numbersAfter.Contains(rule.Item1) select rule).ToList();
                if (brokenRules.Any())
                {
                    isInvalid = true;
                    Console.WriteLine($"Manual has invalid number: number:{manual[i]} afer:{string.Join(", ", numbersAfter)} full:{string.Join(", ", manual)}");
                    Console.WriteLine($"Broken rules: {string.Join(", ", brokenRules)}");
                }
                if(isInvalid) { break; }
            }
            if (isInvalid) { continue; }
            
            int middle = (manual.Count - 1) / 2;
            middleSum += manual[middle];
        }

        Console.WriteLine(middleSum);
    }
}