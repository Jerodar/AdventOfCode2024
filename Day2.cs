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
            List<int> numbers = report.Split(" ").Select(int.Parse).ToList();
            
            if (IsSafeReport(numbers, true) || IsSafeReport(numbers, false))
                safeCount++;
        }
        Console.WriteLine($"safeCount {safeCount}");

        Console.WriteLine("");
        Console.WriteLine("Day 2 Part Two");
        safeCount = 0;
        foreach (var report in reportList)
        {
            List<int> numbers = report.Split(" ").Select(int.Parse).ToList();
            
            if (TestAndFixReport(numbers))
                safeCount++;
        }
        Console.WriteLine($"safeCount {safeCount}");
    }

    private static bool TestAndFixReport(List<int> record)
    {
        bool isSafe = IsSafeReport(record, true);
        if (isSafe) return true;

        isSafe = IsSafeReport(record, false);
        if (isSafe) return true;

        for (int i = 0; i < record.Count; i++)
        {
            var recordCopy = new List<int>(record);
            recordCopy.RemoveAt(i);
        
            isSafe = IsSafeReport(recordCopy, true);
            if (isSafe) return true;

            isSafe = IsSafeReport(recordCopy, false);
            if (isSafe) return true;
        }
        
        return false;
    }

    private static bool IsSafeReport(List<int> record, bool isIncreasing)
    {
        for (int i = 0; i < (record.Count - 1); i++)
        {
            var diff = record[i] - record[i+1];
            if ((diff < 0 && !isIncreasing) || (diff > 0 && isIncreasing) || (diff == 0))
                return false;

            if (Math.Abs(diff) is not (>= 1 and <= 3))
                return false;
        }
        return true;
    }
}