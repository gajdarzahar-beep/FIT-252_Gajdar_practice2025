using System;

namespace task14tests;

public static class SingleThreadIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step)
    {
        double result = 0.0;
        double currentX = a;

        while (currentX < b)
        {
            double nextX = currentX + step;
            if (nextX > b)
            {
                nextX = b;
            }

            double trapArea = (function(currentX) + function(nextX)) / 2.0 * (nextX - currentX);
            result += trapArea;
            currentX = nextX;
        }

        return result;
    }
}