using Xunit;
using task17;
using System.Threading;

namespace task17tests;

public class ServerThreadTests
{
    [Fact]
    public void ServerThread_AddCommand_ExecutesCommand()
    {
        var serverThread = new ServerThread();
        var command = new TestCommand();

        serverThread.AddCommand(command);
        Thread.Sleep(100);

        Assert.True(command.Executed);
    }

    [Fact]
    public void HardStopCommand_StopsThreadImmediately()
    {
        var serverThread = new ServerThread();
        var hardStop = new HardStop();

        serverThread.AddCommand(hardStop);
        Thread.Sleep(100);

        Assert.Throws<InvalidOperationException>(() => serverThread.AddCommand(new TestCommand()));
    }

    [Fact]
    public void SoftStopCommand_StopsThreadAfterAllCommands()
    {
        var serverThread = new ServerThread();
        var softStop = new SoftStop();

        serverThread.AddCommand(new TestCommand());
        serverThread.AddCommand(softStop);
        serverThread.AddCommand(new TestCommand());

        Thread.Sleep(200);

        Assert.Throws<InvalidOperationException>(() => serverThread.AddCommand(new TestCommand()));
    }

    private class TestCommand : ICommand
    {
        public bool Executed { get; private set; }

        public void Execute()
        {
            Executed = true;
        }
    }
}