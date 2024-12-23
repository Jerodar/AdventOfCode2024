using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day23
{
    public static void Run()
    {
        Console.WriteLine("Day 23 Part One");
        
        List<(string, string)> input = PuzzleData.GetDay23Connections();
        
        Dictionary<int, List<int>> connections = Parse(input);
        
        List<HashSet<int>> networks = FindNetworkOfThree(connections);
        
        var tNetworks = (from network in networks where (from connection in network where IsFirstT(connection) select connection).Any() select network).ToList();
        Console.WriteLine(tNetworks.Count);
        
        Console.WriteLine();
        Console.WriteLine("Day 23 Part Two");
        
        var largestNetwork = FindLargestNetwork(connections, networks);

        var sortedNetwork = largestNetwork.ToList();
        sortedNetwork.Sort();
        foreach (var computer in sortedNetwork)
            Console.Write($"{IntToPc(computer)},");
        Console.WriteLine($" total size: {sortedNetwork.Count}");
    }
    
    private static HashSet<int> FindLargestNetwork(Dictionary<int, List<int>> connections, List<HashSet<int>> networks)
    {
        foreach (var connection in connections)
        {
            foreach (var network in networks)
            {
                if (!network.Contains(connection.Key)) continue;
                
                foreach (var computer in connection.Value)
                {
                    var canAdd = true;
                    foreach (var currentComputer in network)
                        if (!connections[computer].Contains(currentComputer))
                            canAdd = false;
                
                    if (canAdd) network.Add(computer);
                }
            }
        }

        return networks.MaxBy(x => x.Count) ?? new HashSet<int>();
    }

    private static List<HashSet<int>> FindNetworkOfThree(Dictionary<int, List<int>> connections)
    {
        List<HashSet<int>> tNetworks = new();
        
        foreach (var connection in connections)
        {
            foreach (var computer1 in connection.Value)
            {
                if (connection.Key > computer1) continue;
                
                foreach (var computer2 in connections[computer1])
                {
                    if (computer1 > computer2) continue;

                    if (!connections[computer2].Contains(connection.Key)) continue;
                    
                    tNetworks.Add(new HashSet<int> {connection.Key, computer1, computer2});
                }
            }
        }
        
        return tNetworks;
    }

    private static Dictionary<int, List<int>> Parse(List<(string, string)> input)
    {
        Dictionary<int, List<int>> connections = new();

        foreach (var newConnection in input)
        {
            var hash1 = PcToInt(newConnection.Item1);
            var hash2 = PcToInt(newConnection.Item2);
            
            if(!connections.ContainsKey(hash1))
                connections.Add(hash1, new List<int>());
            connections[hash1].Add(hash2);
            
            if(!connections.ContainsKey(hash2))
                connections.Add(hash2, new List<int>());
            connections[hash2].Add(hash1);
        }

        return connections;
    }

    private static int PcToInt(string input)
    {
        return (input[0]-'a')*26 + (input[1]-'a');
    }

    private static string IntToPc(int input)
    {
        var firstChar = (char)(input / 26 +'a');
        var secondChar = (char)(input % 26 + 'a');
        return firstChar.ToString() + secondChar;
    }

    private static bool IsFirstT(int input)
    {
        return input / 26 == 't' - 'a';
    }
}