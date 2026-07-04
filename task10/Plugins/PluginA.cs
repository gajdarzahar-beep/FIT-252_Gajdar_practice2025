using System;

namespace task10.Plugins;

[PluginLoad]
public class PluginA
{
    public void Execute()
    {
        Console.WriteLine("PluginA executed");
    }
}