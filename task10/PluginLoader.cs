using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace task10;

public static class PluginLoader
{
    public static List<object> LoadPlugins(string directoryPath)
    {
        var plugins = new List<object>();
        var pluginTypes = new List<Type>();

        if (!Directory.Exists(directoryPath))
        {
            Console.WriteLine($"Directory not found: {directoryPath}");
            return plugins;
        }

        var dllFiles = Directory.GetFiles(directoryPath, "*.dll");
        Console.WriteLine($"Found {dllFiles.Length} DLL files in {directoryPath}");

        foreach (var dll in dllFiles)
        {
            try
            {
                Console.WriteLine($"Loading DLL: {dll}");
                var assembly = Assembly.LoadFrom(dll);
                Console.WriteLine($"  Types found: {assembly.GetTypes().Length}");

                foreach (var type in assembly.GetTypes())
                {
                    Console.WriteLine($"    Type: {type.FullName}");
                    var attr = type.GetCustomAttribute<PluginLoadAttribute>();
                    if (attr != null && !type.IsInterface && !type.IsAbstract)
                    {
                        Console.WriteLine($"      ✓ Found plugin: {type.Name}");
                        pluginTypes.Add(type);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading {dll}: {ex.Message}");
            }
        }

        Console.WriteLine($"Total plugins found: {pluginTypes.Count}");

        var sortedTypes = TopologicalSort(pluginTypes);

        foreach (var type in sortedTypes)
        {
            try
            {
                var instance = Activator.CreateInstance(type);
                if (instance != null)
                {
                    plugins.Add(instance);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating instance of {type.Name}: {ex.Message}");
            }
        }

        return plugins;
    }

    private static List<Type> TopologicalSort(List<Type> types)
    {
        var sorted = new List<Type>();
        var visited = new HashSet<Type>();

        foreach (var type in types)
        {
            if (!visited.Contains(type))
            {
                Visit(type, types, visited, sorted);
            }
        }

        return sorted;
    }

    private static void Visit(Type type, List<Type> allTypes, HashSet<Type> visited, List<Type> sorted)
    {
        if (visited.Contains(type))
        {
            return;
        }

        visited.Add(type);

        var attr = type.GetCustomAttribute<PluginLoadAttribute>();
        if (attr != null)
        {
            foreach (var depName in attr.Dependencies)
            {
                var depType = allTypes.FirstOrDefault(t => t.Name == depName);
                if (depType != null && !visited.Contains(depType))
                {
                    Visit(depType, allTypes, visited, sorted);
                }
            }
        }

        sorted.Add(type);
    }
}