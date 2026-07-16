using task17;

namespace task18;

public interface IExtendedCommand : ICommand
{
    bool IsCompleted { get; }
}