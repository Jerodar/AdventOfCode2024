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
        TimeSpan time = Time(() =>
        {
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
        });
        
        Console.WriteLine($"The sum of the valid calibrations is: {testSum}");
        Console.WriteLine($"Calculated in {time.TotalMilliseconds}ms");
        Console.WriteLine();
        Console.WriteLine("Day 7 Part One - optimised");
        
        calibrationList = PuzzleData.GetDay7Input();
        time = Time(() =>
        {
            testSum = 0;
            foreach (var (result, inputs) in calibrationList)
            {
                var intermediates = new List<long>() { inputs[0] };
                var isValid = false;
                for (int i = 1; i < inputs.Count; i++)
                {
                    var nextValue = inputs[i];
                    var newIntermediates = new List<long>();
                    foreach (var intermediate in intermediates)
                    {
                        var addValue = intermediate + nextValue;
                        var mulValue = intermediate * nextValue;
                        
                        if (i == (inputs.Count - 1) && (addValue == result || mulValue == result))
                        {
                            isValid = true;
                            break;
                        }
                        
                        if (addValue <= result)
                            newIntermediates.Add(addValue);
                        if (mulValue <= result)
                            newIntermediates.Add(mulValue);
                    }
                    
                    intermediates = newIntermediates;
                }

                if (isValid)
                    testSum += result;
            }
        });
        Console.WriteLine($"The sum of the valid calibrations is: {testSum}");
        Console.WriteLine($"Calculated in {time.TotalMilliseconds}ms");
        
        Console.WriteLine();
        Console.WriteLine("Day 7 Part Two");
        
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
                var isValid = false;
                for (int i = 1; i < inputs.Count; i++)
                {
                    var nextValue = inputs[i];
                    var newIntermediates = new List<long>();
                    foreach (var intermediate in intermediates)
                    {
                        var addValue = intermediate + nextValue;
                        var mulValue = intermediate * nextValue;
                        var concValue = ConcatNumbers(intermediate, nextValue);
                        
                        if (i == (inputs.Count - 1) && (addValue == result || mulValue == result || concValue == result))
                        {
                            isValid = true;
                            break;
                        }
                        
                        if (addValue <= result)
                            newIntermediates.Add(addValue);
                        if (mulValue <= result)
                            newIntermediates.Add(mulValue);
                        if (concValue <= result)
                            newIntermediates.Add(concValue);
                    }
                    
                    intermediates = newIntermediates;
                }

                if (isValid)
                    testSum += result;
            }
        });
        
        
        Console.WriteLine($"The sum of the valid calibrations is: {testSum}");
        Console.WriteLine($"Calculated in {time.TotalMilliseconds}ms");
    }

    private static long ConcatNumbers(long number1, int number2)
    {
        var digits = number2 switch
        {
            < 10 => 1,
            < 100 => 2,
            < 1000 => 3,
            < 10000 => 4,
            < 100000 => 5,
            < 1000000 => 6,
            < 10000000 => 7,
            < 100000000 => 8,
            _ => 9
        };
        var result = (number1 * (long)Math.Pow(10,digits)) + number2;
        return result;
    }
    
    private static TimeSpan Time(Action action)
    {
        var stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }
}