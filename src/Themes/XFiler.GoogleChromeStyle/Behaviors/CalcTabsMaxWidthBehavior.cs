using System.Windows;

namespace XFiler.GoogleChromeStyle;

internal class CalcTabsMaxWidthBehavior : AttachableForStyleBehavior<Window, CalcTabsMaxWidthBehavior>
{
    protected override void OnAttached()
    {
        base.OnAttached();
        CalcTabMaxWidth();
        AssociatedObject.SizeChanged += AssociatedObjectOnSizeChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        AssociatedObject.SizeChanged -= AssociatedObjectOnSizeChanged;
    }

    private void AssociatedObjectOnSizeChanged(object sender, SizeChangedEventArgs e) => CalcTabMaxWidth();

    private void CalcTabMaxWidth()
    {
        var tabsMaxWidth = AssociatedObject.ActualWidth - 24 * 3 - 115 - 34;
        WindowProperties.SetTabsMaxWidth(AssociatedObject, tabsMaxWidth);
    }
}