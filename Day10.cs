using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day10
{
    public static void Run()
    {
        Console.WriteLine("Day 10 Part One");

        string[] map = PuzzleData.GetDay10TopoMap();
        var maxRow = map.Length;
        var maxCol = map[0].Length;
        
        int totalScore = 0;
        int totalRating = 0;
        for (int row = 0; row < maxRow; row++)
        {
            for (int col = 0; col < maxCol; col++)
            {
                if (map[row][col] != '0') continue;
                totalScore += GetTrailHeadScore(map, row, col);
                totalRating += GetTrailHeadRating(map, row, col);
            }
        }
        Console.WriteLine($"Total score: {totalScore}");
        Console.WriteLine();
        Console.WriteLine("Day 10 Part Two");
        Console.WriteLine($"Total rating: {totalRating}");
    }

    private static int GetTrailHeadScore(string[] map, int row, int col)
    {
        var uniquePeaks = new HashSet<(int row, int col)>();
        
        List<(int row, int col)> visitedPeaks = FindPeaks(map, row, col); 
        
        foreach (var visitedPeak in visitedPeaks)
            uniquePeaks.Add(visitedPeak);

        return uniquePeaks.Count;
    }
    
    private static int GetTrailHeadRating(string[] map, int row, int col)
    {
        List<(int row, int col)> visitedPeaks = FindPeaks(map, row, col); 

        return visitedPeaks.Count;
    }

    private static List<(int row, int col)> FindPeaks(string[] map, int row, int col)
    {
        var maxRow = map.Length;
        var maxCol = map[0].Length;
        var visitedPeaks = new List<(int row, int col)>();
        var currentHeight = map[row][col];
        var nextHeight = currentHeight + 1;

        if (currentHeight == '9')
        {
            visitedPeaks.Add((row, col));
            return visitedPeaks;
        }

        if (row + 1 < maxRow && map[row + 1][col] == nextHeight)
            visitedPeaks.AddRange(FindPeaks(map, row + 1, col));
        if (row - 1 >= 0 && map[row - 1][col] == nextHeight)
            visitedPeaks.AddRange(FindPeaks(map, row - 1, col));
        if (col + 1 < maxCol && map[row][col + 1] == nextHeight)
            visitedPeaks.AddRange(FindPeaks(map, row, col + 1));
        if (col - 1 >= 0 && map[row][col - 1] == nextHeight)
            visitedPeaks.AddRange(FindPeaks(map, row, col - 1));
        
        return visitedPeaks;
    }
}