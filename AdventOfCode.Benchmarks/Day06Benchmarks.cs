using BenchmarkDotNet.Attributes;

namespace AdventOfCode.Benchmarks;

[MemoryDiagnoser]
public class Day06Benchmarks
{
    private static readonly Day06 Solver = new();

    [Benchmark]
    public async Task Solve_1()
    {
        await Solver.Solve_1();
    }
    
    [Benchmark]
    public async Task Solve_2()
    {
        await Solver.Solve_2();
    }
}