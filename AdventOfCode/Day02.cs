namespace AdventOfCode;

public sealed class Day02 : BaseDay
{
    private readonly string input;

    public Day02()
    {
        this.input = File.ReadAllText(this.InputFilePath);
    }

    private IEnumerable<RpsMove[]> ParseInput() => 
        this.input.Split('\n')
            .Select(match => 
                match.Split(' ')
                    .Select(move => new RpsMove(move))
                    .ToArray());

    public override ValueTask<string> Solve_1() => new(ParseInput().Aggregate(0, (i, moves) =>
    {
        i += moves[1].Value + 1;

        if (moves[0].Value == moves[1].Value)
            return i + 3;

        if (moves[0].Value == (moves[1].Value + 1) % 3)
            return i + 0;

        return i + 6;

    }).ToString());

    public override ValueTask<string> Solve_2() => new(ParseInput().Aggregate(0, (i, moves) =>
    {
        int move = moves[1].Value switch
        {
            0 => Mod(moves[0].Value - 1, 3),
            2 => Mod(moves[0].Value + 1, 3),
            _ => moves[0].Value
        };

        move += 1;

        return i + move + moves[1].Value * 3;
    }).ToString());

    static int Mod(int x, int m) {
        return (x%m + m)%m;
    }


    struct RpsMove
    {
        public int Value { get; }

        public RpsMove(string input)
        {
            switch (input)
            {
                case "A":
                case "X":
                    this.Value = 0;
                    break;
                case "B":
                case "Y":
                    this.Value = 1;
                    break;
                case "C":
                case "Z":
                    this.Value = 2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input), "Unknown move, can't calculate value");
            }
        }
    }
}