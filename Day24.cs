using AdventOfCode2024Input;
using Microsoft.VisualBasic;

namespace AdventOfCode2024;

public static class Day24
{
    public static void Run()
    {
        Console.WriteLine("Day 24 Part One");

        Dictionary<string, int> inputWires = PuzzleData.GetDay24Wires();
        List<(string input1,string type,string input2,string name)> gateStrings = PuzzleData.GetDay24Gates();
        
        List<Gate> gates = new List<Gate>();
        List<Gate> endGates = new List<Gate>();
        List<Gate> inGates = new List<Gate>();

        foreach (var wire in inputWires)
        {
            var gate = new Gate(wire.Key, "");
            gate.SetValue(wire.Value == 1);
            gates.Add(gate);
            inGates.Add(gate);
        }

        foreach (var gateString in gateStrings)
        {
            var gate = new Gate(gateString.name, gateString.type);
            gates.Add(gate);
            if (gateString.name[0] == 'z')
                endGates.Add(gate);
        }
        
        foreach (var gateString in gateStrings)
        {
            var gate = (from g in gates where g.Name == gateString.name select g).First();
            var input1 = (from g in gates where g.Name == gateString.input1 select g).First();
            var input2 = (from g in gates where g.Name == gateString.input2 select g).First();
            gate.Connect(input1, input2);
        }
        
        var decimalOutput = GetOutput(endGates);
        Console.WriteLine();
        Console.WriteLine($"Total: {decimalOutput}");
        
        Console.WriteLine();
        Console.WriteLine("Day 24 Part Two");

        endGates.Clear();
        var swap1a = (from gate in gates where gate.Name == "z18" select gate).First();
        var swap1b = (from gate in gates where gate.Name == "fvw" select gate).First();
        var swap1aName = swap1a.Name;
        var swap1bName = swap1b.Name;
        var swap2a = (from gate in gates where gate.Name == "z22" select gate).First();
        var swap2b = (from gate in gates where gate.Name == "mdb" select gate).First();
        var swap2aName = swap2a.Name;
        var swap2bName = swap2b.Name;
        var swap3a = (from gate in gates where gate.Name == "z36" select gate).First();
        var swap3b = (from gate in gates where gate.Name == "nwq" select gate).First();
        var swap3aName = swap3a.Name;
        var swap3bName = swap3b.Name;
        var swap4a = (from gate in gates where gate.Name == "wpq" select gate).First();
        var swap4b = (from gate in gates where gate.Name == "grf" select gate).First();
        var swap4aName = swap4a.Name;
        var swap4bName = swap4b.Name;
        
        endGates.Clear();
        foreach (var gate in gates)
        {
            gate.Reset();
            gate.Swap(swap1a, swap1b, swap1aName, swap1bName);
            gate.Swap(swap2a, swap2b, swap2aName, swap2bName);
            gate.Swap(swap3a, swap3b, swap3aName, swap3bName);
            gate.Swap(swap4a, swap4b, swap4aName, swap4bName);
            if (gate.Name[0] == 'z')
                endGates.Add(gate);
        }
        
        for (long x = 235126757145; x < 23512675714500; x++)
        for (long y = 135126757145; y < 13512675714500; y++)
        {
            foreach (var gate in gates)
            {
                gate.Reset();
            }
            
            SetInputs(x,y,inGates);
            decimalOutput = GetOutput(endGates);
            
            if (decimalOutput != x + y)
            {
                Console.WriteLine($"          4         3         2         1");
                Console.WriteLine($"     5432109876543210987654321098765432109876543210");
                Console.WriteLine($"in1: 00000000{Convert.ToString(x, 2)}");
                Console.WriteLine($"in2: 000000000{Convert.ToString(y, 2)}");
                Console.WriteLine($"exp: 0000000{Convert.ToString(x+y, 2)}");
                Console.WriteLine($"out: 0000000{Convert.ToString(decimalOutput, 2)}");
                Console.WriteLine();
                
                (from gate in gates where gate.Name == "z05" select gate).First().Print(3);
                return;
            }
        }
    }

