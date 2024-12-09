using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day9
{
    public static void Run()
    {
        Console.WriteLine("Day 9 Part One");

        string driveMap = PuzzleData.GetDay9DriveMap();
        
        int diskSize = driveMap.Sum(c => Convert.ToInt32(c.ToString()));
        
        int[] resultMap = GetExpandedDriveMap(driveMap, diskSize);
        
        //PrintDriveMap(resultMap);
        
        resultMap = CompressDriveMap(resultMap);
        
        //PrintDriveMap(resultMap);
        
        long checkSum = CalculateCheckSum(resultMap);
        
        Console.WriteLine();
        Console.WriteLine(checkSum);
    }

    private static int[] GetExpandedDriveMap(string driveMap, int diskSize)
    {
        int[] resultMap = new int[diskSize];
        int fileId = 0;
        int nextPos = 0;
        bool isFile = true;
        
        foreach (char c in driveMap)
        {
            int fileSize = Convert.ToInt32(c.ToString());
            var result = -1;
            
            if (isFile)
            {
                result = fileId;
                fileId++;
            }

            var endOfFile = fileSize + nextPos;
            for (;nextPos < endOfFile; nextPos++)
                resultMap[nextPos] = result;
            
            isFile = !isFile;
        }

        return resultMap;
    }
    
    private static int[] CompressDriveMap(int[] resultMap)
    {
        int backIndex = resultMap.Length - 1;
        for (int i = 0; i < backIndex; i++)
        {
            if (resultMap[i] > -1) continue;
            
            while(resultMap[backIndex] == -1)
                backIndex--;
                
            resultMap[i] = resultMap[backIndex];
            resultMap[backIndex] = -1;
            backIndex--;
        }
        return resultMap;
    }
    
    private static long CalculateCheckSum(int[] resultMap)
    {
        long checkSum = 0;
        for (int i = 0; i < resultMap.Length; i++)
        {
            if (resultMap[i] == -1) continue;
            checkSum += (resultMap[i] * i);
        }
        return checkSum;
    }

    private static void PrintDriveMap(int[] driveMap)
    {
        string logMap = "";
        foreach (int i in driveMap)
        {
            if (i != -1)
            {
                logMap += i;
            }
            else
            {
                logMap += ".";
            }
        }
        Console.WriteLine();
        Console.WriteLine(logMap);
    }
}