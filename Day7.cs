using AdventOfCode2024Input;
using System.Diagnostics;

namespace AdventOfCode2024;

public static class Day7
{
    public static void Run()
    {
        Console.WriteLine("Day 7 Part One");
        
        List<(long,List<int>)> calibrationList = PuzzleData.GetDay7Input();
        
        long testSum = 0;
        foreach (var (result, inputs) in calibrationList)
        {
            var intermediates = new List<long>() { inputs[0] };
            for (int i = 1; i < inputs.Count; i++)
            {
                var nextValue = inputs[i];
                var newIntermediates = new List<long>();
                foreach (var intermediate in intermediates)
                {
                    newIntermediates.Add(intermediate + nextValue);
                    newIntermediates.Add(intermediate * nextValue);
                }
                intermediates = newIntermediates;
            }

            if (intermediates.Contains(result))
                testSum += result;
        }
        
        Console.WriteLine($"The sum of the valid calibrations is: {testSum}");
        Console.WriteLine();
        Console.WriteLine("Day 7 Part Two");
        
        calibrationList = PuzzleData.GetDay7Input();
        TimeSpan time = Time(() =>
        {
            testSum = 0;
            foreach (var (result, inputs) in calibrationList)
            {
                var intermediates = new List<long>() { inputs[0] };
                for (int i = 1; i < inputs.Count; i++)
                {
                    var nextValue = inputs[i];
                    var newIntermediates = new List<long>();
                    foreach (var intermediate in intermediates)
                    {
                        newIntermediates.Add(intermediate + nextValue);
                        newIntermediates.Add(intermediate * nextValue);
                        newIntermediates.Add(long.Parse(intermediate.ToString() + nextValue.ToString()));
                    }

                    intermediates = newIntermediates;
                }

                if (intermediates.Contains(result))
                    testSum += result;
            }
        });
        
        
        Console.WriteLine($"The sum of the valid calibrations is: {testSum}");
        Console.WriteLine($"Calculated in {time.TotalMilliseconds}ms");
        
        Console.WriteLine();
        Console.WriteLine("Day 7 Part Two - optimised");
        
        calibrationList = PuzzleData.GetDay7Input();
        time = Time(() =>
        {
            testSum = 0;
            foreach (var (result, inputs) in calibrationList)
            {
                var intermediates = new List<long>() { inputs[0] };
                for (int i = 1; i < inputs.Count; i++)
                {
                    var nextValue = inputs[i];
                    var newIntermediates = new List<long>();
                    foreach (var intermediate in intermediates)
                    {
                        var addValue = intermediate + nextValue;
                        var mulValue = intermediate * nextValue;
                        var concValue = long.Parse(intermediate.ToString() + nextValue.ToString());
                        if (addValue <= result)
                            newIntermediates.Add(addValue);
                        if (mulValue <= result)
                            newIntermediates.Add(mulValue);
                        if (concValue <= result)
                            newIntermediates.Add(concValue);
                    }
                    
                    intermediates = newIntermediates;
                }

                if (intermediates.Contains(result))
                    testSum += result;
            }
        });
        
        
        Console.WriteLine($"The sum of the valid calibrations is: {testSum}");
        Console.WriteLine($"Calculated in {time.TotalMilliseconds}ms");
    }
    
    private static TimeSpan Time(Action action)
    {
        var stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }
}