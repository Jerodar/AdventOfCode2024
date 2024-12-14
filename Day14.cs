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
}