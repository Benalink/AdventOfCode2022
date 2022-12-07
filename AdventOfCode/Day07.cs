namespace AdventOfCode;

public sealed class Day07 : BaseDay
{
    private readonly string input;

    public Day07()
    {
        this.input = File.ReadAllText(this.InputFilePath);
    }

    public FileNode Parse(string[] inputLines, FileNode node = null)
    {
        node ??= FileNode.Default;

        return inputLines switch
        {
            [] => node,
            [var current, .. var rest] => 
                current.Split(' ') switch
            {
                ["$", "cd", "/"] => Parse(rest, node.GetRoot()),
                ["$", "cd", ".."] => Parse(rest, node.Parent),
                ["$", "cd", var arg] => Parse(rest, node.Children[arg]),
                ["$", "ls"] => Parse(rest, node),
                ["dir", var directoryName] => Parse(rest, node.AddDir(directoryName)),
                [var size, var fileName] => Parse(rest, node.AddFile(size, fileName)),
                _ => throw new InvalidOperationException("Oops")
            }
        };
    }

    public List<FileNode> GetDirectoriesWhere(Predicate<FileNode> predicate, FileNode node)
    {
        var matches = new List<FileNode>();

        if (predicate(node))
        {
            matches.Add(node);
        }

        if (!node.IsDir) return matches;
        
        IEnumerable<FileNode> childMatches = node.Children.SelectMany(child => GetDirectoriesWhere(predicate, child.Value));
        matches.AddRange(childMatches);

        return matches;
    }

    public override ValueTask<string> Solve_1()
    {
        FileNode tree = Parse(this.input.Split('\n')).GetRoot();
        List<FileNode> matches = GetDirectoriesWhere(node => node.IsDir && node.CalculateSize() <= 100000, tree);
        int matchingSizeSum = matches.Sum(node => node.CalculateSize());
        
        return new ValueTask<string>(matchingSizeSum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        FileNode tree = Parse(this.input.Split('\n')).GetRoot();
        int totalUsedSpace = tree.CalculateSize();
        int freeSpace = 70000000 - totalUsedSpace;
        int additionalFreeSpaceNeeded = 30000000 - freeSpace;
        List<FileNode> matches = GetDirectoriesWhere(node => node.IsDir && node.CalculateSize() >= additionalFreeSpaceNeeded, tree);

        
        int minMatchingSize = matches.Min(node => node.CalculateSize());
        
        return new ValueTask<string>(minMatchingSize.ToString());
    }
}

public record FileNode
{
    internal static readonly FileNode Default = new() { Name = "/", IsDir = true, Children = new Dictionary<string, FileNode>(), Size = 0};
    
    public FileNode Parent { get; set; }
    public string Name { get; set; }
    public bool IsDir { get; set; }
    public int Size { get; set; }
    public Dictionary<string, FileNode> Children { get; init; }

    public int CalculateSize()
    {
        return !this.IsDir ? this.Size : this.Children.Values.Sum(node => node.CalculateSize());
    }


    public FileNode AddFile(string fileSize, string fileName)
    {
        this.Children.TryAdd(fileName, new FileNode { Name = fileName, Size = int.Parse(fileSize), IsDir = false, Parent = this});
        return this;
    }
    
    public FileNode AddDir(string dirName)
    {
        this.Children.TryAdd(dirName, new FileNode { Name = dirName, IsDir = true, Children = new Dictionary<string, FileNode>(), Parent = this});
        return this;
    }

    public FileNode GetRoot()
    {
        return this.Parent is not null ? this.Parent.GetRoot() : this;
    }
}