namespace AdventOfCode;

public sealed class Day08 : BaseDay
{
    private readonly string input;

    public Day08()
    {
        this.input = File.ReadAllText(this.InputFilePath);
    }

    public int[,] ParseInputMap()
    {
        int[,] map = null;
        
        string[] lines = this.input.Split('\n');
        
        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[y];

            for (int x = 0; x < line.Length; x++)
            {
                map ??= new int[line.Length, lines.Length];

                map[x, y] = int.Parse(line[x].ToString());
            }
        }

        return map;
    }
    
    public override ValueTask<string> Solve_1()
    {
        int[,] map = ParseInputMap();
        var visibleList = new HashSet<(int x, int y)>();

        for (int x = 0; x < map.GetLength(0); x++)
        {
            int highestSeen = -1;
            for (int y = 0; y < map.GetLength(1); y++)
            {
                int current = map[x, y];

                if (current > highestSeen)
                {
                    visibleList.Add((x, y));
                    highestSeen = current;

                    if (current == 9) break;
                }
            }

            highestSeen = -1;
            for (int y = map.GetLength(1) - 1; y >= 0; y--)
            {
                int current = map[x, y];

                if (current > highestSeen)
                {
                    visibleList.Add((x, y));
                    highestSeen = current;

                    if (current == 9) break;
                }
            }
        }
        
        for (int y = 0; y < map.GetLength(1); y++)
        {
            int highestSeen = -1;
            for (int x = 0; x < map.GetLength(0); x++)
            {
                int current = map[x, y];

                if (current > highestSeen)
                {
                    visibleList.Add((x, y));
                    highestSeen = current;

                    if (current == 9) break;
                }
            }

            highestSeen = -1;
            for (int x = map.GetLength(0) - 1; x >= 0; x--)
            {
                int current = map[x, y];

                if (current > highestSeen)
                {
                    visibleList.Add((x, y));
                    highestSeen = current;

                    if (current == 9) break;
                }
            }
        }

        return new(visibleList.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        int[,] map = ParseInputMap();

        int highestScenicScore = -1;
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                int scenicScore = CalculateScenicScore(map, x, y);
                if (scenicScore > highestScenicScore) highestScenicScore = scenicScore;
            }
        }

        return new(highestScenicScore.ToString());
    }

    private int CalculateScenicScore(int[,] map, int x, int y)
    {
        int height = map[x, y];
        
        int score = 1;
        int count = 0;
        for (int xi = x + 1; xi < map.GetLength(0); xi++)
        {
            if (map[xi, y] < height)
            {
                count++;
            }
            else
            {
                count++;
                break;
            }
        }

        score *= count;
        count = 0;
        for (int xi = x - 1; xi >= 0; xi--)
        {
            if (map[xi, y] < height)
            {
                count++;
            }
            else
            {
                count++;
                break;
            }
        }
        
        score *= count;
        count = 0;
        for (int yi = y + 1; yi < map.GetLength(1); yi++)
        {
            if (map[x, yi] < height)
            {
                count++;
            }
            else
            {
                count++;
                break;
            }
        }

        score *= count;
        count = 0;
        for (int yi = y - 1; yi >= 0; yi--)
        {
            if (map[x, yi] < height)
            {
                count++;
            }
            else
            {
                count++;
                break;
            }
        }

        score *= count;

        return score;
    }
}