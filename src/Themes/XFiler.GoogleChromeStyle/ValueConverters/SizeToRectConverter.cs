namespace XFiler.GoogleChromeStyle.ValueConverters;

internal class SizeToRectConverter : BaseMultiValueConverter
{
    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        => values.Length == 2
            ? new Rect(new Point(), new Point((double) values[0], (double) values[1]))
            : new Rect();
}