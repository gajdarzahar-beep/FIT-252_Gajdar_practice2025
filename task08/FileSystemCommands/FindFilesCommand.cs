using System;
using System.IO;
using task08.CommandLib;
<<<<<<< HEAD
using task07;

namespace task08.FileSystemCommands;

[DisplayName("FindFilesCommand")]
[Version(1, 0)]
=======

namespace task08.FileSystemCommands;

>>>>>>> task08
public class FindFilesCommand : ICommand
{
    private readonly string _directoryPath;
    private readonly string _pattern;

    public FindFilesCommand(string directoryPath, string pattern)
    {
        _directoryPath = directoryPath;
        _pattern = pattern;
    }

    public void Execute()
    {
        if (!Directory.Exists(_directoryPath))
        {
            Console.WriteLine($"Directory '{_directoryPath}' does not exist.");
            return;
        }

        string[] files = Directory.GetFiles(_directoryPath, _pattern, SearchOption.AllDirectories);

        Console.WriteLine($"Found {files.Length} files matching pattern '{_pattern}':");
        foreach (var file in files)
        {
            Console.WriteLine($"  {file}");
        }
    }
}