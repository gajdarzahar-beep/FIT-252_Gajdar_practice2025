using System;
using System.IO;
using System.Reflection;
using System.Linq;
using task08.CommandLib;

namespace CommandRunner;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Command Runner ===");
        string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "task08.dll");

        if (!File.Exists(dllPath))
        {
            Console.WriteLine($"DLL not found: {dllPath}");
            return;
        }

        Assembly assembly = Assembly.LoadFrom(dllPath);

        var commandTypes = assembly.GetTypes()
            .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var type in commandTypes)
        {
            Console.WriteLine($"\nCommand: {type.Name}");
            object instance = null;

            if (type.Name == "DirectorySizeCommand")
            {
                Console.Write("Enter directory path: ");
                string path = Console.ReadLine();
                instance = Activator.CreateInstance(type, path);
            }
            else if (type.Name == "FindFilesCommand")
            {
                Console.Write("Enter directory path: ");
                string path = Console.ReadLine();
                Console.Write("Enter file pattern (e.g., *.txt): ");
                string pattern = Console.ReadLine();
                instance = Activator.CreateInstance(type, path, pattern);
            }
            else
            {
                instance = Activator.CreateInstance(type);
            }

            if (instance is ICommand command)
            {
                command.Execute();
            }
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
