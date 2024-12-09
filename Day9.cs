using System.Runtime.Intrinsics.Arm;
using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day9
{
    private class File
    {
        public int ID = -1;
        public int size = -1;
        public int index = -1;
    }
    
    public static void Run()
    {
        string driveMap = PuzzleData.GetDay9DriveMap();
        
        Console.WriteLine("Day 9 Part One");

        int diskSize = driveMap.Sum(c => Convert.ToInt32(c.ToString()));
        int[] resultMap = GetExpandedDriveMap(driveMap, diskSize);
        resultMap = CompressDriveMap(resultMap);
        long checkSum = CalculateCheckSum(resultMap);
        Console.WriteLine(checkSum);
        
        Console.WriteLine();
        Console.WriteLine("Day 9 Part Two");
        
        List<File> files = GetFiles(driveMap);
        files = RearrangeFiles(files);
        checkSum = CalculateFilesCheckSum(files);
        Console.WriteLine(checkSum);
    }

    private static List<File> GetFiles(string driveMap)
    {
        int fileId = 0;
        int nextPos = 0;
        bool isFile = true;
        var results = new List<File>();
        
        foreach (char c in driveMap)
        {
            int fileSize = Convert.ToInt32(c.ToString());

            if (isFile)
            {
                results.Add(new File { ID = fileId, size = fileSize, index = nextPos });
                fileId++;
            }
            else
            {
                results.Add(new File { ID = -1, size = fileSize, index = nextPos });
            }

            nextPos += fileSize;
            isFile = !isFile;
        }

        return results;
    }
    
    private static List<File> RearrangeFiles(List<File> files)
    {
        for (int i = 0; i < files.Count; i++)
        {
            var file = files[i];
            if (file.ID > -1) continue;

            var insertIndex = file.index;
            var gapSize = file.size;
            while(gapSize > 0)
            {
                var fileToMove = (from target in files where target.ID > 0 && target.index > insertIndex && target.size <= gapSize orderby target.index descending select target).FirstOrDefault();
                if (fileToMove == null) break;
                fileToMove.index = insertIndex;
                insertIndex += fileToMove.size;
                gapSize -= fileToMove.size;
            }
        }
        
        return files;
    }
        
    private static long CalculateFilesCheckSum(List<File> files)
    {
        long checkSum = 0;
        foreach (File file in files)
        {
            if (file.ID == -1) continue;
            var lastIndex = file.index + file.size;
            for (int i = file.index; i < lastIndex; i++)
                checkSum += file.ID * i;
        }
        return checkSum;
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
}