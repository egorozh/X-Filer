using System.Windows;

namespace XFiler.GoogleChromeStyle;

internal class WindowProperties
{
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
        "CornerRadius", typeof(CornerRadius), typeof(WindowProperties),
        new PropertyMetadata(default(CornerRadius)));

    public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        => element.SetValue(CornerRadiusProperty, value);

    public static CornerRadius GetCornerRadius(DependencyObject element)
        => (CornerRadius) element.GetValue(CornerRadiusProperty);

    public static readonly DependencyProperty TabsMaxWidthProperty = DependencyProperty.RegisterAttached(
        "TabsMaxWidth", typeof(double), typeof(WindowProperties), new PropertyMetadata(default(double)));

    public static void SetTabsMaxWidth(DependencyObject element, double value)
        => element.SetValue(TabsMaxWidthProperty, value);

    public static double GetTabsMaxWidth(DependencyObject element)
        => (double) element.GetValue(TabsMaxWidthProperty);
}