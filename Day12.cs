using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day12
{
    private enum Dir
    {
        Up,
        Down,
        Left,
        Right,
        None
    }
    
    public static void Run()
    {
        Console.WriteLine("Day 12 Part One");

        string[] map = PuzzleData.GetDay12GardenMap();
        var maxRow = map.Length;
        var maxCol = map[0].Length;
        var done = new HashSet<(int col, int row)>();
        
        long cost = 0;
        for (int row = 0; row < map.Length; row++)
        {
            for (int col = 0; col < map[row].Length; col++)
            {
                (int perimeter, int area) regionResult = FindRegion(map, done,row, col, maxRow, maxCol,Dir.None);
                if (regionResult.area > 0)
                    Console.WriteLine($"Region {map[row][col]} with area {regionResult.area} and perimeter {regionResult.perimeter} has cost {regionResult.perimeter * regionResult.area}");
                cost += regionResult.perimeter * regionResult.area;
            }
        }
        Console.WriteLine($"Total cost: {cost}");
    }

    private static (int perimeter, int area) FindRegion(string[] map, HashSet<(int col, int row)> done, int row, int col, int maxRow, int maxCol, Dir direction)
    {
        if (!done.Add((row, col)))
            return (0,0);

        var currentRegion = map[row][col];
        var area = 1;
        var perimeter = 4;

        if (direction != Dir.None)
            perimeter--;

        if (direction != Dir.Down && row - 1 >= 0 && map[row - 1][col] == currentRegion)
        {
            perimeter--;
            var upResult = FindRegion(map, done, row - 1, col, maxRow, maxCol,Dir.Up);
            area += upResult.area;
            perimeter += upResult.perimeter;
        }
        if (direction != Dir.Up && row + 1 < maxRow && map[row + 1][col] == currentRegion)
        {
            perimeter--;
            var upResult = FindRegion(map, done, row + 1, col, maxRow, maxCol,Dir.Down);
            area += upResult.area;
            perimeter += upResult.perimeter;
        }
        if (direction != Dir.Right && col - 1 >= 0 && map[row][col - 1] == currentRegion)
        {
            perimeter--;
            var upResult = FindRegion(map, done, row, col - 1, maxRow, maxCol,Dir.Left);
            area += upResult.area;
            perimeter += upResult.perimeter;
        }
        if (direction != Dir.Left && col + 1 < maxCol && map[row][col + 1] == currentRegion)
        {
            perimeter--;
            var upResult = FindRegion(map, done, row, col + 1, maxRow, maxCol,Dir.Right);
            area += upResult.area;
            perimeter += upResult.perimeter;
        }
        
        return (perimeter, area);
    }
}