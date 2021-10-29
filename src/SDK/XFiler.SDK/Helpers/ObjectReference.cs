using System;
using System.Windows;

namespace XFiler.SDK;

public class ObjectReference : Freezable
{
    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        "Value", typeof(object), typeof(ObjectReference),
        new PropertyMetadata(default(object), ValueChangedCallback));

    private static void ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ObjectReference objectReference)
            objectReference.InvokeValueChanged();
    }
    
    public object Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public event EventHandler? ValueChanged;

    #region Freezable

    protected override Freezable CreateInstanceCore() => throw new NotImplementedException();

    #endregion

    private void InvokeValueChanged() => ValueChanged?.Invoke(this, EventArgs.Empty);
}