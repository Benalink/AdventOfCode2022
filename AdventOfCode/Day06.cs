namespace AdventOfCode;

public sealed class Day06 : BaseDay
{
    private readonly string input;

    public Day06()
    {
        this.input = File.ReadAllText(this.InputFilePath);
    }

    private int SeekMarker(int markerLength)
    {
        ReadOnlySpan<char> inputSpan = this.input.AsSpan();

        for (int head = markerLength - 1; head < inputSpan.Length; head++)
        {
            ReadOnlySpan<char> slice = inputSpan.Slice(head - (markerLength - 1), markerLength);

            int i = markerLength - 1;
            while (i > 0)
            {
                if (slice[..i].Contains(slice[i]))
                {
                    break;
                }
                i--;
            }

            if (i == 0) return head;
        }

        return -1;
    }
    
    public override ValueTask<string> Solve_1()
    {
        int head = SeekMarker(4);
        return new((head + 1).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int head = SeekMarker(14);
        return new((head + 1).ToString());
    }
}