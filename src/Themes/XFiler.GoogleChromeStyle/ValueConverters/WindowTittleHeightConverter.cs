namespace XFiler.GoogleChromeStyle.ValueConverters;

internal class WindowTitleHeightConverter : BaseValueConverter
{
    public override object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        if (value is WindowState windowState)
            return windowState == WindowState.Normal ? 42 : 32;

        return 42;
    }
}