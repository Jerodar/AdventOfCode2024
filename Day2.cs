using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day2
{
    public static void Run()
    {
        Console.WriteLine("Day 2 Part One");

        List<string> reportList = PuzzleData.GetDayTwoList();

        int safeCount = 0;
        foreach (var report in reportList)
        {
            Console.WriteLine($"Report: {report}");
            List<int> numbers = report.Split(" ").Select(int.Parse).ToList();
            if (numbers[0] == numbers[1])
            {
                Console.WriteLine($"first numbers are not increasing or decreasing!");
                continue;
            }
            bool isIncreasing = numbers[0] < numbers[1];
            bool isSafe = true;
            for (int i = 0; i < (numbers.Count -1); i++)
            {
                var diff = numbers[i] - numbers[i + 1];
                var absDiff = Math.Abs(diff);
                if ((diff < 0 && !isIncreasing) || (diff > 0 && isIncreasing) || (diff == 0))
                {
                    Console.WriteLine($"Wrong direction!");
                    isSafe = false;
                    break;
                }
                if (absDiff < 1 || absDiff > 3)
                {
                    Console.WriteLine($"not at least one and less than 3!");
                    isSafe = false;
                    break;
                }
            }

            if (isSafe)
            {
                Console.WriteLine($"Report is safe.");
                safeCount++;
            }
        }
        Console.WriteLine($"safeCount {safeCount}");
    }
}