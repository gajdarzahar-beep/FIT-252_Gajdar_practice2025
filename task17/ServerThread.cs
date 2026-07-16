using System;
using System.Collections.Concurrent;
using System.Threading;

namespace task17;

public class ServerThread
{
    private readonly Thread _thread;
    private readonly BlockingCollection<ICommand> _queue;
    private volatile bool _isHardStopped;
    private volatile bool _isSoftStopped;

    public ServerThread()
    {
        _queue = new BlockingCollection<ICommand>(new ConcurrentQueue<ICommand>());
        _isHardStopped = false;
        _isSoftStopped = false;
        _thread = new Thread(Run);
        _thread.Start();
    }

    public void AddCommand(ICommand command)
    {
        if (_isHardStopped)
        {
            throw new InvalidOperationException("Thread is hard stopped");
        }

        _queue.Add(command);
    }

    public void Stop()
    {
        _queue.CompleteAdding();
        _thread.Join();
    }

    private void Run()
    {
        while (!_isHardStopped && !_isSoftStopped)
        {
            try
            {
                ICommand command = _queue.Take();

                if (command is HardStop)
                {
                    _isHardStopped = true;
                    break;
                }

                if (command is SoftStop)
                {
                    _isSoftStopped = true;
                    while (_queue.TryTake(out ICommand cmd))
                    {
                        try
                        {
                            cmd.Execute();
                        }
                        catch (Exception)
                        {
                            
                        }
                    }
                    _queue.CompleteAdding();  
                    break;
                }

                try
                {
                    command.Execute();
                }
                catch (Exception)
                {
                    
                }
            }
            catch (InvalidOperationException)
            {
                break;
            }
        }
    }
}