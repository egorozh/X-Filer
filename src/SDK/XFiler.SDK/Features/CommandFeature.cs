using System.Windows.Input;
using Prism.Commands;

namespace XFiler.SDK;

public abstract class CommandFeature<T> : Feature, ICommandFeature
{
    private readonly DelegateCommand<T> _command;

    public ICommand Command => _command;

    protected CommandFeature()
    {
        _command = new DelegateCommand<T>(ExecuteInternal, CanExecuteInternal);
    }

    private bool CanExecuteInternal(T arg) => CanExecute(arg);

    private void ExecuteInternal(T arg) => Execute(arg);

    public virtual bool CanExecute(T arg) => true;

    public abstract void Execute(T arg);

    protected void Update()
    {
        _command.RaiseCanExecuteChanged();
    }
}