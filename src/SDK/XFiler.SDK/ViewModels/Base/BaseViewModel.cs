using System;
using System.ComponentModel;

namespace XFiler.SDK;

public class BaseViewModel : INotifyPropertyChanged
{
    #region Events

    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<ExPropertyChangedEventArgs>? ExPropertyChanged;

    #endregion

    #region Protected Methods

    protected virtual void OnPropertyChanged(string propertyName, object? before, object? after)
    {
        PropertyChanged?.Invoke(this, new ExPropertyChangedEventArgs(propertyName, before, after));
        ExPropertyChanged?.Invoke(this, new ExPropertyChangedEventArgs(propertyName, before, after));
    }

    #endregion
}

public class ExPropertyChangedEventArgs : PropertyChangedEventArgs
{
    public object? OldValue { get; }
    public object? NewValue { get; }

    public ExPropertyChangedEventArgs(string propertyName, object? oldValue, object? newValue)
        : base(propertyName)
    {
        OldValue = oldValue;
        NewValue = newValue;
    }
}