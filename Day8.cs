using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day8
{
    public static void Run()
    {
        Console.WriteLine("Day 8 Part One");

        string[] map = PuzzleData.GetDay8Map();
        
        var antennaList = new Dictionary<char, List<(int row, int col)>>();
        var antinodes = new HashSet<(int row, int col)>();
        var maxRow = map.Length;
        var maxCol = map[0].Length;
        
        for (int row = 0; row < maxRow; row++)
        {
            for (int col = 0; col < maxCol; col++)
            {
                if (map[row][col] == '.') continue;
                
                if (!antennaList.ContainsKey(map[row][col]))
                    antennaList.Add(map[row][col], new List<(int row, int col)>());
                
                antennaList[map[row][col]].Add((row, col));
            }
        }
        
        foreach (var antennaType in antennaList)
        {
            if (antennaType.Value.Count < 2) continue;

            for (int i = 0; i < antennaType.Value.Count; i++)
            {
                var firstPos = antennaType.Value[i];
                for (int j = i + 1; j < antennaType.Value.Count; j++)
                {
                    var secondPos = antennaType.Value[j];
                    (int row, int col) diff = (firstPos.row - secondPos.row, firstPos.col - secondPos.col);
                    
                    (int row, int col) firstAntinode = (firstPos.row + diff.row, firstPos.col + diff.col);
                    if(firstAntinode.row >= 0 && firstAntinode.row < maxRow && firstAntinode.col >= 0 && firstAntinode.col < maxCol)
                        antinodes.Add(firstAntinode);
                    
                    (int row, int col) secondAntinode = (secondPos.row - diff.row, secondPos.col - diff.col);
                    if(secondAntinode.row >= 0 && secondAntinode.row < maxRow && secondAntinode.col >= 0 && secondAntinode.col < maxCol)
                        antinodes.Add(secondAntinode);  
                }
            }
        }
        
        Console.WriteLine(antinodes.Count);
        Console.WriteLine();
        Console.WriteLine("Day 8 Part Two");
        
        antinodes = new HashSet<(int row, int col)>();
        
        foreach (var antennaType in antennaList)
        {
            if (antennaType.Value.Count < 2) continue;

            for (int i = 0; i < antennaType.Value.Count; i++)
            {
                var firstPos = antennaType.Value[i];
                antinodes.Add(firstPos);
                
                for (int j = i + 1; j < antennaType.Value.Count; j++)
                {
                    var secondPos = antennaType.Value[j];
                    antinodes.Add(secondPos);
                    
                    var newAntinodes = GetHarmonics(firstPos, secondPos, maxRow, maxCol);
                    
                    foreach (var newAntinode in newAntinodes)
                        antinodes.Add(newAntinode);
                }
            }
        }
        Console.WriteLine(antinodes.Count);
    }

    private static List<(int row, int col)> GetHarmonics((int row, int col) firstPos, (int row, int col) secondPos,
        int maxRow, int maxCol)
    {
        var result = new List<(int row, int col)>();

        (int row, int col) diff = (firstPos.row - secondPos.row, firstPos.col - secondPos.col);
        
        (int row, int col) nextAntinode = (firstPos.row + diff.row, firstPos.col + diff.col);
        while (nextAntinode.row >= 0 && nextAntinode.row < maxRow && nextAntinode.col >= 0 && nextAntinode.col < maxCol)
        {
            result.Add(nextAntinode);
            nextAntinode = (nextAntinode.row  + diff.row, nextAntinode.col + diff.col);
        }
        
        nextAntinode = (secondPos.row - diff.row, secondPos.col - diff.col);
        while (nextAntinode.row >= 0 && nextAntinode.row < maxRow && nextAntinode.col >= 0 && nextAntinode.col < maxCol)
        {
            result.Add(nextAntinode);
            nextAntinode = (nextAntinode.row  - diff.row, nextAntinode.col - diff.col);
        }
        
        return result;
    }
}