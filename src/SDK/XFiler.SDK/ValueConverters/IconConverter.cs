using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;

namespace XFiler.SDK;

public class IconConverter : BaseMultiValueConverter
{
    public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is IMenuItemViewModel menuItemView &&
            values[1] is IIconLoader iconLoader)
        {
            return new Image
            {
                Source = iconLoader.GetIcon(menuItemView.Route, IconSize.Small),
                Stretch = Stretch.Uniform
            };
        }
            
        return null!;
    }
}