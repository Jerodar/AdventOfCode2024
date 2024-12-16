using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day16
{
    private class Node
    {
        public (int row, int col) Position { get; init; }
        public List<Connection> Connections { get; } = new();
        public int MinCostToStart { get; set; }
        public List<Connection> NearestToStart { get; set; } = new();
        public bool Visited { get; set; }

        public void AddConnection(Connection newConnection)
        {
            if (!(from connection in Connections where 
                    connection.ConnectedNode.Position == newConnection.ConnectedNode.Position 
                    select connection.ConnectedNode).Any())
                Connections.Add(newConnection);
        }

        public void AddNearestToStart(Connection newConnection)
        {
            if (!(from connection in NearestToStart where 
                        connection.ConnectedNode.Position == newConnection.ConnectedNode.Position 
                    select connection.ConnectedNode).Any())
                NearestToStart.Add(newConnection);
        }
    }
    
    private class Connection
    {
        public Node ConnectedNode { get; init; } = new();
        public int Cost { get; init; }
        public bool IsHorizontal { get; init; }
    }

    private class Graph
    {
        public List<Node> Nodes { get; } = new();

        public Node? GetNode(int row, int col)
        {
            return (from node in Nodes where node.Position.row == row && node.Position.col == col select node).FirstOrDefault();
        }
        
        public void CreateStraightConnections()
        {
            foreach (var node in Nodes)
            {
                var horizontalConnections = node.Connections.Where(c => c.IsHorizontal).ToList();
                if (horizontalConnections.Count == 2)
                {
                    var newCost = horizontalConnections[0].Cost + horizontalConnections[1].Cost;
                    var firstNode = horizontalConnections[0].ConnectedNode;
                    var secondNode = horizontalConnections[1].ConnectedNode;
                    firstNode.Connections.Add(new Connection() { ConnectedNode = secondNode, IsHorizontal = true, Cost = newCost});
                    secondNode.Connections.Add(new Connection() { ConnectedNode = firstNode, IsHorizontal = true, Cost = newCost});
                }
                var verticalConnections = node.Connections.Where(c => !c.IsHorizontal).ToList();
                if (verticalConnections.Count == 2)
                {
                    var newCost = verticalConnections[0].Cost + verticalConnections[1].Cost;
                    var firstNode = verticalConnections[0].ConnectedNode;
                    var secondNode = verticalConnections[1].ConnectedNode;
                    firstNode.Connections.Add(new Connection() { ConnectedNode = secondNode, IsHorizontal = false, Cost = newCost});
                    secondNode.Connections.Add(new Connection() { ConnectedNode = firstNode, IsHorizontal = false, Cost = newCost});
                }
            }
        }
    }

    public static void Run()
    {
        Console.WriteLine("Day 16 Part One");

        string[] map = PuzzleData.GetDay16Map();
        (int row, int col) start = (0, 0);
        (int row, int col) goal = (0, 0);

        for (int row = 0; row < map.Length; row++)
        {
            for (int col = 0; col < map[row].Length; col++)
            {
                if (map[row][col] == 'S') start = (row, col);
                if (map[row][col] == 'E') goal = (row, col);
            }
        }

        var graph = new Graph();
        FillGraph(graph, map, start.row, start.col);
        graph.CreateStraightConnections();
        DijkstraSearch(graph, start.row, start.col, goal.row, goal.col);
        Node? endNodeMaybe =  graph.GetNode(goal.row, goal.col);
        Node endNode = endNodeMaybe ?? new Node();
        Console.WriteLine($"min cost is {endNode.MinCostToStart}");
        
        Console.WriteLine();
        Console.WriteLine("Day 16 Part Two");
        
        var traveledPaths = new HashSet<(int row, int col)>();
        var traveledConnections = new HashSet<(int arow, int acol,int brow, int bcol)>();
        var winningConnections = (from connection in endNode.NearestToStart where connection.Cost == endNode.NearestToStart.Min(c => c.Cost) select connection).ToList();
        GetVisitedNodes(endNode, traveledConnections);
        GetPathCount(traveledConnections, traveledPaths);
        PrintMap(map, traveledPaths);
        
        Console.WriteLine($"traveled paths count: {traveledPaths.Count}");
    }

    private static void FillGraph(Graph graph, string[] map, int startRow, int startCol)
    {
        if (graph.GetNode(startRow, startCol) != null) return;
        
        var currentNode = new Node() { Position = (startRow, startCol) };
        graph.Nodes.Add(currentNode);
        
        if (map[startRow + 1][startCol] == '.')
        {
            FindNeighbours(graph, map, startRow, startCol, currentNode, 1,0,false);
        }
        if (map[startRow - 1][startCol] == '.')
        {
            FindNeighbours(graph, map, startRow, startCol, currentNode, -1,0,false);
        }
        if (map[startRow ][startCol + 1] == '.')
        {
            FindNeighbours(graph, map, startRow, startCol, currentNode, 0,1,true);
        }
        if (map[startRow ][startCol - 1] == '.')
        {
            FindNeighbours(graph, map, startRow, startCol, currentNode, 0,-1,true);
        }
    }

    private static void FindNeighbours(Graph graph, string[] map, int startRow, int startCol, Node currentNode, int rowDir, int colDir, bool isHorizontal)
    {
        (int row, int col, int cost) nextCrossroad = FindNextNode(map, startRow + rowDir, startCol + colDir, rowDir, colDir);
        if (nextCrossroad.cost < 0) return;
        
        var nextNode = graph.GetNode(nextCrossroad.row, nextCrossroad.col);
        if (nextNode != null)
        {
            currentNode.AddConnection(new Connection() { ConnectedNode = nextNode, Cost = nextCrossroad.cost, IsHorizontal = isHorizontal });
            nextNode.AddConnection(new Connection() { ConnectedNode = currentNode, Cost = nextCrossroad.cost, IsHorizontal = isHorizontal });
        }
        FillGraph(graph, map, nextCrossroad.row, nextCrossroad.col);
    }

    private static (int row, int col, int cost) FindNextNode(string[] map, int startRow, int startCol, int rowDir, int colDir)
    {
        int cost = 1;
        while (true)
        {
            var left = map[startRow + colDir * -1][startCol + rowDir];
            if (left == '.')
                return (startRow, startCol, cost);

            var right = map[startRow + colDir][startCol + rowDir * -1];
            if (right == '.')
                return (startRow, startCol, cost);

            var front = map[startRow + rowDir][startCol + colDir];
            if (front == 'E' || front == 'S')
                return (startRow + rowDir, startCol + colDir, cost + 1);
            
            if (front == '#' && right == '#' && left == '#')
                return (startRow, startCol, -1);

            startRow += rowDir;
            startCol += colDir;
            cost++;
        }
    }
    
    private static void DijkstraSearch(Graph graph, int startRow, int startCol, int endRow, int endCol)
    {
        var priorityQueue = new List<Node>();
        var startNode = graph.GetNode(startRow, startCol);
        if (startNode == null) return;
        startNode.MinCostToStart = 0;
        startNode.NearestToStart.Add(new Connection { ConnectedNode = startNode, IsHorizontal = true });
        priorityQueue.Add(startNode);
        
        do
        {
            var node = priorityQueue.OrderBy(node => node.MinCostToStart).First();
            priorityQueue.Remove(node);
            foreach (var previousConnection in node.NearestToStart)
            {
                var isHorizontal = previousConnection.IsHorizontal;
                foreach (var connection in node.Connections.OrderBy(connection => (connection.Cost + (isHorizontal == connection.IsHorizontal ? 0 : 1000))))
                {
                    var childNode = connection.ConnectedNode;
                    if (childNode.Visited) continue;

                    var newCost = node.MinCostToStart + connection.Cost + (isHorizontal == connection.IsHorizontal ? 0 : 1000);
                    
                    if (childNode.NearestToStart.Count > 0 && newCost == childNode.MinCostToStart)
                    {
                        childNode.AddNearestToStart(new Connection { ConnectedNode = node, IsHorizontal = connection.IsHorizontal });
                        if (!priorityQueue.Contains(childNode))
                            priorityQueue.Add(childNode);
                    }
                    else if (childNode.NearestToStart.Count == 0 || newCost < childNode.MinCostToStart)
                    {
                        childNode.NearestToStart = new List<Connection>()
                            { new() { ConnectedNode = node, IsHorizontal = connection.IsHorizontal } };
                        
                        childNode.MinCostToStart = newCost;
                        if (!priorityQueue.Contains(childNode))
                            priorityQueue.Add(childNode);
                    }
                } 
            }
            node.Visited = true;
            if (node.Position.row == endRow && node.Position.col == endCol)
                return;
        } while (priorityQueue.Any());
    }


    private static void GetVisitedNodes(Node node, HashSet<(int arow, int acol, int brow, int bcol)> visited)
    {
        foreach (var connection in node.NearestToStart)
        { // not sure why but it was getting stuck in a loop, so as a band-aid return if trying to save an existing connection
            if (connection.ConnectedNode == node) return;
            if (!visited.Add((node.Position.row, node.Position.col, connection.ConnectedNode.Position.row, connection.ConnectedNode.Position.col))) return;
            GetVisitedNodes(connection.ConnectedNode, visited);
        }
    }

    private static void GetPathCount(HashSet<(int arow, int acol, int brow, int bcol)> visitedConnections,
        HashSet<(int row, int col)> traveledPoints)
    {
        foreach (var connection in visitedConnections)
        {
            traveledPoints.Add((connection.brow, connection.bcol));
            if (connection.arow < connection.brow)
                for (var row = connection.arow; row < connection.brow; row++)
                    traveledPoints.Add((row, connection.acol));
            else if (connection.arow > connection.brow)
                for (var row = connection.arow; row > connection.brow; row--)
                    traveledPoints.Add((row, connection.acol));
            else if (connection.acol < connection.bcol)
                for (var col = connection.acol; col < connection.bcol; col++)
                    traveledPoints.Add((connection.arow, col));
            else if (connection.acol > connection.bcol)
                for (var col = connection.acol; col > connection.bcol; col--)
                    traveledPoints.Add((connection.arow, col));
        }
    }
    
    private static void PrintMap(string[] map, HashSet<(int row, int col)> traveledPaths)
    {
        for (int row = 0; row < map.Length; row++)
        {
            for (int col = 0; col < map[row].Length; col++)
            {
                if (traveledPaths.Contains((row, col)))
                    Console.Write('O');
                else if (map[row][col] == '#')
                    Console.Write('#');
                else
                    Console.Write(' ');
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}