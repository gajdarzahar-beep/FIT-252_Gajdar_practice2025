using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using task14;

namespace task14tests;

public static class Benchmark
{
    public static List<TestResult> MeasureTime()
    {
        var results = new List<TestResult>();
        var sin = (double x) => Math.Sin(x);
        double a = -100;
        double b = 100;

        double[] steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
        int[] threadCounts = { 1, 2, 4, 8, 16, 32 };
        int attempts = 5;

        double optimalStep = 1e-4;
        double bestStep = 0;

        int optimalThreads = 1;
        double bestTime = double.MaxValue;

        for (int i = 0; i < steps.Length; i++)
        {
            if (steps[i] <= optimalStep)
            {
                bestStep = steps[i];
                break;
            }
        }

        double sum = 0;
        double avgTime = 0;
        double minTime = 0;
        double maxTime = 0;
        List<double> times = null;

        for (int t = 0; t < threadCounts.Length; t++)
        {
            int threads = threadCounts[t];
            times = new List<double>();

            for (int i = 0; i < attempts; i++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DefiniteIntegral.Solve(a, b, sin, bestStep, threads);
                sw.Stop();
                times.Add(sw.Elapsed.TotalMilliseconds);
            }

            Console.WriteLine($"Measurements for {threads} threads: {string.Join(", ", times)}");

            sum = 0;
            for (int i = 0; i < times.Count; i++)
            {
                sum = sum + times[i];
            }
            avgTime = sum / times.Count;

            minTime = times[0];
            maxTime = times[0];
            for (int i = 0; i < times.Count; i++)
            {
                if (times[i] < minTime) minTime = times[i];
                if (times[i] > maxTime) maxTime = times[i];
            }

            TestResult result = new TestResult();
            result.Threads = threads;
            result.AverageTime = avgTime;
            result.MinTime = minTime;
            result.MaxTime = maxTime;
            results.Add(result);

            if (avgTime < bestTime)
            {
                bestTime = avgTime;
                optimalThreads = threads;
            }
        }

        var singleThreadTimes = new List<double>();
        for (int i = 0; i < attempts; i++)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            SingleThreadIntegral.Solve(a, b, sin, bestStep);
            sw.Stop();
            singleThreadTimes.Add(sw.Elapsed.TotalMilliseconds);
        }

        Console.WriteLine($"Measurements for the single-threaded version: {string.Join(", ", singleThreadTimes)}");

        double sumSingle = 0;
        for (int i = 0; i < singleThreadTimes.Count; i++)
        {
            sumSingle = sumSingle + singleThreadTimes[i];
        }
        double singleThreadAvg = sumSingle / singleThreadTimes.Count;

        TestResult optimalResult = null;
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].Threads == optimalThreads)
            {
                optimalResult = results[i];
                break;
            }
        }

        double differencePercent = 0;
        if (singleThreadAvg > 0)
        {
            differencePercent = ((singleThreadAvg - optimalResult.AverageTime) / singleThreadAvg) * 100;
        }

        return results;
    }
}
public class TestResult
{
    public int Threads { get; set; }
    public double AverageTime { get; set; }
    public double MinTime { get; set; }
    public double MaxTime { get; set; }
}
