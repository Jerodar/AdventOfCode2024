using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day18
{
    private class Node
    {
        public (int row, int col) Position { get; init; }
        public List<Connection> Connections { get; } = new();
        public int MinCostToStart { get; set; }
        public Node? NearestToStart { get; set; }
        public bool Visited { get; set; }

        public void AddConnection(Connection newConnection)
        {
            if (!(from connection in Connections where 
                    connection.ConnectedNode.Position == newConnection.ConnectedNode.Position 
                    select connection.ConnectedNode).Any())
                Connections.Add(newConnection);
        }
    }
    
    private class Connection
    {
        public Node ConnectedNode { get; init; } = new();
        public int Cost { get; init; } = 1;
    }

    private class Graph
    {
        public List<Node> Nodes { get; } = new();

        public Node? GetNode(int row, int col)
        {
            return (from node in Nodes where node.Position.row == row && node.Position.col == col select node).FirstOrDefault();
        }
    }

    public static void Run()
    {
        Console.WriteLine("Day 18 Part One");

        // be careful, coordinates are reversed for input!
        List<(int col, int row)> allBytes = PuzzleData.GetDay18Bytes();
        const int max = 71;
        const int byteCount = 1024;
        var bytes = allBytes.GetRange(0, byteCount);

        var graph = FillGraph(max, bytes);
        
        var start = graph.GetNode(0, 0) ?? new Node();
        var end = graph.GetNode(max-1, max-1) ?? new Node();
        
        DijkstraSearch(graph, start, end);
        
        var shortestPath = GetShortestPath(end);
        
        Console.WriteLine($"Shortest Path length: {shortestPath.Count}");
    }

    private static Graph FillGraph(int max, List<(int col, int row)> walls)
    {
        var graph = new Graph();
        for (int row = 0; row < max; row++)
        {
            for (int col = 0; col < max; col++)
            {
                if (walls.Contains((col, row))) continue;
                
                var node = new Node { Position = (row, col) };
                
                var leftNeighbour = graph.GetNode(row, col -1);
                if (leftNeighbour != null)
                {
                    node.AddConnection(new Connection() { ConnectedNode = leftNeighbour });
                    leftNeighbour.AddConnection(new Connection() { ConnectedNode = node });
                }
                
                var upNeighbour = graph.GetNode(row -1, col);
                if (upNeighbour != null)
                {
                    node.AddConnection(new Connection() { ConnectedNode = upNeighbour });
                    upNeighbour.AddConnection(new Connection() { ConnectedNode = node });
                }
                
                graph.Nodes.Add(node);
            }
        }

        return graph;
    }

    private static void DijkstraSearch(Graph graph, Node start, Node goal)
    {
        start.MinCostToStart = 0;
        var queue = new List<Node> { start };
        do
        {
            queue= queue.OrderBy(node => node.MinCostToStart).ToList();
            var node = queue.First();
            queue.Remove(node);
            foreach (var connection in node.Connections.OrderBy(connection => connection.Cost))
            {
                var childNode = connection.ConnectedNode;
                if (childNode.Visited) continue;

                if (childNode.MinCostToStart == 0 ||
                    node.MinCostToStart + connection.Cost < childNode.MinCostToStart)
                {
                    childNode.MinCostToStart = node.MinCostToStart + connection.Cost ;
                    childNode.NearestToStart = node;
                    if(!queue.Contains(childNode)) queue.Add(childNode);
                }
            }
            node.Visited = true;
            if (node.Position == goal.Position) return;
        } while (queue.Any());
    }

    private static List<Node> GetShortestPath(Node node)
    {
        List<Node> shortestPath = new(); 
        
        while (node.NearestToStart != null)
        {
            shortestPath.Add(node.NearestToStart);
            node = node.NearestToStart;
        }

        return shortestPath;
    }
}