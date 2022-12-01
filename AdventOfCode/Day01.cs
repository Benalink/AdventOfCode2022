namespace AdventOfCode;

public sealed class Day01 : BaseDay
{
    private readonly string input;

    public Day01()
    {
        this.input = File.ReadAllText(this.InputFilePath);
    }

    private IEnumerable<int> CalculateCalories() =>
        this.input.Split("\n\n")
            .Select(elf => elf.Split('\n')
                .Select(int.Parse)
                .Sum());

    public override ValueTask<string> Solve_1() => new(CalculateCalories().Max().ToString());

    public override ValueTask<string> Solve_2() =>
        new(CalculateCalories().OrderByDescending(c => c).Take(3).Sum().ToString());
}