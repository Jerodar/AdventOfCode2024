using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day11
{
    public static void Run()
    {
        Console.WriteLine("Day 11 Part One");

        List<long> stones = PuzzleData.GetDay11Stones();

        for (int blink = 25; blink > 0; blink--)
        {
            for (int i = stones.Count - 1; i >= 0; i--)
            {
                if (stones[i] == 0)
                {
                    stones[i] = 1;
                    continue;
                }

                var digits = GetDigits(stones[i]);
                if (digits % 2 == 0)
                {
                    var splitter = (long)Math.Pow(10, (int)(digits / 2));
                    stones.Add(stones[i] / splitter);
                    stones[i] %= splitter;
                    continue;
                }

                stones[i] *= 2024;
            }
        }

        Console.WriteLine(stones.Count);
    }

    private static int GetDigits(long number)
    {
        var digits = number switch
        {
            < 10 => 1,
            < 100 => 2,
            < 1000 => 3,
            < 10000 => 4,
            < 100000 => 5,
            < 1000000 => 6,
            < 10000000 => 7,
            < 100000000 => 8,
            < 1000000000 => 9,
            < 10000000000 => 10,
            < 100000000000 => 11,
            < 1000000000000 => 12,
            < 10000000000000 => 13,
            < 100000000000000 => 14,
            < 1000000000000000 => 15,
            < 10000000000000000 => 16,
            < 100000000000000000 => 17,
            < 1000000000000000000 => 18,
            _ => 19
        };
        return digits;
    }
}