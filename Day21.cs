using AdventOfCode2024Input;

namespace AdventOfCode2024;

public class Day21
{
    private const int NumPadDir = 25;
    private readonly Dictionary<(char, char, int),long> _cache = new();
    
    public void Run()
    {
        Console.WriteLine("Day 21 Part One");

        string[] codes = PuzzleData.GetDay21Codes();
        
        var robotNum = 'A';
        var robotDir1 = 'A';
        var robotDir2 = 'A';
        long totalComplexity = 0;
        foreach (string code in codes)
        {
            Console.WriteLine($"calculating code {code}");
            var result = "";
            foreach (char c in code)
            {
                var dir1Code = GoToNextNumber(robotNum, c);
                robotNum = c;
                
                var newResult = "";
                foreach (char c2 in dir1Code)
                {
                    var dir2Code = GoToNextDir(robotDir1, c2);
                    robotDir1 = c2;

                    foreach (char c3 in dir2Code)
                    {
                        var dir3Code = GoToNextDir(robotDir2, c3);
                        robotDir2 = c3;

                        newResult += dir3Code;
                    }
                }
                result += newResult;
            }
            Console.WriteLine(result);
            int numeric = int.Parse(code[..^1]);
            int complexity = numeric * result.Length;
            Console.WriteLine($"{result.Length} * {numeric}");
            Console.WriteLine();
            totalComplexity += complexity;
        }
        Console.WriteLine($"Total complexity: {totalComplexity}");
        Console.WriteLine();
        Console.WriteLine("Day 21 Part Two");
        
        totalComplexity = 0;
        foreach (string code in codes)
        {
            Console.WriteLine($"calculating code {code}");
            long cost = 0;
            var prevChar = 'A';
            foreach (char c in code)
            {
                var subResult = GoToNextNumber(prevChar, c);
                prevChar = c;

                cost += GetCost(subResult, NumPadDir);
            }
            int numeric = int.Parse(code[..^1]);
            long complexity = numeric * cost;
            totalComplexity += complexity;
            
            Console.WriteLine($"{cost} * {numeric}");
            Console.WriteLine();
        }
        Console.WriteLine($"Total complexity: {totalComplexity}");
    }

    private long GetCost(string code, int numpadCount)
    {
        if (numpadCount == 0)
        {
            return code.Length;
        }
        
        long cost = 0;
        var prevChar = 'A';
        foreach (char c in code)
        {
            if (!_cache.ContainsKey((prevChar, c, numpadCount)))
            {
                var nextCode = GoToNextDir(prevChar, c);
                var newCost = GetCost(nextCode, numpadCount-1);
                _cache.Add((prevChar, c, numpadCount), newCost);
                cost += newCost;
            }
            else
            {
                cost += _cache[(prevChar, c, numpadCount)];
            }
            prevChar = c;
        }
        return cost;
    }
    
    private static string GoToNextDir(char current, char next)
    {
        return (current, next) switch
        {
            ('^', '^') => "A",
            ('^', '<') => "v<A",
            ('^', 'v') => "vA",
            ('^', '>') => "v>A",
            ('^', 'A') => ">A",
            
            ('<', '^') => ">^A",
            ('<', '<') => "A",
            ('<', 'v') => ">A",
            ('<', '>') => ">>A",
            ('<', 'A') => ">>^A",
            
            ('v', '^') => "^A",
            ('v', '<') => "<A",
            ('v', 'v') => "A",
            ('v', '>') => ">A",
            ('v', 'A') => "^>A",
            
            ('>', '^') => "<^A",
            ('>', '<') => "<<A",
            ('>', 'v') => "<A",
            ('>', '>') => "A",
            ('>', 'A') => "^A",
            
            ('A', '^') => "<A",
            ('A', '<') => "v<<A",
            ('A', 'v') => "<vA",
            ('A', '>') => "vA",
            ('A', 'A') => "A",

            _ => ""
        };
    }
    
