using Xunit;
using task08.FileSystemCommands;
using System;
using System.IO;

namespace task08tests;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        var command = new DirectorySizeCommand(testDir);
        var exception = Record.Exception(() => command.Execute());

        Assert.Null(exception);
        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDir");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        var command = new FindFilesCommand(testDir, "*.txt");
        var exception = Record.Exception(() => command.Execute());

        Assert.Null(exception);
        Directory.Delete(testDir, true);
    }

    [Fact]
    public void DirectorySizeCommand_InvalidPath()
    {
        var command = new DirectorySizeCommand(@"C:\NonExistentDir12345");
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);
    }

    [Fact]
    public void FFindFilesCommand_InvalidPath()
    {
        var command = new FindFilesCommand(@"C:\NonExistentDir12345", "*.txt");
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);
    }
}
