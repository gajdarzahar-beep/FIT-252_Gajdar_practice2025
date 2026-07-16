using Xunit;
using task14;
using System;

namespace task14tests;

public class DefiniteIntegralTests
{
    [Fact]
    public void Integral_1()
    {
        var X = (double x) => x;
        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, X, 1e-4, 2), 1e-4);
    }

    [Fact]
    public void Integral_2()
    {
        var SIN = (double x) => Math.Sin(x);
        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, SIN, 1e-5, 8), 1e-4);
    }

    [Fact]
    public void Integral_3()
    {
        var X = (double x) => x;
        Assert.Equal(12.5, DefiniteIntegral.Solve(0, 5, X, 1e-6, 8), 1e-5);
    }

    [Fact]
    public void Integral_InvalidThreadsNumber()
    {
        var X = (double x) => x;
        Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(0, 1, X, 0.1, 0));
    }

    [Fact]
    public void Integral_InvalidStep()
    {
        var X = (double x) => x;
        Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(0, 1, X, -0.1, 2));
    }

    [Fact]
    public void Integral_InvalidInterval()
    {
        var X = (double x) => x;
        Assert.Throws<ArgumentException>(() => DefiniteIntegral.Solve(1, 0, X, 0.1, 2));
    }
}