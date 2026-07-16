using System;
using task17;

namespace task18;

public class TestCommand : ILongRunningCommand
{
    private readonly int _id;
    private int _counter;
    private bool _isCompleted;

    public TestCommand(int id)
    {
        _id = id;
        _counter = 0;
        _isCompleted = false;
    }

    public bool IsCompleted => _isCompleted;

    public void Execute()
    {
        if (_isCompleted) return;

        _counter++;
        Console.WriteLine($"Поток {_id} вызов {_counter}");

        if (_counter >= 3)
        {
            _isCompleted = true;
        }
    }

    public int GetCounter() => _counter;
}