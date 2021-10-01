using System.Windows.Input;

namespace XFiler.SDK;

public interface ICommandFeature
{
    ICommand Command { get; }
}