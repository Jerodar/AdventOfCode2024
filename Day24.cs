using AdventOfCode2024Input;

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

        foreach (var wire in inputWires)
        {
            var gate = new Gate(wire.Key, "");
            gate.SetValue(wire.Value == 1);
            gates.Add(gate);
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
        
        long decimalOutput = 0;
        foreach (var gate in endGates.OrderByDescending(g => g.Name))
        {
            var result = gate.GetValue();
            Console.Write($"{gate.Name}:{result} ");
            decimalOutput = decimalOutput << 1;
            if (result) decimalOutput ++;
        }
        Console.WriteLine();
        Console.WriteLine($"Total: {decimalOutput}");
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
        public string Name { get; init; }

        private readonly GateTypes _type;
        private bool? _value;
        private Gate? _input1;
        private Gate? _input2;

        public Gate(string name, string type)
        {
            Name = name;
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
            _input2 = input2;
        }
    }
}