    private static (long,long) GetInputs(List<Gate> inGates)
    {
        long xInput = 0;
        long yInput = 0;
        foreach (var gate in inGates.OrderByDescending(g => g.Name))
        {
            if (gate.Name[0] == 'x')
            {
                xInput = xInput << 1;
                if (gate.GetValue()) xInput++;
            }
            else
            {
                yInput = yInput << 1;
                if (gate.GetValue()) yInput++;
            }
        }
        return (xInput, yInput);
    }

    private static void SetInputs(long x, long y, List<Gate> inGates)
    {
        string xString = Convert.ToString(x, 2);
        string yString = Convert.ToString(y, 2);
        foreach (var gate in inGates)
        {
            var bit = int.Parse(gate.Name[1..]);
            if (gate.Name[0] == 'x')
            {
                if (xString.Length <= bit) gate.SetValue(false);
                else gate.SetValue(xString[^(bit + 1)]=='1');
            }

            if (gate.Name[0] == 'y')
            {
                if (yString.Length <= bit) gate.SetValue(false);
                else gate.SetValue(yString[^(bit + 1)]=='1');
            }
        }
    }


    private static long GetOutput(List<Gate> endGates)
    {
        long decimalOutput = 0;
        foreach (var gate in endGates.OrderByDescending(g => g.Name))
        {
            decimalOutput = decimalOutput << 1;
            if (gate.GetValue()) decimalOutput ++;
        }

        return decimalOutput;
    }

    private enum GateTypes
    {
        Unknown,
        And,
        Or,
        Xor
    }
    
    private class Gate
    {
        public string Name { get; private set; }

        private readonly string _originalName;
        private readonly GateTypes _type;
        private bool? _value;
        private Gate? _input1;
        private Gate? _input2;
        private Gate? _originalInput1;
        private Gate? _originalInput2;

        public Gate(string name, string type)
        {
            Name = name;
            _originalName = name;
            _type = type switch
            {
                "AND" => GateTypes.And,
                "OR" => GateTypes.Or,
                "XOR" => GateTypes.Xor,
                _ => GateTypes.Unknown
            };
        }

        public void SetValue(bool value)
        {
            _value = value;
        }

        public bool GetValue()
        {
            if (_value != null) return _value.GetValueOrDefault();

            if (_input1 == null || _input2 == null)
            {
                throw new Exception($"Gate {Name} is missing a connection!");
            }

            _value = _type switch
            {
                GateTypes.And => _input1.GetValue() && _input2.GetValue(),
                GateTypes.Or => _input1.GetValue() || _input2.GetValue(),
                GateTypes.Xor => _input1.GetValue() ^ _input2.GetValue(),
                _ => false
            };

            return _value.GetValueOrDefault();
        }

        public void Connect(Gate input1, Gate input2)
        {
            _input1 = input1;
            _originalInput1 = input1;
            _input2 = input2;
            _originalInput2 = input2;
        }

        public void Swap(Gate original, Gate swapped, string originalName, string swappedName)
        {
            if (Name == originalName) Name = swappedName;
            else if (Name == swappedName) Name = originalName;
            if (_input1 == original) _input1 = swapped;
            else if (_input1 == swapped) _input1 = original;
            if (_input2 == original) _input2 = swapped;
            else if (_input2 == swapped) _input2 = original;
        }

        public void SwapReset()
        {
            Name = _originalName;
            if (_type != GateTypes.Unknown) _value = null;
            _input1 = _originalInput1;
            _input2 = _originalInput2;
        }
        
        public void Reset()
        {
            if (_type != GateTypes.Unknown) _value = null;
        }

        public void Print(int depth)
        {
            if (depth < 0) return;
            depth--;
            Console.WriteLine($"{_input1?.Name} {_type} {_input2?.Name} - {Name} - {_value}");
            _input1?.Print(depth);
            _input2?.Print(depth);
        }
    }
}