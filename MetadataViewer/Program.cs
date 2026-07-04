using System;
using System.IO;
using System.Reflection;

namespace MetadataViewer;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: MetadataViewer <path_to_dll>");
            return;
        }

        if (!File.Exists(args[0]))
        {
            Console.WriteLine($"File not found: {args[0]}");
            return;
        }

        Assembly assembly = Assembly.LoadFrom(args[0]);

        foreach (Type type in assembly.GetTypes())
        {
            if (type.IsPublic)
            {
                Console.WriteLine($"Class: {type.FullName}");

                foreach (Attribute attr in type.GetCustomAttributes())
                    Console.WriteLine($"  Attribute: {attr.GetType().Name}");

                foreach (ConstructorInfo ctor in type.GetConstructors())
                    Console.WriteLine($"  Constructor: {ctor}({GetParams(ctor.GetParameters())})");

                foreach (MethodInfo method in type.GetMethods())
                    Console.WriteLine($"  Method: {method.ReturnType.Name} {method.Name}({GetParams(method.GetParameters())})");

                Console.WriteLine();
            }
        }
    }

    static string GetParams(ParameterInfo[] p)
    {
        string result = "";
        foreach (ParameterInfo param in p)
        {
            if (result.Length > 0) result += ", ";
            result += $"{param.ParameterType.Name} {param.Name}";
        }
        return result;
    }
}