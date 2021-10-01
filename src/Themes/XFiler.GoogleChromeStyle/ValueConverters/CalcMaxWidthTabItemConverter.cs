using System;
using System.Globalization;
using XFiler.SDK;

namespace XFiler.GoogleChromeStyle.ValueConverters;

public class CalcMaxWidthTabItemConverter : BaseMultiValueConverter
{
    private const double SystemButtonsWidth = 44 * 3;

    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 2 &&
            values[0] is double controlWidth &&
            values[1] is int itemsCount)
        {
            var newWidth = (controlWidth - SystemButtonsWidth - 10.0) / itemsCount;
            if (newWidth < 256.0)
                return newWidth;
        }

        return 256.0;
    }
}