using System;

namespace task10.Plugins;

[PluginLoad("PluginA")]
public class PluginB
{
    public void Execute()
    {
        Console.WriteLine("PluginB executed (depends on PluginA)");
    }
}