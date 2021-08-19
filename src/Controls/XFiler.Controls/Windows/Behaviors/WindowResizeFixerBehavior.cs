using System;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace XFiler.Controls.Windows.Behaviors
{
    internal class WindowResizeFixerBehavior : Behavior<Window>
    {
        private WindowResizer _fixer;

        protected override void OnAttached()
        {
            base.OnAttached();

            _fixer = new WindowResizer(AssociatedObject);

            AssociatedObject.BorderThickness = AssociatedObject.WindowState == WindowState.Normal
                ? new Thickness(10)
                : new Thickness(0);

            AssociatedObject.StateChanged += (s, e) => UpdatePadding();

            AssociatedObject.Loaded += (s, e) => UpdatePadding();

            _fixer.WindowDockChanged += FixerOnWindowDockChanged;

            AssociatedObject.StateChanged += AssociatedObject_StateChanged;
        }

        private void AssociatedObject_StateChanged(object? sender, EventArgs e)
        {
            AssociatedObject.BorderThickness = AssociatedObject.WindowState == WindowState.Normal
                ? new Thickness(10)
                : new Thickness(0);
        }

        private void FixerOnWindowDockChanged(WindowDockPosition dockPosition)
        {
            switch (dockPosition)
            {
                case WindowDockPosition.Undocked:
                    if (AssociatedObject.WindowState != WindowState.Maximized)
                        AssociatedObject.BorderThickness = new Thickness(10);
                    break;
                default:
                    AssociatedObject.BorderThickness = new Thickness(0);
                    break;
            }
        }

        private void UpdatePadding()
        {
            AssociatedObject.Padding = AssociatedObject.WindowState == WindowState.Maximized
                ? _fixer.CurrentMonitorMargin
                : new Thickness(0);
        }
    }
}