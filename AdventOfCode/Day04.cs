namespace AdventOfCode;

public sealed class Day04 : BaseDay
{
    private readonly string input;

    public Day04()
    {
        this.input = File.ReadAllText(this.InputFilePath);
    }

    private IEnumerable<(int[] A, int[] B)> ParseInput() =>
        this.input.Split('\n').Select(line =>
        {
            string[] pairSplit = line.Split(',');
            IEnumerable<int>[] ranges = pairSplit.Select(range =>
            {
                int[] boundSplit = range.Split('-').Select(int.Parse).ToArray();
                return Enumerable.Range(boundSplit[0], boundSplit[1] - boundSplit[0] + 1);
            }).ToArray();

            return (ranges[0].ToArray(), ranges[1].ToArray());
        });

    public override ValueTask<string> Solve_1()
    {
        int fullyContainedCount = ParseInput()
            .Count(pair => pair.A.Intersect(pair.B).Count() == Math.Min(pair.A.Length, pair.B.Length));

        return new ValueTask<string>(fullyContainedCount.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int overlapsCount = ParseInput().Count(pair => pair.A.Intersect(pair.B).Any());
        return new ValueTask<string>(overlapsCount.ToString());
    }
}