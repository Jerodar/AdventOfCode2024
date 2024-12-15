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

        var gpsSum = CalculateGpsSum(maxRow, maxCol, map);

        PrintMap(robot, map, maxRow, maxCol);
        Console.WriteLine($"gps sum: {gpsSum}");
        
        Console.WriteLine();
        Console.WriteLine("Day 15 Part Two");
        
        map = new char[maxRow, maxCol*2];
        
        for (int row = 0; row < mapInput.Length; row++)
        {
            for (int col = 0; col < mapInput[row].Length; col++)
            {
                if (mapInput[row][col] == '@')
                {
                    robot = (row, col*2);
                    map[row, col*2] = '.';
                    map[row, col*2 + 1] = '.';
                }
                else if (mapInput[row][col] == 'O')
                {
                    map[row, col*2] = '[';
                    map[row, col*2 + 1] = ']';
                }
                else
                {
                    map[row, col*2] = mapInput[row][col];
                    map[row, col*2 + 1] = mapInput[row][col];
                }
            }
        }
        
        maxCol *= 2;
        PrintMap(robot, map, maxRow, maxCol);

        foreach (char c in commands)
        {
            switch (c)
            {
                case '^':
                    if (CanMoveUp(robot.row, robot.col, map))
                    {
                        MoveWideUp(robot.row - 1, robot.col, map);
                        robot.row--;
                    }
                    break;
                case 'v':
                    if (CanMoveDown(robot.row, robot.col, map, maxRow))
                    {
                        MoveWideDown(robot.row + 1, robot.col, map);
                        robot.row++;
                    }
                    break;
                case '<':
                    robot = MoveWideLeft(robot, map);
                    break;
                case '>':
                    robot = MoveWideRight(robot, map, maxCol);
                    break;
            }
        }

        gpsSum = CalculateGpsSum(maxRow, maxCol, map);

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
    
    private static long CalculateGpsSum(int maxRow, int maxCol, char[,] map)
    {
        long gpsSum = 0;
        for (int row = 0; row < maxRow; row++)
        {
            for (int col = 0; col < maxCol; col++)
            {
                if (map[row, col] != 'O' && map[row, col] != '[') continue;
                gpsSum += (100 * row) + col;
            }
        }

        return gpsSum;
    }
    
    private static bool CanMoveUp(int startRow, int startCol, char[,] map)
    {
        for (int row = startRow - 1; row >= 0; row--)
        {
            if (map[row, startCol] == '#') return false;
            if (map[row, startCol] == '[')
            { 
                if(CanMoveUp(row, startCol + 1, map)) continue;
                break;
            }
            if (map[row, startCol] == ']')
            { 
                if(CanMoveUp(row, startCol - 1, map)) continue;
                break;
            }

            return true;
        }
        return false;
    }

    private static bool CanMoveDown(int startRow, int startCol, char[,] map, int maxRow)
    {
        for (int row = startRow + 1; row < maxRow; row++)
        {
            if (map[row, startCol] == '#') return false;
            if (map[row, startCol] == '[')
            { 
                if(CanMoveDown(row, startCol + 1, map, maxRow)) continue;
                break;
            }
            if (map[row, startCol] == ']')
            { 
                if(CanMoveDown(row, startCol - 1, map, maxRow)) continue;
                break;
            }

            return true;
        }
        return false;
    }
    
    private static void MoveWideUp(int startRow, int startCol, char[,] map)
    {
        
        if (map[startRow, startCol] == ']') startCol -= 1;
        if (map[startRow, startCol] != '[') return;
        
        if (map[startRow - 1, startCol] == '[' || map[startRow - 1, startCol] == ']')
            MoveWideUp(startRow - 1, startCol, map);
        
        if (map[startRow - 1, startCol + 1] == '[')
            MoveWideUp(startRow - 1, startCol + 1, map);
        
        if (map[startRow - 1, startCol] == '.' && map[startRow - 1, startCol + 1] == '.')
        {
            map[startRow - 1, startCol] = '[';
            map[startRow - 1, startCol + 1] = ']';
            map[startRow, startCol] = '.';
            map[startRow, startCol + 1] = '.';
        }
        else
        {
            Console.WriteLine("ERROR!");
        }
        
    }
    
    private static void MoveWideDown(int startRow, int startCol, char[,] map)
    {
        if (map[startRow, startCol] == ']') startCol -= 1;
        if (map[startRow, startCol] != '[') return;
        
        if (map[startRow + 1, startCol] == '[' || map[startRow + 1, startCol] == ']')
            MoveWideDown(startRow + 1, startCol, map);
        
        if (map[startRow + 1, startCol + 1] == '[')
            MoveWideDown(startRow + 1, startCol + 1, map);
        
        if (map[startRow + 1, startCol] == '.' && map[startRow + 1, startCol + 1] == '.')
        {
            map[startRow + 1, startCol] = '[';
            map[startRow + 1, startCol + 1] = ']';
            map[startRow, startCol] = '.';
            map[startRow, startCol + 1] = '.';
        }
        else
        {
            Console.WriteLine("ERROR!");
        }
    }
    
    private static (int row, int col) MoveWideLeft((int row, int col) robot, char[,] map)
    {
        for (int col = robot.col - 1; col >= 0; col--)
        {
            if (map[robot.row, col] == '#') break;
            if (map[robot.row, col] == '[' || map[robot.row, col] == ']') continue;
            
            for (int revCol = col; revCol < robot.col; revCol++)
                map[robot.row, revCol] = map[robot.row, revCol+1];

            robot.col -= 1;
            break;
        }

        return robot;
    }
    
    private static (int row, int col) MoveWideRight((int row, int col) robot, char[,] map, int maxCol)
    {
        for (int col = robot.col + 1; col < maxCol; col++)
        {
            if (map[robot.row, col] == '#') break;
            if (map[robot.row, col] == '[' || map[robot.row, col] == ']') continue;
            
            for (int revCol = col; revCol > robot.col; revCol--)
                map[robot.row, revCol] = map[robot.row, revCol-1];
            
            robot.col += 1;
            break;
        }

        return robot;
    }
}