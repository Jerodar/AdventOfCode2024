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
                (int perimeter, int area) = FindRegion(map, done,row, col, maxRow, maxCol,Dir.None);
                if (area > 0)
                    Console.WriteLine($"Region {map[row][col]} with area {area} and perimeter {perimeter} has cost {perimeter * area}");
                cost += perimeter * area;
            }
        }
        Console.WriteLine($"Total cost: {cost}");
        Console.WriteLine();

        Console.WriteLine("Day 12 Part Two");

        done = new HashSet<(int col, int row)>();

        cost = 0;
        for (int row = 0; row < map.Length; row++)
        {
            for (int col = 0; col < map[row].Length; col++)
            {
                var edges = new HashSet<(int col, int row, Dir Direction)>();
                int area = FindRegionSides(map, done, row, col, maxRow, maxCol, Dir.None, edges);
                if (area > 0)
                {
                    int sides = CalculateSides(edges);
                    Console.WriteLine($"Region {map[row][col]} with area {area} and sides {sides} has cost {area * sides}");
                    cost += area * sides;
                }    
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

    private static int FindRegionSides(string[] map, HashSet<(int col, int row)> done, int row, int col, int maxRow, int maxCol, Dir direction, HashSet<(int col, int row, Dir Direction)> edges)
    {
        if (!done.Add((row, col)))
            return 0;

        var currentRegion = map[row][col];
        var area = 1;

        if (row - 1 < 0 || map[row - 1][col] != currentRegion)
        {
            edges.Add((col,row,Dir.Up));
        }
        else if (direction != Dir.Down)
        {
            area += FindRegionSides(map, done, row - 1, col, maxRow, maxCol, Dir.Up, edges);
        }

        if(row + 1 >= maxRow || map[row + 1][col] != currentRegion)
        {
            edges.Add((col, row, Dir.Down));
        }
        else if (direction != Dir.Up)
        {
            area += FindRegionSides(map, done, row + 1, col, maxRow, maxCol, Dir.Down, edges);
        }

        if(col - 1 < 0 || map[row][col - 1] != currentRegion)
        {
            edges.Add((col, row, Dir.Left));
        }
        else if (direction != Dir.Right)
        {
            area += FindRegionSides(map, done, row, col - 1, maxRow, maxCol, Dir.Left, edges);
        }

        if (col + 1 >= maxCol || map[row][col + 1] != currentRegion)
        {
            edges.Add((col, row, Dir.Right));
        }
        else if (direction != Dir.Left)
        {
            area += FindRegionSides(map, done, row, col + 1, maxRow, maxCol, Dir.Right, edges);
        }

        return area;
    }

    private static int CalculateSides(HashSet<(int col, int row, Dir Direction)> edges)
    {
        int sides = 2; // 1 vertical and 1 horizontal side is not counted below

        var verticalEdges = edges.Where(edge => edge.Direction is Dir.Up or Dir.Down);
        var sortedVertical = verticalEdges.OrderBy(edge => edge.Direction).ThenBy(edge => edge.col).ThenBy(edge => edge.row).ToList();

        var horizontalEdges = edges.Where(edge => edge.Direction is Dir.Up or Dir.Down);
        var sortedHorizontal = horizontalEdges.OrderBy(edge => edge.Direction).ThenBy(edge => edge.row).ThenBy(edge => edge.col).ToList();

        for (int i = 0; i < sortedVertical.Count - 1; i++)
        {
            if (sortedVertical[i].Direction != sortedVertical[i+1].Direction ||
                sortedVertical[i].col != sortedVertical[i + 1].col ||
                sortedVertical[i].row + 1 != sortedVertical[i + 1].row)
                sides++;
        }

        for (int i = 0; i < sortedHorizontal.Count - 1; i++)
        {
            if (sortedHorizontal[i].Direction != sortedHorizontal[i + 1].Direction ||
                sortedHorizontal[i].row != sortedHorizontal[i + 1].row ||
                sortedHorizontal[i].col + 1 != sortedHorizontal[i + 1].col)
                sides++;
        }

        return sides;
    }
}