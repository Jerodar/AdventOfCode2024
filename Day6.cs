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
        Console.WriteLine("Day 6 Part One");
        
        string[] map = PuzzleData.GetDay6Map();
        
        var path = new Dictionary<(int row, int col), Dir>();
        var obstructions = new List<(int row, int col)>();

        (int row, int col) startPosition = GetGuardPos(map);
        var position = startPosition;
        var dir = Dir.Up;

        var nextPos = StepForward(position, dir);
        while (nextPos.row >= 0 && nextPos.col >= 0 && nextPos.row < map.Length && nextPos.col < map[0].Length)
        {
            if (map[nextPos.row][nextPos.col] == '#')
            {
                dir = TurnRight(dir);
            }
            else
            {
                path.TryAdd(position, dir);
        
                // Part Two addition, simulate placing an obstruction in front and check if that creates a loop
                if (!obstructions.Contains(nextPos) && !path.ContainsKey(nextPos) && nextPos != startPosition &&
                    SimulateObstruction(map, path, position, dir))
                {
                    obstructions.Add(nextPos);
                }

                position = nextPos;
            }

            nextPos = StepForward(position, dir);
        }
        path.TryAdd(position, dir);
        
        Console.WriteLine($"{path.Count} steps");
        
        Console.WriteLine("");
        Console.WriteLine("Day 6 Part Two");
        Console.WriteLine($"{obstructions.Count} obstructions");
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
    
    private static Dir TurnRight(Dir dir)
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
    
    private static (int row, int col) StepForward((int row, int col) position, Dir dir)
    {
        switch (dir)
        {
            case Dir.Up:
                return (position.row - 1, position.col);
            case Dir.Right:
                return (position.row, position.col + 1);
            case Dir.Down:
                return (position.row + 1, position.col);
            case Dir.Left:
            default:
                return (position.row, position.col - 1);
        }
    }
    
    private static bool SimulateObstruction(string[] map, Dictionary<(int row, int col), Dir> path, (int row, int col) position, Dir dir)
    {
        // Simulates if placing an obstructing in front of the current position causes a loop
        // If the while loop condition is false, it means the guard escaped and there was no loop
        // If the path the guard takes is the same position and direction as previously, they are in a loop

        var obstruction = StepForward(position, dir);
        var simPath = new Dictionary<(int row, int col), Dir>();
        
        var nextPos = StepForward(position, dir);
        while (nextPos.row >= 0 && nextPos.col >= 0 && nextPos.row < map.Length && nextPos.col < map[0].Length)
        {
            if (map[nextPos.row][nextPos.col] == '#' || nextPos == obstruction)
            {
                dir = TurnRight(dir);
            }
            else
            {
                if (path.ContainsKey(position) && path[position] == dir)
                    return true;
                if (simPath.ContainsKey(position) && simPath[position] == dir)
                    return true;

                simPath.TryAdd(position, dir);
                position = nextPos;
            }

            nextPos = StepForward(position, dir);
        }
        
        return false;
    }
}