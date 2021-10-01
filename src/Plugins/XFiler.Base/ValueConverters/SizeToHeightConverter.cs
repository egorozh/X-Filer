namespace XFiler.Base;

internal sealed class SizeToHeightConverter : BaseValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double size)
            return size + 20.0;

        return value;
    }
}