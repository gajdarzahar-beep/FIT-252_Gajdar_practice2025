using System;
using System.Threading;
using task17;

namespace task18;

public class ServerThread
{
    private readonly Thread _thread;
    private readonly IScheduler _scheduler;
    private volatile bool _isStopped;

    public ServerThread(IScheduler scheduler)
    {
        _scheduler = scheduler;
        _isStopped = false;
        _thread = new Thread(Run);
        _thread.Start();
    }

    public void AddCommand(ICommand command)
    {
        _scheduler.Add(command);
    }

    public void Stop()
    {
        _isStopped = true;
        _thread.Join();
    }

    private void Run()
    {
        while (!_isStopped)
        {
            if (_scheduler.HasCommand())
            {
                ICommand command = _scheduler.Select();
                if (command != null)
                {
                    try
                    {
                        command.Execute();
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }
            else
            {
                Thread.Sleep(10);
            }
        }
    }
}