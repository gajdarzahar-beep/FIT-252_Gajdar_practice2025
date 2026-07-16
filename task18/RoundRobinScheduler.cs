using System.Collections.Concurrent;
using System.Collections.Generic;
using task17;

namespace task18;

public class RoundRobinScheduler : IScheduler
{
    private readonly ConcurrentQueue<ICommand> _queue;
    private readonly List<IExtendedCommand> _longRunningCommands;
    private int _currentIndex;

    public RoundRobinScheduler()
    {
        _queue = new ConcurrentQueue<ICommand>();
        _longRunningCommands = new List<IExtendedCommand>();
        _currentIndex = 0;
    }

    public void Add(ICommand cmd)
    {
        if (cmd is IExtendedCommand extendedCmd)
        {
            if (extendedCmd.IsCompleted)
            {
                return;
            }
            _longRunningCommands.Add(extendedCmd);
        }
        else
        {
            _queue.Enqueue(cmd);
        }
    }

    public bool HasCommand()
    {
        return !_queue.IsEmpty || _longRunningCommands.Count > 0;
    }

    public ICommand Select()
    {
        if (_queue.TryDequeue(out ICommand cmd))
        {
            return cmd;
        }

        if (_longRunningCommands.Count > 0)
        {
            _longRunningCommands.RemoveAll(c => c.IsCompleted);

            if (_longRunningCommands.Count == 0)
            {
                return null;
            }

            if (_currentIndex >= _longRunningCommands.Count)
            {
                _currentIndex = 0;
            }

            var result = _longRunningCommands[_currentIndex];
            _currentIndex++;

            if (_currentIndex >= _longRunningCommands.Count)
            {
                _currentIndex = 0;
            }

            return result;
        }

        return null;
    }
}