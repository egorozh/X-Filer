using System;
using System.Globalization;

namespace XFiler.SDK;

public class CloneParamsConverter : BaseMultiValueConverter
{
    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        => values.Clone();
}