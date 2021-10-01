using System;
using System.Globalization;
using System.Windows;
using XFiler.SDK;

namespace XFiler.GoogleChromeStyle.ValueConverters;

public class BoolToVisibilityConverter : BaseValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isVisible)
            return isVisible ? Visibility.Visible : Visibility.Collapsed;

        return Visibility.Collapsed;
    }
}