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
                    xmasCount += IsXmas(wordSearchInput,row,col);
            }
        }
        Console.WriteLine(xmasCount);
    }

    private static int IsXmas(string[] wordSearchInput, int row, int col)
    {
        int xMasCount = 0;
        int maxRow = wordSearchInput.Length - 1;
        int maxCol = wordSearchInput[row].Length - 1;
        
        // Check for remaining MAS all around the X, one X can connect to multiple MAS
        if (row > 0 && col > 0 && wordSearchInput[row - 1][col - 1] == 'M')
        {
            xMasCount += IsMas(wordSearchInput,row,col,-1,-1);
        }
        if (row > 0 && wordSearchInput[row - 1][col] == 'M')
        {
            xMasCount += IsMas(wordSearchInput,row,col,-1,0);
        }
        if (row > 0 && col < maxCol && wordSearchInput[row - 1][col + 1] == 'M')
        {
            xMasCount += IsMas(wordSearchInput,row,col,-1,1);
        }
        if (col > 0 && wordSearchInput[row][col - 1] == 'M')
        {
            xMasCount += IsMas(wordSearchInput,row,col,0,-1);
        }
        if (col < maxCol && wordSearchInput[row][col + 1] == 'M')
        {
            xMasCount += IsMas(wordSearchInput,row,col,0,1);
        }
        if (row < maxRow && col > 0 && wordSearchInput[row + 1][col - 1] == 'M')
        {
            xMasCount += IsMas(wordSearchInput,row,col,1,-1);
        }
        if (row < maxRow && wordSearchInput[row + 1][col] == 'M')
        {
            xMasCount += IsMas(wordSearchInput,row,col,1,0);
        }
        if (row < maxRow && col < maxCol - 1 && wordSearchInput[row + 1][col + 1] == 'M')
        {
            xMasCount += IsMas(wordSearchInput,row,col,1,1);
        }
        
        return xMasCount;
    }

    private static int IsMas(string[] wordSearchInput, int xRow, int xCol, int rowDir, int colDir)
    {
        int sRow = xRow + (rowDir * 3);
        int sCol = xCol + (colDir * 3);

        // Check boundaries
        if( sRow < 0 || sRow >= wordSearchInput.Length) { return 0; }
        if( sCol < 0 || sCol >= wordSearchInput[xRow].Length) { return 0; }
        
        int aRow = xRow + (rowDir * 2);
        int aCol = xCol + (colDir * 2);

        if (wordSearchInput[aRow][aCol] == 'A' && wordSearchInput[sRow][sCol] == 'S') { return 1; };
        
        return 0;
    }
}