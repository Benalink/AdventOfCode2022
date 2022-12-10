using System.Numerics;

namespace AdventOfCode;

public sealed class Day09 : BaseDay
{
    private readonly string input;

    public Day09()
    {
        this.input = File.ReadAllText(this.InputFilePath);
    }
    
    public override ValueTask<string> Solve_1()
    {
        IEnumerable<MoveCmd> moves = this.input.Split('\n').Select(cmd => new MoveCmd(cmd.Split(' ')));

        RopeStateHistory result = RunMoves(moves);

        int uniqueTailPositions = result.History.Select(h => h.Tails[0]).Distinct().OrderBy(x => x.Y).Count();
        return new ValueTask<string>(uniqueTailPositions.ToString());
    }

    private static RopeStateHistory RunMoves(IEnumerable<MoveCmd> moves, RopeState initial = null)
    {
        RopeStateHistory result = moves.Aggregate(new RopeStateHistory(initial), (state, cmd) =>
        {
            for (int i = 0; i < cmd.Steps; i++)
            {
                RopeState last = state.Latest;
                RopeState workingState = last with { Head = last.Head + cmd.Direction };

                for (int t = 0; t < last.Tails.Length; t++)
                {
                    Vector2 tail = workingState.Tails[t];
                    Vector2 head = workingState.Head;
                    if (t > 0)
                    {
                        head = workingState.Tails[t - 1];
                    }

                    Vector2 tailDelta = head - tail;
                    Vector2 absoluteTailDelta = Vector2.Abs(tailDelta);

                    if (absoluteTailDelta.X + absoluteTailDelta.Y > 1 && absoluteTailDelta != Vector2.One)
                    {
                        Vector2 clampedTailDelta = ClampTailDelta(tailDelta);
                        Vector2 updatedTail = tail + clampedTailDelta;
                        Vector2[] tails = workingState.Tails.ToArray();
                        tails[t] = updatedTail;
                        workingState = workingState with { Tails = tails };
                    }
                }
                
                state.Push(workingState);
            }

            return state;
        });
        return result;
    }

    private static Vector2 ClampTailDelta(Vector2 delta) => new(Math.Clamp(delta.X, -1, 1), Math.Clamp(delta.Y, -1, 1));

    public override ValueTask<string> Solve_2()
    {
        IEnumerable<MoveCmd> moves = this.input.Split('\n').Select(cmd => new MoveCmd(cmd.Split(' ')));
        RopeStateHistory result = RunMoves(moves, new RopeState(Vector2.Zero, Enumerable.Repeat(Vector2.Zero, 9).ToArray()));

        int uniqueTailPositions = result.History.Select(h => h.Tails[8]).Distinct().OrderBy(x => x.Y).Count();
        return new ValueTask<string>(uniqueTailPositions.ToString());
    }
}



public record RopeState(Vector2 Head, Vector2[] Tails);

public class RopeStateHistory
{
    public RopeState Latest { get; private set; }
    public List<RopeState> History { get; }

    public RopeStateHistory(RopeState initial = null)
    {
        this.Latest = initial ?? new RopeState(Vector2.Zero, new []{ Vector2.Zero });
        this.History = new List<RopeState> { this.Latest};
    }

    public RopeStateHistory Push(RopeState state)
    {
        this.Latest = state;
        this.History.Add(state);
        
        return this;
    }
}


public struct MoveCmd
{
    public Vector2 Direction { get; }
    public int Steps { get;  }

    public MoveCmd(IReadOnlyList<string> inputCommand)
    {
        this.Direction = inputCommand[0] switch
        {
            "R" => new Vector2(1, 0),
            "L" => new Vector2(-1, 0),
            "U" => new Vector2(0, 1),
            "D" => new Vector2(0, -1),
            _ => throw new InvalidOperationException()
        };
        
        this.Steps = int.Parse(inputCommand[1]);
    }
}