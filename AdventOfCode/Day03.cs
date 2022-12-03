namespace AdventOfCode;

public sealed class Day03 : BaseDay
{
    private readonly string input;

    public Day03()
    {
        this.input = File.ReadAllText(this.InputFilePath);
    }

    private IEnumerable<Rucksack> ParseRucksacks() =>
        this.input.Split('\n').Select(txt => new Rucksack(txt));

    public override ValueTask<string> Solve_1()
    {
        int errorSum = ParseRucksacks()
            .Select(rucksack => 
                GetPriority(
                    rucksack.CompartmentA.Intersect(rucksack.CompartmentB).First()
                    )).Sum();
        
        return new ValueTask<string>(errorSum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int badgePrioritySum = ParseRucksacks()
            .Select((rucksack, i) => (rucksack, i))
            .GroupBy(t => t.i / 3)
            .SelectMany(g => 
                g.Select(t => t.rucksack.Contents.AsEnumerable())
                    .Aggregate((common, next) => common.Intersect(next))).Select(GetPriority).Sum();

        return new ValueTask<string>(badgePrioritySum.ToString());
    }

    private static int GetPriority(char item) => char.IsLower(item) ? item - 96 : item - 38;
    
    public class Rucksack
    {
        public string Contents { get; }
        public int CompartmentSize { get; }
        public string CompartmentA { get; }
        public string CompartmentB { get; }
        
        public Rucksack(string contents)
        {
            this.Contents = contents;
            this.CompartmentSize = contents.Length / 2;
            this.CompartmentA = contents[.. this.CompartmentSize];
            this.CompartmentB = contents[this.CompartmentSize ..];
        }
    }
}