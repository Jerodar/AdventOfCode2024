using AdventOfCode2024Input;

namespace AdventOfCode2024;

public static class Day19
{
    private class Node
    {
        public char Content { get; init; }
        public bool Terminates { get; set; } = false;
        private readonly List<Node> _children = new();

        public Node? GetChild(char c)
        {
            return (from node in _children where node.Content == c select node).FirstOrDefault();
        }
        
        public Node AddChild(char c)
        {
            var node = GetChild(c);
            if (node != null) return node;
            
            node = new Node { Content = c };
            _children.Add(node);
            return node;
        }

        public List<string> Print(string prev)
        {
            if (_children.Count == 0 && !Terminates) return new List<string>();
            
            prev += Content;
            var result = new List<string>();
            
            if (Terminates) result.Add(prev);
            foreach (var node in _children)
                result.AddRange(node.Print(prev));
            
            return result;
        }
    }
    
    private class InvertedTree
    {
        private readonly List<Node> _children = new();

        // Creates a tree with the last char as root
        public void Add(string newContent)
        {
            var currentNode = GetChild(newContent[^1]);
            if (currentNode == null)
            {
                currentNode = new Node { Content = newContent[^1] };
                _children.Add(currentNode);
            }
            
            for (int i = newContent.Length -2; i >= 0; i--)
            {
                currentNode = currentNode.AddChild(newContent[i]);
            }

            currentNode.Terminates = true;
        }

        // Prints the tree starting at the root, so the result is inverted from the inputs
        public void PrintTree()
        {
            if (_children.Count == 0) return;
            var result = new List<string>();
            foreach (var node in _children)
                result.AddRange(node.Print(""));
            foreach (var line in result)
                Console.Write(line + ", ");
            Console.WriteLine();
        }

        // Checks if the target string can be found in the tree, starting with the last char
        public bool RecursiveFind(string target)
        {
            var currentNode = GetChild(target[^1]);
            if (currentNode == null) return false;
            
            for (int i = target.Length - 2; i >= 0; i--)
            {
                if (currentNode.Terminates && RecursiveFind(target[..(i+1)])) return true;
                
                var nextNode = currentNode.GetChild(target[i]);
                if (nextNode == null) return false;

                currentNode = nextNode;
            }
            
            return currentNode.Terminates;
        }

        public long RecursiveFindCount(string target)
        {
            long count = 0;
            var currentNode = GetChild(target[^1]);
            if (currentNode == null) return count;
            
            for (int i = target.Length - 2; i >= 0; i--)
            {
                if (currentNode.Terminates) count += RecursiveFindCount(target[..(i+1)]);
                
                var nextNode = currentNode.GetChild(target[i]);
                if (nextNode == null) return count;

                currentNode = nextNode;
            }
            if (currentNode.Terminates) count++;
            return count;
        }
        
        private Node? GetChild(char c)
        {
            return (from node in _children where node.Content == c select node).FirstOrDefault();
        }
    }
    
    public static void Run()
    {
        Console.WriteLine("Day 19 Part One");

        string[] towels = PuzzleData.GetDay19Towels();
        string[] designs = PuzzleData.GetDay19Designs();
        
        InvertedTree tree = new();
        foreach (string towel in towels)
            tree.Add(towel);
        tree.PrintTree();

        long count = 0;
        foreach (string design in designs)
        {
            if (tree.RecursiveFind(design))
            {
                Console.WriteLine($"Found {design}");
                count++;
            }
            else
            {
                Console.WriteLine($"Failed {design}");
            }
        }
        Console.WriteLine(count);
        
        Console.WriteLine();
        Console.WriteLine("Day 19 Part Two");
        count = 0;
        foreach (string design in designs)
        {
            var designCount = tree.RecursiveFindCount(design);
            Console.WriteLine($"{designCount} - {design}");
            count += designCount;
        }
        Console.WriteLine(count);
    }
}