using System;
using System.Globalization;
using XFiler.SDK;

namespace XFiler.GoogleChromeStyle.ValueConverters;

internal class PageIsExlporerPageModelConverter : BaseValueConverter
{   
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        => value is IExplorerPageModel;
}