using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScottPlot;

namespace task14tests;

public static class PlotGenerator
{
    public static void CreatePlot(List<TestResult> results)
    {
        Console.WriteLine("Let's start plotting the graph...");
        Console.WriteLine($"Number of points: {results.Count}");

        var plt = new Plot();
        plt.Title("Зависимость времени выполнения от числа потоков");
        plt.XLabel("Количество потоков");
        plt.YLabel("Время выполнения (мс)");

        double[] xs = results.Select(r => (double)r.Threads).ToArray();
        double[] ys = results.Select(r => r.AverageTime).ToArray();

        Console.WriteLine($"X: {string.Join(", ", xs)}");
        Console.WriteLine($"Y: {string.Join(", ", ys)}");

        plt.Add.Scatter(xs, ys);
        plt.Add.Markers(xs, ys);

        string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
        string path = Path.Combine(projectRoot, "plot.png");

        plt.SavePng(path, 800, 600);
        Console.WriteLine($"The graph is saved in {path}");
    }
}