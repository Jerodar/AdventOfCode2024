using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day11
{
    public static void Run()
    {
        Console.WriteLine("Day 11 Part One");

        List<long> stones = PuzzleData.GetDay11Stones();
        var numberCount = new Dictionary<long, long>();
        foreach (long stone in stones)
        {
            if (!numberCount.TryAdd(stone, 1))
                numberCount[stone]++;
        }

        for (int blink = 25; blink > 0; blink--)
        {
            numberCount = Blink(numberCount);
        }

        long count = 0;
        foreach (KeyValuePair<long, long> number in numberCount)
            count+= number.Value;

        Console.WriteLine(count);
        Console.WriteLine();

        Console.WriteLine("Day 11 Part Two");

        for (int blink = 50; blink > 0; blink--)
        {
            numberCount = Blink(numberCount);
        }

        count = 0;
        foreach (KeyValuePair<long, long> number in numberCount)
            count += number.Value;

        Console.WriteLine(count);
    }

    private static Dictionary<long, long> Blink(Dictionary<long, long> numberCount)
    {
        var newNumberCount = new Dictionary<long, long>();

        for (int i = 0; i < numberCount.Count; i++)
        {
            var number = numberCount.ElementAt(i).Key;
            if (number == 0)
            {
                if (!newNumberCount.TryAdd(1, numberCount[number]))
                    newNumberCount[1] += numberCount[number];
                continue;
            }

            var digits = GetDigits(number);
            if (digits % 2 == 0)
            {
                var splitter = (long)Math.Pow(10, (int)(digits / 2));
                var msd = number / splitter;
                var lsd = number % splitter;
                if (!newNumberCount.TryAdd(lsd, numberCount[number]))
                    newNumberCount[lsd] += numberCount[number];
                if (!newNumberCount.TryAdd(msd, numberCount[number]))
                    newNumberCount[msd] += numberCount[number];
                continue;
            }

            var mulResult = number * 2024;
            if (!newNumberCount.TryAdd(mulResult, numberCount[number]))
                newNumberCount[mulResult] += numberCount[number];
        }
        return newNumberCount;
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