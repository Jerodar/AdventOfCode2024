using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day13
{
    public static void Run()
    {
        Console.WriteLine("Day 13 Part One");

        List<((int ax, int ay),(int bx,int by), (int px,int py))> clawMachines = PuzzleData.GetDay13ClawMachines();

        long totalCost = 0;
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
        
        Console.WriteLine();
        Console.WriteLine("Day 13 Part Two");
        totalCost = 0;
        foreach (((int x, int y) a, (int x, int y) b, (int x, int y) p) clawMachine in clawMachines)
        {
            var px = clawMachine.p.x + 10000000000000;
            var py = clawMachine.p.y + 10000000000000;
            
            // instead of brute force use the magic of algebra
            //
            // Starting formula:
            // PX = AX*a + BX*b
            // PY = AY*a + BY*b
            //
            // Make factor for B equal in both formula:
            // PX * BY = AX*BY*a + BX*BY*b
            // PY * BX = AY*BX*a + BY*BX*b
            //
            // Subtract the second formula from the first
            // PX*BY - PY*BX = (AX*BY*a + BX*BY*b) - (AY*BX*a + BY*BX*b)
            //
            // Eliminate b and make single factor for a. That gives you the solution to a
            // PX*BY - PY*BX = AX*BY*a - AY*BX*a + BX*BY*b - BY*BX*b
            // PX*BY - PY*BX = AX*BY*a - AY*BX*a
            // PX*BY - PY*BX = (AX*BY - AY*BX)a
            //           RHS = ACOEFF*a
            //             a = RHS / ACOEFF
            //
            // These type of problems only ever have none, 1 or infinite solutions, so no need to check minimum of cost
            
            var aCoefficient = clawMachine.a.x * clawMachine.b.y - clawMachine.a.y * clawMachine.b.x;
            var rightHandSide = px * clawMachine.b.y - py * clawMachine.b.x;
            if (aCoefficient == 0) // if both buttons have the same slope, check which button is cheaper per unit moved
            {
                if (clawMachine.a.x > clawMachine.b.x * 3)
                    totalCost = (px / clawMachine.a.x) * 3;
                else
                    totalCost = (px / clawMachine.b.x);
            }
            else if (rightHandSide % aCoefficient == 0) // must be integer
            {
                var a = rightHandSide / aCoefficient;
                var b = (px - clawMachine.a.x * a) / clawMachine.b.x;
                totalCost += a * 3 + b;
            }
        }
        Console.WriteLine($"Total cost: {totalCost}");
    }
}