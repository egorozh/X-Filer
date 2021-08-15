using System;
using System.Collections;
using System.Globalization;
using XFiler.SDK;

namespace XFiler.Base
{
    internal class IsManySelectedItems : BaseValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection enumerable)
                return enumerable.Count > 1;

            return false;
        }
    }
}