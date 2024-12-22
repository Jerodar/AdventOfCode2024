using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day22
{
    public static void Run()
    {
        Console.WriteLine("Day 22 Part One");

        List<int> secrets = PuzzleData.GetDay22SecretNumbers();

        long sum = 0;
        var sequencesTotals = new Dictionary<(int, int, int, int), int>();
        foreach (int secret in secrets)
        {
            var sequences = new Dictionary<(int, int, int, int), int>();
            long result = secret;
            int[] prices = new int[2000];
            for (int i = 0; i < 2000; i++)
            {
                result = ((result * 64) ^ result) % 16777216;
                result = ((result / 32) ^ result) % 16777216;
                result = ((result * 2048) ^ result) % 16777216;
                prices[i] = (int)result % 10;

                if (i < 4) continue;
                
                var diff1 = prices[i] - prices[i-1];
                var diff2 = prices[i-1] - prices[i-2];
                var diff3 = prices[i-2] - prices[i-3];
                var diff4 = prices[i-3] - prices[i-4];
                sequences.TryAdd((diff1, diff2, diff3, diff4), prices[i]);
            }
            Console.WriteLine(result);
            sum += result;
            foreach (var sequence in sequences)
            {
                if(!sequencesTotals.TryAdd(sequence.Key, sequence.Value))
                    sequencesTotals[sequence.Key] += sequence.Value;
            }
        }
        Console.WriteLine($"Total: {sum}");
        Console.WriteLine();
        Console.WriteLine("Day 22 Part Two");
        var maxBananas = sequencesTotals.Max(p => p.Value);
        Console.WriteLine($"Max banana's: {maxBananas}");
        var winningSequences = (from keyvalue in sequencesTotals where keyvalue.Value == maxBananas select keyvalue.Key).ToList();
        foreach (var sequence in winningSequences)
            Console.WriteLine(sequence);
        // 2512 too high
    }
}