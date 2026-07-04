using System;

namespace task10.Plugins;

[PluginLoad("PluginB")]
public class PluginC
{
    public void Execute()
    {
        Console.WriteLine("PluginC executed (depends on PluginB)");
    }
}