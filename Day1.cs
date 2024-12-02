using AdventOfCode2024Input;
using System.Diagnostics;

namespace AdventOfCode2024;

public static class Day1
{
    public static void Run()
    {
        Console.WriteLine("Day 1 Part One");

        var firstList = PuzzleData.GetDayOneFirstList();
        var secondList = PuzzleData.GetDayOneSecondList();
        
        firstList.Sort();
        secondList.Sort();

        var distanceSum = 0;

        for (var i = 0; i < firstList.Count; i++)
        {
            var distance = Math.Abs(firstList[i] - secondList[i]);
            Console.WriteLine($"First {firstList[i]} Second {secondList[i]} Distance {distance}");
            distanceSum += distance;
        } 

        Console.WriteLine("The total distance is " + distanceSum);
        Console.WriteLine("");
        Console.WriteLine("Day 1 Part Two");
        var similarityScore = 0;
        
        Console.WriteLine("Using .Count");
        TimeSpan time = Time(() =>
        {
            foreach (var firstItem in firstList)
            {
                var similarityCount = secondList.Count(x => x == firstItem);
                similarityScore += similarityCount * firstItem;
            }
        });
        Console.WriteLine($"SimilarityScore {similarityScore} in {time.TotalMilliseconds} ms");
        
        similarityScore = 0;
        Console.WriteLine("Using linq");
        time = Time(() =>
        {
            similarityScore += (from firstItem in firstList let similarityCount = secondList.Count(x => x == firstItem) select similarityCount * firstItem).Sum();
        });
        Console.WriteLine($"SimilarityScore {similarityScore} in {time.TotalMilliseconds} ms");
        
        
        similarityScore = 0;
        Console.WriteLine("Using custom search");
        time = Time(() =>
        {
            for (var i = 0; i < firstList.Count; i++)
            {
                var similarityCount = 0;
                if (firstList[i] < secondList[i])
                {
                    similarityCount += CountMatchesBackwards(firstList[i], i, secondList);
                }
                else if (firstList[i] > secondList[i])
                {
                    similarityCount += CountMatchesForward(firstList[i], i, secondList);
                }
                else
                {
                    similarityCount++;
                    similarityCount += CountMatchesForward(firstList[i], i, secondList);
                    similarityCount += CountMatchesBackwards(firstList[i], i, secondList);
                }
            
                similarityScore += similarityCount * firstList[i];   
            }
        });
        Console.WriteLine($"SimilarityScore {similarityScore} in {time.TotalMilliseconds} ms");
    }

    private static int CountMatchesForward(int target, int start, List<int> list)
    {
        var similarityCount = 0;
        var j = start + 1;
        while (j < list.Count && target >= list[j])
        {
            if (list[j] == target)
                similarityCount++;
            j++;
        }
        return similarityCount;
    }

    private static int CountMatchesBackwards(int target, int start, List<int> list)
    {
        var similarityCount = 0;
        var j = start - 1;
        while (j >= 0 && target <= list[j])
        {
            if (list[j] == target)
                similarityCount++;
            j--;
        }
        return similarityCount;
    }

    private static TimeSpan Time(Action action)
    {
        var stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }
}