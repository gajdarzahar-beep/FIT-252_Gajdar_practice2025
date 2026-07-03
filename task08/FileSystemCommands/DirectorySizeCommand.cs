using System;
using System.IO;
using task08.CommandLib;

namespace task08.FileSystemCommands;

public class DirectorySizeCommand : ICommand
{
    private readonly string _directoryPath;

    public DirectorySizeCommand(string directoryPath)
    {
        _directoryPath = directoryPath;
    }

    public void Execute()
    {
        if (!Directory.Exists(_directoryPath))
        {
            Console.WriteLine($"Directory '{_directoryPath}' does not exist.");
            return;
        }

        long size = 0;
        foreach (var file in Directory.GetFiles(_directoryPath, "*", SearchOption.AllDirectories))
        {
            size += new FileInfo(file).Length;
        }

        Console.WriteLine($"Directory size: {size} bytes");
    }
}