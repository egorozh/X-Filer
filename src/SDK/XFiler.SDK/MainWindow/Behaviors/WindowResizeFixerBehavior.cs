using System;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace XFiler.SDK
{
    internal class WindowResizeFixerBehavior : Behavior<Window>
    {
        private WindowResizer _fixer;

        protected override void OnAttached()
        {
            base.OnAttached();

            _fixer = new WindowResizer(AssociatedObject);

            AssociatedObject.StateChanged += (s, e) => UpdatePadding();

            AssociatedObject.Loaded += (s, e) => UpdatePadding();

            _fixer.WindowDockChanged += FixerOnWindowDockChanged;

            AssociatedObject.StateChanged += AssociatedObject_StateChanged;
        }

        private void AssociatedObject_StateChanged(object sender, EventArgs e)
        {
            switch (AssociatedObject.WindowState)
            {
                case WindowState.Normal:
                    AssociatedObject.Margin = new Thickness(10);
                    break;
                case WindowState.Maximized:
                    AssociatedObject.Margin = new Thickness(0);
                    break;
            }
        }

        private void FixerOnWindowDockChanged(WindowDockPosition dockPosition)
        {
            switch (dockPosition)
            {
                case WindowDockPosition.Undocked:
                    if (AssociatedObject.WindowState != WindowState.Maximized)
                        AssociatedObject.Margin = new Thickness(10);
                    break;
                default:
                    AssociatedObject.Margin = new Thickness(0);
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