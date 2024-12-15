using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day15
{
    public static void Run()
    {
        Console.WriteLine("Day 15 Part One");
        
        string[] mapInput = PuzzleData.GetDay15Map();
        string commands = PuzzleData.GetDay15Commands();
        int maxRow = mapInput.Length;
        int maxCol = mapInput[0].Length;
        char[,] map = new char[maxRow, maxCol];
        (int row, int col) robot = (0, 0);

        for (int row = 0; row < mapInput.Length; row++)
        {
            for (int col = 0; col < mapInput[row].Length; col++)
            {
                if (mapInput[row][col] == '@')
                {
                    robot = (row, col);
                    map[row, col] = '.';
                }
                else
                {
                    map[row, col] = mapInput[row][col];
                }
            }
        }

        foreach (char c in commands)
        {
            robot = c switch
            {
                '^' => MoveUp(robot, map),
                'v' => MoveDown(robot, map, maxRow),
                '<' => MoveLeft(robot, map),
                '>' => MoveRight(robot, map, maxCol),
                _ => robot
            };
        }

        long gpsSum = 0;
        for (int row = 0; row < maxRow; row++)
        {
            for (int col = 0; col < maxCol; col++)
            {
                if (map[row, col] != 'O') continue;
                gpsSum += (100 * row) + col;
            }
        }

        PrintMap(robot, map, maxRow, maxCol);
        Console.WriteLine($"gps sum: {gpsSum}");
    }

    private static (int row, int col) MoveUp((int row, int col) robot, char[,] map)
    {
        for (int row = robot.row - 1; row >= 0; row--)
        {
            if (map[row, robot.col] == '#') break;
            if (map[row, robot.col] == 'O') continue;
            
            map[row, robot.col] = 'O';
            map[robot.row - 1, robot.col] = '.';
            robot.row -= 1;
            break;
        }

        return robot;
    }
    
    private static (int row, int col) MoveDown((int row, int col) robot, char[,] map, int maxRow)
    {
        for (int row = robot.row + 1; row < maxRow; row++)
        {
            if (map[row, robot.col] == '#') break;
            if (map[row, robot.col] == 'O') continue;
            
            
            map[row, robot.col] = 'O';
            map[robot.row + 1, robot.col] = '.';
            robot.row += 1;
            break;
        }

        return robot;
    }
    
    private static (int row, int col) MoveLeft((int row, int col) robot, char[,] map)
    {
        for (int col = robot.col - 1; col >= 0; col--)
        {
            if (map[robot.row, col] == '#') break;
            if (map[robot.row, col] == 'O') continue;
            
            map[robot.row, col] = 'O';
            map[robot.row, robot.col - 1] = '.';
            robot.col -= 1;
            break;
        }

        return robot;
    }
    
    private static (int row, int col) MoveRight((int row, int col) robot, char[,] map, int maxCol)
    {
        for (int col = robot.col + 1; col < maxCol; col++)
        {
            if (map[robot.row, col] == '#') break;
            if (map[robot.row, col] == 'O') continue;
            
            map[robot.row, col] = 'O';
            map[robot.row, robot.col + 1] = '.';
            robot.col += 1;
            break;
        }

        return robot;
    }

    private static void PrintMap((int row, int col) robot, char[,] map, int maxRow, int maxCol)
    {
        map[robot.row, robot.col] = '@';
        for (int row = 0; row < maxRow; row++)
        {
            for (int col = 0; col < maxCol; col++)
            {
                Console.Write(map[row, col]);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
        map[robot.row, robot.col] = '.';
    }
}