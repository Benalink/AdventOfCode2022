using System.Runtime.Serialization;

namespace AdventOfCode;

public sealed class Day05 : BaseDay
{
    private readonly string input;

    public Day05()
    {
        this.input = File.ReadAllText(this.InputFilePath);
    }

    private (Stack<string>[] state, IEnumerable<Move> moves) ParseInput()
    {
        string[] stateSplit = this.input.Split("\n\n");
        Stack<string>[] state = ParseState(stateSplit[0]);
        IEnumerable<Move> moves = ParseMoves(stateSplit[1]);

        return (state, moves);
    }

    private static IEnumerable<Move> ParseMoves(string moveInput)
    {
        string[] lines = moveInput.Split('\n');
        
        foreach (string line in lines)
        {
            int[] integers = line.Split(' ')
                .Select(s => (int.TryParse(s, out int number), number))
                .Where(t => t.Item1)
                .Select(t => t.number)
                .ToArray();

            if (integers.Length != 3)
            {
                throw new InvalidInputException("Move input did not match expected format.");
            }

            yield return new Move
            {
                Amount = integers[0],
                FromCol = integers[1],
                ToCol = integers[2]
            };
        }
    }

    private static Stack<string>[] ParseState(string stateInput)
    {
        string[] lines = stateInput.Split('\n');
        Stack<string>[] state = null;

        bool init = false;
        foreach (string line in lines.Reverse())
        {
            if (!init)
            {
                int columnCount = line.Split("  ").Length;
                state = new Stack<string>[columnCount];
                init = true;
                continue;
            }
            
            for (int i = 0; i < state.Length; i++)
            {
                state[i] ??= new Stack<string>();
                string crate = line.Substring(i * 4, 3);
                
                if(!string.IsNullOrWhiteSpace(crate))
                    state[i].Push(line.Substring(i * 4, 3));
            }
        }

        return state;
    }

    public override ValueTask<string> Solve_1()
    {
        (Stack<string>[] state, IEnumerable<Move> moves) = ParseInput();
        
        foreach (Move move in moves)
        {
            for (int i = 0; i < move.Amount; i++)
            {
                string crate = state[move.FromCol - 1].Pop();
                state[move.ToCol - 1].Push(crate);
            }
        }

        IEnumerable<string> tops = state.Select(s => s.Pop()[1].ToString());
        string answer = string.Join(null, tops);
        return new ValueTask<string>(answer);
    }

    public override ValueTask<string> Solve_2()
    {
        (Stack<string>[] state, IEnumerable<Move> moves) = ParseInput();

        foreach (Move move in moves)
        {
            var workingSet = new List<string>();
            for (int i = 0; i < move.Amount; i++)
            {
                string crate = state[move.FromCol - 1].Pop();
                workingSet.Add(crate);
            }

            for (int i = workingSet.Count - 1; i >= 0; i--)
            {
                state[move.ToCol - 1].Push(workingSet[i]);
            }
        }
        
        IEnumerable<string> tops = state.Select(s => s.Pop()[1].ToString());
        string answer = string.Join(null, tops);
        return new ValueTask<string>(answer);
    }
}

[Serializable]
public class InvalidInputException : Exception
{
    //
    // For guidelines regarding the creation of new exception types, see
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
    // and
    //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
    //

    public InvalidInputException()
    {
    }

    public InvalidInputException(string message) : base(message)
    {
    }

    public InvalidInputException(string message, Exception inner) : base(message, inner)
    {
    }

    protected InvalidInputException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}

public struct Move
{
    public int Amount { get; init; }
    public int FromCol { get; init; }
    public int ToCol { get; init; }
}