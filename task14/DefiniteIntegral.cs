using System;
using System.Threading;

namespace task14;

public static class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        if (threadsNumber <= 0)
        {
            throw new ArgumentException("threadsNumber must be greater than 0", nameof(threadsNumber));
        }

        if (step <= 0)
        {
            throw new ArgumentException("step must be greater than 0", nameof(step));
        }

        if (a >= b)
        {
            throw new ArgumentException("a must be less than b", nameof(a));
        }

        double totalResult = 0.0;
        double totalLength = b - a;
        double segmentLength = totalLength / threadsNumber;

        var barrier = new Barrier(threadsNumber, (barier) =>
        {
            
        });

        var threads = new Thread[threadsNumber];

        for (int i = 0; i < threadsNumber; i++)
        {
            double segmentA = a + i * segmentLength;
            double segmentB = (i == threadsNumber - 1) ? b : a + (i + 1) * segmentLength;

            threads[i] = new Thread(() =>
            {
                double localResult = 0.0;
                double currentX = segmentA;

                while (currentX < segmentB)
                {
                    double nextX = currentX + step;
                    if (nextX > segmentB)
                    {
                        nextX = segmentB;
                    }

                    double trapArea = (function(currentX) + function(nextX)) / 2.0 * (nextX - currentX);
                    localResult += trapArea;
                    currentX = nextX;
                }

                Interlocked.Exchange(ref totalResult, totalResult + localResult);
                barrier.SignalAndWait();
            });

            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        return totalResult;
    }
}