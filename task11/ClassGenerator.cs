using System;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace task11;

public static class ClassGenerator
{
    public static object CreateCalculator()
    {
        string code = @"
using System;

public class Calculator
{
    public int Add(int a, int b) => a + b;
    public int Minus(int a, int b) => a - b;
    public int Mul(int a, int b) => a * b;
    public int Div(int a, int b) => a / b;
}
";

        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location)
        };

        var compilation = CSharpCompilation.Create(
            "GeneratedCalculator",
            new[] { syntaxTree },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        using var ms = new System.IO.MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            var errors = "";
            foreach (var diagnostic in result.Diagnostics)
            {
                errors += $"{diagnostic}\n";
            }
            throw new Exception($"Compilation failed:\n{errors}");
        }

        ms.Seek(0, System.IO.SeekOrigin.Begin);
        var assembly = Assembly.Load(ms.ToArray());
        var type = assembly.GetType("Calculator");
        return Activator.CreateInstance(type);
    }
}