    private static string GoToNextNumber(char current, char next)
    {
        return (current, next) switch
        {
            ('0', '0') => "A",
            ('0', '1') => "^<A",
            ('0', '2') => "^A",
            ('0', '3') => "^>A",
            ('0', '4') => "^^<A",
            ('0', '5') => "^^A",
            ('0', '6') => "^^>A",
            ('0', '7') => "^^^<A",
            ('0', '8') => "^^^A",
            ('0', '9') => "^^^>A",
            ('0', 'A') => ">A",
            
            ('1', '0') => ">vA",
            ('1', '1') => "A",
            ('1', '2') => ">A",
            ('1', '3') => ">>A",
            ('1', '4') => "^A",
            ('1', '5') => "^>A",
            ('1', '6') => "^>>A",
            ('1', '7') => "^^A",
            ('1', '8') => "^^>A",
            ('1', '9') => "^^>>A",
            ('1', 'A') => ">>vA",
            
            ('2', '0') => "vA",
            ('2', '1') => "<A",
            ('2', '2') => "A",
            ('2', '3') => ">A",
            ('2', '4') => "<^A",
            ('2', '5') => "^A",
            ('2', '6') => "^>A",
            ('2', '7') => "<^^A",
            ('2', '8') => "^^A",
            ('2', '9') => "^^>A",
            ('2', 'A') => "v>A",
            
            ('3', '0') => "<vA",
            ('3', '1') => "<<A",
            ('3', '2') => "<A",
            ('3', '3') => "A",
            ('3', '4') => "<<^A",
            ('3', '5') => "<^A",
            ('3', '6') => "^A",
            ('3', '7') => "<<^^A",
            ('3', '8') => "<^^A",
            ('3', '9') => "^^A",
            ('3', 'A') => "vA",
            
            ('4', '0') => ">vvA",
            ('4', '1') => "vA",
            ('4', '2') => "v>A",
            ('4', '3') => "v>>A",
            ('4', '4') => "A",
            ('4', '5') => ">A",
            ('4', '6') => ">>A",
            ('4', '7') => "^A",
            ('4', '8') => "^>A",
            ('4', '9') => "^>>A",
            ('4', 'A') => ">>vvA",
            
            ('5', '0') => "vvA",
            ('5', '1') => "<vA",
            ('5', '2') => "vA",
            ('5', '3') => "v>A",
            ('5', '4') => "<A",
            ('5', '5') => "<A",
            ('5', '6') => ">A",
            ('5', '7') => "<^A",
            ('5', '8') => "^A",
            ('5', '9') => "^>A",
            ('5', 'A') => "vv>A",
            
            ('6', '0') => "<vvA",
            ('6', '1') => "<<vA",
            ('6', '2') => "<vA",
            ('6', '3') => "vA",
            ('6', '4') => "<<A",
            ('6', '5') => "<A",
            ('6', '6') => "A",
            ('6', '7') => "<<^A",
            ('6', '8') => "<^A",
            ('6', '9') => "^A",
            ('6', 'A') => "vvA",
            
            ('7', '0') => ">vvvA",
            ('7', '1') => "vvA",
            ('7', '2') => "vv>A",
            ('7', '3') => "vv>>A",
            ('7', '4') => "vA",
            ('7', '5') => "v>A",
            ('7', '6') => "v>>A",
            ('7', '7') => "A",
            ('7', '8') => ">A",
            ('7', '9') => ">>A",
            ('7', 'A') => ">>vvvA",
            
            ('8', '0') => "vvvA",
            ('8', '1') => "<vvA",
            ('8', '2') => "vvA",
            ('8', '3') => "vv>A",
            ('8', '4') => "<vA",
            ('8', '5') => "vA",
            ('8', '6') => "v>A",
            ('8', '7') => "<A",
            ('8', '8') => "A",
            ('8', '9') => ">A",
            ('8', 'A') => "vvv>A",
            
            ('9', '0') => "<vvvA",
            ('9', '1') => "<<vvA",
            ('9', '2') => "<vvA",
            ('9', '3') => "vvA",
            ('9', '4') => "<<vA",
            ('9', '5') => "<vA",
            ('9', '6') => "vA",
            ('9', '7') => "<<A",
            ('9', '8') => "<A",
            ('9', '9') => "A",
            ('9', 'A') => "vvvA",
            
            ('A', '0') => "<A",
            ('A', '1') => "^<<A",
            ('A', '2') => "<^A",
            ('A', '3') => "^A",
            ('A', '4') => "^^<<A",
            ('A', '5') => "<^^A",
            ('A', '6') => "^^A",
            ('A', '7') => "^^^<<A",
            ('A', '8') => "<^^^A",
            ('A', '9') => "^^^A",
            ('A', 'A') => "A",
            _ => ""
        };
    }
}