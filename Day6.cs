using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day6
{
    private enum Dir
    {
        Up,
        Down,
        Left,
        Right
    }
    
    public static void Run()
    {
        Console.WriteLine($"Day 6 Part One");

        string[] map = PuzzleData.GetDay6Map();

        (int x, int y) position = GetGuardPos(map);
        var dir = Dir.Up;
        var done = false;
        var steps = new List<(int x, int y)>();
        while (position.x >= 0 && position.y >= 0 && position.x < map.Count() && position.y < map[0].Count())
        {
            if (map[position.x][position.y] == '#')
            {
                position = StepBack(position, dir);
                dir = Turn(dir);
            }
            else
            {
                if (!steps.Contains(position))
                    steps.Add(position);
                position = StepForward(position, dir);
            }
        }
        Console.WriteLine($"{steps.Count} steps");
        
    }

    private static (int x, int y) StepForward((int x, int y) position, Dir dir)
    {
        switch (dir)
        {
            case Dir.Up:
                return (position.x - 1, position.y);
            case Dir.Right:
                return (position.x, position.y + 1);
            case Dir.Down:
                return (position.x + 1, position.y);
            case Dir.Left:
            default:
                return (position.x, position.y - 1);
        }
    }

    private static (int x, int y) StepBack((int x, int y) position, Dir dir)
    {
        switch (dir)
        {
            case Dir.Up:
                return (position.x + 1, position.y);
            case Dir.Right:
                return (position.x, position.y - 1);
            case Dir.Down:
                return (position.x - 1, position.y);
            case Dir.Left:
            default:
                return (position.x, position.y + 1);
        }
    }

    private static Dir Turn(Dir dir)
    {
        switch (dir)
        {
            case Dir.Up:
                return Dir.Right;
            case Dir.Right:
                return Dir.Down;
            case Dir.Down:
                return Dir.Left;
            case Dir.Left:
            default:
                return Dir.Up;
        }
    }

    private static (int, int) GetGuardPos(string[] map)
    {
        var guardPos = (0, 0);
        for (int i = 0; i < map.Length; i++)
        {
            var guardIndex = map[i].IndexOf('^');
            if (guardIndex != -1)
            {
                guardPos = (i, guardIndex);
                break;
            }
        }

        return guardPos;
    }
}