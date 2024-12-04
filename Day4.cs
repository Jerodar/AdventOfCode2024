using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day4
{
    public static void Run()
    {
        Console.WriteLine("Day 4 Part One");
        string[] wordSearchInput = PuzzleData.GetDay4Array();

        int xmasCount = 0;
        for (int row = 0; row < (wordSearchInput.Length); row++)
        {
            for (int col = 0; col < (wordSearchInput[row].Length); col++)
            {
                if (wordSearchInput[row][col] == 'X')
                    xmasCount += IsXmas(wordSearchInput, row, col);
            }
        }
        Console.WriteLine(xmasCount);
        Console.WriteLine("");
        Console.WriteLine("Day 4 Part Two");

        int crossmasCount = 0;
        for (int row = 0; row < (wordSearchInput.Length); row++)
        {
            for (int col = 0; col < (wordSearchInput[row].Length); col++)
            {
                if (wordSearchInput[row][col] == 'A')
                    crossmasCount += IsCrossMas(wordSearchInput, row, col);
            }
        }
        Console.WriteLine(crossmasCount);
    }

    private static int IsXmas(string[] wordSearchInput, int xRow, int xCol)
    {
        int xMasCount = 0;
        int maxRow = wordSearchInput.Length - 1;
        int maxCol = wordSearchInput[xRow].Length - 1;

        // Check for remaining MAS all around the X, one X can connect to multiple MAS
        if (xRow > 0 && xCol > 0 && wordSearchInput[xRow - 1][xCol - 1] == 'M')
        {
            xMasCount += IsXmasInDir(wordSearchInput, xRow, xCol, -1, -1);
        }
        if (xRow > 0 && wordSearchInput[xRow - 1][xCol] == 'M')
        {
            xMasCount += IsXmasInDir(wordSearchInput, xRow, xCol, -1, 0);
        }
        if (xRow > 0 && xCol < maxCol && wordSearchInput[xRow - 1][xCol + 1] == 'M')
        {
            xMasCount += IsXmasInDir(wordSearchInput, xRow, xCol, -1, 1);
        }
        if (xCol > 0 && wordSearchInput[xRow][xCol - 1] == 'M')
        {
            xMasCount += IsXmasInDir(wordSearchInput, xRow, xCol, 0, -1);
        }
        if (xCol < maxCol && wordSearchInput[xRow][xCol + 1] == 'M')
        {
            xMasCount += IsXmasInDir(wordSearchInput, xRow, xCol, 0, 1);
        }
        if (xRow < maxRow && xCol > 0 && wordSearchInput[xRow + 1][xCol - 1] == 'M')
        {
            xMasCount += IsXmasInDir(wordSearchInput, xRow, xCol, 1, -1);
        }
        if (xRow < maxRow && wordSearchInput[xRow + 1][xCol] == 'M')
        {
            xMasCount += IsXmasInDir(wordSearchInput, xRow, xCol, 1, 0);
        }
        if (xRow < maxRow && xCol < maxCol - 1 && wordSearchInput[xRow + 1][xCol + 1] == 'M')
        {
            xMasCount += IsXmasInDir(wordSearchInput, xRow, xCol, 1, 1);
        }

        return xMasCount;
    }

    private static int IsXmasInDir(string[] wordSearchInput, int xRow, int xCol, int rowDir, int colDir)
    {
        int sRow = xRow + (rowDir * 3);
        int sCol = xCol + (colDir * 3);

        // Check boundaries
        if (sRow < 0 || sRow >= wordSearchInput.Length) { return 0; }
        if (sCol < 0 || sCol >= wordSearchInput[xRow].Length) { return 0; }

        int aRow = xRow + (rowDir * 2);
        int aCol = xCol + (colDir * 2);

        if (wordSearchInput[aRow][aCol] == 'A' && wordSearchInput[sRow][sCol] == 'S') { return 1; };

        return 0;
    }

    private static int IsCrossMas(string[] wordSearchInput, int aRow, int aCol)
    {
        int minRowCol = 1;
        int maxRow = wordSearchInput.Length - 2;
        int maxCol = wordSearchInput[aRow].Length - 2;

        // Check boundaries
        if (aRow < minRowCol || aRow > maxRow || aCol < minRowCol || aCol > maxCol) { return 0; }

        // Check for remaining MAS in the diagonals of A, one A can only be part of one cross MAS
        if (wordSearchInput[aRow - 1][aCol - 1] == 'M')
        {
            if(IsCrossMasInDir(wordSearchInput, aRow, aCol, -1, -1)) { return 1; }
        }
        if (wordSearchInput[aRow - 1][aCol + 1] == 'M')
        {
            if (IsCrossMasInDir(wordSearchInput, aRow, aCol, -1, 1)) { return 1; }
        }
        if (wordSearchInput[aRow + 1][aCol - 1] == 'M')
        {
            if (IsCrossMasInDir(wordSearchInput, aRow, aCol, 1, -1)) { return 1; }
        }
        if (wordSearchInput[aRow + 1][aCol + 1] == 'M')
        {
            if (IsCrossMasInDir(wordSearchInput, aRow, aCol, 1, 1)) { return 1; }
        }

        return 0;
    }

    private static bool IsCrossMasInDir(string[] wordSearchInput, int aRow, int aCol, int rowDir, int colDir)
    {
        if (wordSearchInput[aRow - rowDir][aCol - colDir] == 'S' &&
            ((wordSearchInput[aRow - rowDir][aCol + colDir] == 'M' && wordSearchInput[aRow + rowDir][aCol - colDir] == 'S') ||
            (wordSearchInput[aRow + rowDir][aCol - colDir] == 'M' && wordSearchInput[aRow - rowDir][aCol + colDir] == 'S'))) 
        {
            return true; 
        };

        return false;
    }
}
