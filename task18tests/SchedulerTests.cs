using Xunit;
using task17;
using task18;
using System.Threading;

namespace task18tests;

public class SchedulerTests
{
    [Fact]
    public void RoundRobinScheduler_AddCommand_ExecutesCommand()
    {
        var scheduler = new RoundRobinScheduler();
        var command = new TestCommand();

        scheduler.Add(command);
        var selected = scheduler.Select();

        Assert.NotNull(selected);
        Assert.Equal(command, selected);
    }

    [Fact]
    public void RoundRobinScheduler_Select_ReturnsCommandsInOrder()
    {
        var scheduler = new RoundRobinScheduler();
        var command1 = new TestCommand();
        var command2 = new TestCommand();

        scheduler.Add(command1);
        scheduler.Add(command2);

        var selected1 = scheduler.Select();
        var selected2 = scheduler.Select();

        Assert.Equal(command1, selected1);
        Assert.Equal(command2, selected2);
    }

    [Fact]
    public void LongRunningCommand_IsExecutedMultipleTimes()
    {
        var scheduler = new RoundRobinScheduler();
        var command = new LongRunningTestCommand(10);

        scheduler.Add(command);

        for (int i = 0; i < 5; i++)
        {
            var selected = scheduler.Select();
            selected.Execute();
        }

        Assert.Equal(5, ((LongRunningTestCommand)command).ExecuteCount);
        Assert.False(command.IsCompleted);
    }

    [Fact]
    public void LongRunningCommand_IsCompletedAfterMaxExecutions()
    {
        var scheduler = new RoundRobinScheduler();
        var command = new LongRunningTestCommand(3);

        scheduler.Add(command);

        for (int i = 0; i < 3; i++)
        {
            var selected = scheduler.Select();
            selected.Execute();
        }

        Assert.True(command.IsCompleted);
    }

    [Fact]
    public void ServerThread_AddCommand_ExecutesCommand()
    {
        var scheduler = new RoundRobinScheduler();
        var serverThread = new task18.ServerThread(scheduler);
        var command = new TestCommand();

        serverThread.AddCommand(command);
        Thread.Sleep(100);

        Assert.True(((TestCommand)command).IsExecuted);
    }

    private class TestCommand : ICommand
    {
        public bool IsExecuted { get; private set; }

        public void Execute()
        {
            IsExecuted = true;
        }
    }

    private class LongRunningTestCommand : ILongRunningCommand
    {
        private readonly int _maxExecutions;
        private int _executeCount;

        public bool IsCompleted => _executeCount >= _maxExecutions;
        public int ExecuteCount => _executeCount;

        public LongRunningTestCommand(int maxExecutions)
        {
            _maxExecutions = maxExecutions;
        }

        public void Execute()
        {
            if (IsCompleted) return;
            _executeCount++;
        }
    }
}