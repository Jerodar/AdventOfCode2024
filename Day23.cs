using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day23
{
    public static void Run()
    {
        Console.WriteLine("Day 23 Part One");

        List<(string, string)> input = PuzzleData.GetDay23Connections();
        
        Dictionary<string, List<string>> connections = new();

        foreach (var newConnection in input)
        {
            if(connections.ContainsKey(newConnection.Item1))
                connections[newConnection.Item1].Add(newConnection.Item2);
            else
                connections.Add(newConnection.Item1, new List<string> { newConnection.Item2 });
            
            if(connections.ContainsKey(newConnection.Item2))
                connections[newConnection.Item2].Add(newConnection.Item1);
            else
                connections.Add(newConnection.Item2, new List<string> { newConnection.Item1 });
        }

        List<(string, string, string)> networks = new();
        foreach (var connection in connections)
        {
            foreach (var computer1 in connection.Value)
            {
                foreach (var computer2 in connections[computer1])
                {
                    if (connection.Key[0] != 't' && computer1[0] != 't' && computer2[0] != 't')
                        continue;
                    
                    if (connections[computer2].Contains(connection.Key) &&
                        !networks.Contains((computer1, computer2, connection.Key)) &&
                        !networks.Contains((computer2, computer1, connection.Key)) &&
                        !networks.Contains((computer1, connection.Key, computer2)) &&
                        !networks.Contains((computer2, connection.Key, computer1)) &&
                        !networks.Contains((connection.Key, computer2, computer1)))
                        networks.Add((connection.Key, computer1, computer2));
                }
            }
        }
        
        foreach (var network in networks)
            Console.WriteLine(network);
        Console.WriteLine(networks.Count);
    }
}