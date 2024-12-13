using System.Xml.Xsl;
using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day13
{
    public static void Run()
    {
        Console.WriteLine("Day 12 Part One");

        List<((int ax, int ay),(int bx,int by), (int px,int py))> clawMachines = PuzzleData.GetDay13ClawMachines();

        var totalCost = 0;
        foreach (((int x, int y) a,(int x,int y) b, (int x,int y) p) clawMachine in clawMachines)
        {
            int cost = 0;
            for (int a = 0; a <= 100; a++)
            {
                for (int b = 0; b <= 100; b++)
                {
                    if (a * clawMachine.a.x + b * clawMachine.b.x == clawMachine.p.x &&
                        a * clawMachine.a.y + b * clawMachine.b.y == clawMachine.p.y)
                    {
                        var newCost = a * 3 + b;
                        if (cost == 0 || newCost < cost)
                            cost = newCost;
                    }
                }
            }
            totalCost += cost;
        }
        Console.WriteLine($"Total cost: {totalCost}");
    }
}