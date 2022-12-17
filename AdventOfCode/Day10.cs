namespace AdventOfCode;

public sealed class Day10 : BaseDay
{
    private readonly string input;

    public Day10()
    {
        this.input = File.ReadAllText(this.InputFilePath);
    }

    private static IEnumerable<int> RunCycles(IEnumerable<string> commands)
    {
        int register = 1;
        yield return register;
        foreach (string command in commands)
        {
            string[] parts = command.Split(' ');

            if (parts[0] != "noop")
            {
                yield return register;
                int arg = int.Parse(parts[1]);
                register += arg;
            }

            yield return register;
        }
    }
    
    public override ValueTask<string> Solve_1()
    {
        string[] commandList = this.input.Split('\n');
        IEnumerable<(int, int reg)> interestingSignals = RunCycles(commandList)
            .Select((reg, cycle) => (cycle + 1, reg))
            .Where(tuple => (tuple.Item1 - 20) % 40 == 0);

        int sumStrength = interestingSignals.Aggregate(0, (sum, tuple) => sum + tuple.Item1 * tuple.reg);
        return new ValueTask<string>(sumStrength.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        string[] commandList = this.input.Split('\n');
        IEnumerable<(int, int reg)> interestingSignals = RunCycles(commandList)
            .Select((reg, cycle) => (cycle + 1, reg));

        foreach ((int, int reg) cpu in interestingSignals)
        {
            int pos = (cpu.Item1 - 1) % 40;
            Console.Write(Math.Abs(pos - cpu.reg) < 2 ? '#' : ' ');

            if(pos == 39) 
                Console.WriteLine();
        }

        return new ValueTask<string>("Printed above!");
    }
}