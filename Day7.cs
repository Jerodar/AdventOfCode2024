using AdventOfCode2024Input;

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
    }
}