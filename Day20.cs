using System.Xml.Schema;
using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day20
{
    private enum Dir
    {
        None,
        Up,
        Down,
        Left,
        Right
    }
    
    public static void Run()
    {
        Console.WriteLine("Day 20 Part One");

        string[] map = PuzzleData.GetDay20Map();
        
        (int row, int col) start = FindStart(map);
        Dictionary<(int row, int col), int> path = GetPath(map, start);
        var shortcuts = FindShortcuts(path);
        
        Console.WriteLine($"Total: {shortcuts.Count}");
    }

    private static (int row, int col) FindStart(string[] map)
    {
        for (int row = 0; row < map.Length; row++)
            for (int col = 0; col < map[0].Length; col++)
                if (map[row][col] == 'S') return (row, col);
        return (0, 0);
    }

    private static Dictionary<(int row, int col), int> GetPath(string[] map, (int row, int col) start)
    {
        var path = new Dictionary<(int row, int col), int>();
        var current = start;
        var cost = 0;
        var dir = Dir.None;
        
        while (map[current.row][current.col] != 'E')
        {
            path.Add(current, cost);
            cost++;
            if (dir != Dir.Up && map[current.row + 1][current.col] != '#')
            {
                dir = Dir.Down;
                current = (current.row + 1,current.col);
            }
            else if (dir != Dir.Down && map[current.row - 1][current.col] != '#')
            {
                dir = Dir.Up;
                current = (current.row - 1,current.col);
            }
            else if (dir != Dir.Left && map[current.row][current.col + 1] != '#')
            {
                dir = Dir.Right;
                current = (current.row ,current.col + 1);
            }
            else if (dir != Dir.Right && map[current.row][current.col - 1] != '#')
            {
                dir = Dir.Left;
                current = (current.row ,current.col - 1);
            }
        }
        path.Add(current, cost);

        return path;
    }

    private static Dictionary<(int,int,int,int),int> FindShortcuts(Dictionary<(int row, int col), int> path)
    {
        var shortcuts = new Dictionary<(int,int,int,int),int>();
        const int minProfit = 100;
        foreach (var (pos, cost) in path)
        {
            if (path.ContainsKey((pos.row + 2, pos.col)) && !path.ContainsKey((pos.row + 1, pos.col)) && (path[(pos.row + 2, pos.col)] - cost - 2) >= minProfit)
                shortcuts.Add((pos.row,pos.col, pos.row + 2, pos.col), path[(pos.row + 2, pos.col)] - cost - 2);
            
            if (path.ContainsKey((pos.row - 2, pos.col)) && !path.ContainsKey((pos.row - 1, pos.col)) && (path[(pos.row - 2, pos.col)] - cost - 2) >= minProfit)
                shortcuts.Add((pos.row,pos.col, pos.row - 2, pos.col), path[(pos.row - 2, pos.col)] - cost - 2);
            
            if (path.ContainsKey((pos.row, pos.col + 2)) && !path.ContainsKey((pos.row, pos.col + 1)) && (path[(pos.row, pos.col + 2)] - cost - 2) >= minProfit)
                shortcuts.Add((pos.row,pos.col, pos.row, pos.col + 2), path[(pos.row, pos.col + 2)] - cost - 2);
            
            if (path.ContainsKey((pos.row, pos.col - 2)) && !path.ContainsKey((pos.row, pos.col - 1)) && (path[(pos.row, pos.col - 2)] - cost - 2) >= minProfit)
                shortcuts.Add((pos.row,pos.col, pos.row, pos.col - 2), path[(pos.row, pos.col - 2)] - cost - 2);
        }

        return shortcuts;
    }
}