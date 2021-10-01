using System;
using System.Windows;

namespace XFiler.GoogleChromeStyle;

internal class WindowResizeFixerBehavior : AttachableForStyleBehavior<Window, WindowResizeFixerBehavior>
{
    private WindowResizer _fixer;

    protected override void OnAttached()
    {
        base.OnAttached();

        _fixer = new WindowResizer(AssociatedObject);

        UpdateBorderThickness();
        UpdateCornerRadius();
        UpdatePadding();

        AssociatedObject.Loaded += (s, e) => UpdatePadding();

        _fixer.WindowDockChanged += FixerOnWindowDockChanged;

        AssociatedObject.StateChanged += AssociatedObject_StateChanged;
    }

    private void AssociatedObject_StateChanged(object? sender, EventArgs e)
    {
        UpdateBorderThickness();
        UpdateCornerRadius();
        UpdatePadding();
    }

    private void FixerOnWindowDockChanged(WindowDockPosition dockPosition)
    {
        if (dockPosition == WindowDockPosition.Undocked)
        {
            if (AssociatedObject.WindowState != WindowState.Maximized)
            {
                AssociatedObject.BorderThickness = new Thickness(10);
                WindowProperties.SetCornerRadius(AssociatedObject, new CornerRadius(8));
            }
        }
        else
        {
            AssociatedObject.BorderThickness = new Thickness(0);
            WindowProperties.SetCornerRadius(AssociatedObject, new CornerRadius(0));
        }
    }

    private void UpdateBorderThickness()
    {
        AssociatedObject.BorderThickness = AssociatedObject.WindowState == WindowState.Normal
            ? new Thickness(10)
            : new Thickness(0);
    }

    private void UpdatePadding()
    {
        AssociatedObject.Padding = AssociatedObject.WindowState == WindowState.Maximized
            ? _fixer.CurrentMonitorMargin
            : new Thickness(0);
    }

    private void UpdateCornerRadius()
    {
        var corners = AssociatedObject.WindowState == WindowState.Normal
            ? new CornerRadius(8)
            : new CornerRadius(0);
         
        WindowProperties.SetCornerRadius(AssociatedObject, corners);
    }
}