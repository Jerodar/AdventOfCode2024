using AdventOfCode2024;
using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day5
{
    public static void Run()
    {
        Console.WriteLine($"Day 5 Part One");
        List<int>[] manuals = PuzzleData.GetDay5Manuals();
        List<(int, int)> rules = PuzzleData.GetDay5Rules();

        int middleSum = 0;
        List<List<int>> correctedManuals = new();
        foreach (var manual in manuals)
        {
            var brokenRules = CheckForBrokenRules(rules, manual);
            if (!brokenRules.Any())
            {
                int middle = (manual.Count - 1) / 2;
                middleSum += manual[middle];
                continue;
            }

            List<int> correctedManual = new();
            while (brokenRules.Any())
            {
                correctedManual = FixManual(manual, brokenRules);
                brokenRules = CheckForBrokenRules(rules, correctedManual);
            }
            correctedManuals.Add(correctedManual);
        }

        Console.WriteLine(middleSum);

        Console.WriteLine("");
        Console.WriteLine($"Day 5 Part Two");

        middleSum = 0;
        foreach (var manual in correctedManuals)
        {
            int middle = (manual.Count - 1) / 2;
            middleSum += manual[middle];
        }
        Console.WriteLine(middleSum);
    }

    private static List<int> FixManual(List<int> manual, List<(int, int)> brokenRules)
    {
        foreach (var brokenRule in brokenRules)
        {
            var indexA = manual.IndexOf(brokenRule.Item1);
            var indexB = manual.IndexOf(brokenRule.Item2);
            manual[indexA] = brokenRule.Item2;
            manual[indexB] = brokenRule.Item1;
        }

        return manual;
    }

    private static List<(int, int)> CheckForBrokenRules(List<(int, int)> rules, List<int> manual)
    {
        List<(int, int)> brokenRules = new();
        for (int i = manual.Count - 2; i >= 0; i--)
        {
            List<int> numbersAfter = manual.GetRange(i + 1, manual.Count - i - 1);
            brokenRules.AddRange((from rule in rules where rule.Item2 == manual[i] && numbersAfter.Contains(rule.Item1) select rule).ToList());
        }
        return brokenRules;
    }
}