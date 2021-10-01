namespace XFiler;

internal sealed class MenuItemIsReadonlyConverter : BaseValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IMenuItemViewModel menuItem)
            return menuItem.Route != null;

        return true;
    }
}