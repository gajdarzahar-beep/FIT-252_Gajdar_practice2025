using Xunit;

namespace task14tests;

public class BenchmarkTests
{
    [Fact]
    public void RunBenchmark()
    {
        var results = Benchmark.MeasureTime();
        Assert.True(results.Count > 0);
        PlotGenerator.CreatePlot(results);
    }
}