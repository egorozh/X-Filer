using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace XFiler.SDK;

public class CommandReference : Freezable, ICommand
{
    #region Dependency Properties

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command), typeof(ICommand), typeof(CommandReference),
        new PropertyMetadata(OnCommandChanged));

    private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not CommandReference commandReference)
            return;

        var oldCommand = e.OldValue as ICommand;
        var newCommand = e.NewValue as ICommand;

        commandReference.OnCommandChanged(oldCommand, newCommand);
    }

    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
        nameof(CommandParameter), typeof(object), typeof(CommandReference),
        new PropertyMetadata(default(object)));

    #endregion

    #region Public Properties

    public ICommand? Command
    {
        get => (ICommand) GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    #endregion

    #region ICommand Members

    public bool CanExecute(object? parameter)
        => Command != null && Command.CanExecute(GetParameter(parameter, CommandParameter));

    public void Execute(object? parameter)
        => Command?.Execute(GetParameter(parameter, CommandParameter));

    public event EventHandler? CanExecuteChanged;

    #endregion

    #region Private Methods

    private static object? GetParameter(object? mainParameter, object? referenceParameter)
    {
        if (mainParameter == null)
            return referenceParameter;

        if (referenceParameter == null)
            return mainParameter;

        List<object> mergesParameters = new();

        if (referenceParameter is object[] arrayParams2)
            mergesParameters.AddRange(arrayParams2);
        else
            mergesParameters.Add(referenceParameter);

        if (mainParameter is object[] arrayParams)
            mergesParameters.AddRange(arrayParams);
        else
            mergesParameters.Add(mainParameter);

        return mergesParameters.ToArray();
    }

    private void OnCommandChanged(ICommand? oldCommand, ICommand? newCommand)
    {
        if (oldCommand != null)
            oldCommand.CanExecuteChanged -= OldCommandOnCanExecuteChanged;

        if (newCommand != null)
            newCommand.CanExecuteChanged += OldCommandOnCanExecuteChanged;
    }

    private void OldCommandOnCanExecuteChanged(object? sender, EventArgs e) => RaiseCanExecuteChanged();

    private void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    #endregion

    #region Freezable

    protected override Freezable CreateInstanceCore() => throw new NotImplementedException();

    #endregion
}