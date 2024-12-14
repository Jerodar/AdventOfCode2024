using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day14
{
    public static void Run()
    {
        Console.WriteLine("Day 14 Part One");

        List<(int px, int py, int vx, int vy)> robots = PuzzleData.GetDay14Robots();
        const int maxRow = 103;
        const int maxCol = 101;

        for (var seconds = 0; seconds < 100; seconds++)
        {
            for (var i = 0; i < robots.Count; i++)
            {
                robots[i] = MoveRobot(robots[i], maxRow, maxCol);
            }
        }

        int upLeft = 0;
        int downLeft = 0;
        int upRight = 0;
        int downRight = 0;
        const int middleCol = maxCol / 2;
        const int middleRow = maxRow / 2;
        foreach (var robot in robots)
        {
            if (robot.px > middleCol)
            {
                if (robot.py > middleRow)
                    downRight++;
                else if (robot.py < middleRow)
                    upRight++;
            }
            else if (robot.px < middleCol)
            {
                if (robot.py > middleRow)
                    downLeft++;
                else if (robot.py < middleRow)
                    upLeft++;
            }
        }
        var result = upRight * upLeft * downRight * downLeft;
        Console.WriteLine($"Safety factor: {result}");
        Console.WriteLine();
        Console.WriteLine("Day 14 Part Two");

        robots = PuzzleData.GetDay14Robots();

        for (var seconds = 0; seconds < 10000; seconds++)
        {
            for (var i = 0; i < robots.Count; i++)
            {
                robots[i] = MoveRobot(robots[i], maxRow, maxCol);
            }

            if (ContainsTriangle(robots))
            {
                Console.WriteLine($"Map after {seconds+1} seconds:");
                DrawMap(robots, maxRow, maxCol); // check visually if this is a Christmas tree
                Console.WriteLine();
            }
        }
    }

    private static (int px, int py, int vx, int vy)  MoveRobot((int px, int py, int vx, int vy) robot, int maxRow, int maxCol)
    {
        var newX = robot.px + robot.vx;
        var newY = robot.py + robot.vy;
        
        if (newX < 0)
            newX = maxCol + newX;
        if (newY < 0)
            newY = maxRow + newY;
        if (newX >= maxCol)
            newX -= maxCol;
        if (newY >= maxRow)
            newY -= maxRow;
        
        robot.px = newX;
        robot.py = newY;

        return robot;
    }
    
    private static bool ContainsTriangle(List<(int px, int py, int vx, int vy)> robots)
    {
        var locations = new HashSet<(int x, int y)>();
        foreach (var robot in robots)
            locations.Add((robot.px, robot.py));
        return robots.Any(robot => locations.Contains((robot.px - 2, robot.py)) &&
                                            locations.Contains((robot.px - 1, robot.py)) &&
                                            locations.Contains((robot.px - 1, robot.py + 1)) &&
                                            locations.Contains((robot.px - 1, robot.py - 1)) &&
                                            locations.Contains((robot.px, robot.py + 2)) &&
                                            locations.Contains((robot.px, robot.py + 1)) &&
                                            locations.Contains((robot.px, robot.py - 1)) &&
                                            locations.Contains((robot.px, robot.py - 2)));
    }
    
    private static void DrawMap(List<(int px, int py, int vx, int vy)> robots, int maxRow, int maxCol)
    {
        char[,] map = new char[maxRow, maxCol];
        for (int row = 0; row < maxRow; row++)
            for (int col = 0; col < maxCol; col++)
                map[row, col] = '.';
        foreach (var robot in robots)
                map[robot.py, robot.px] = '#';
        for (int row = 0; row < maxRow; row++)
        {
            for (int col = 0; col < maxCol; col++)
                Console.Write(map[row, col]);
            Console.WriteLine();
        }
    }
}