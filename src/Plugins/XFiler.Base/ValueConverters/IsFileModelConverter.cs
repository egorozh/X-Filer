namespace XFiler.Base;

internal sealed class IsFileModelConverter : BaseValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IFileSystemModel model)
            return model.Info is FileInfo;

        return false;
    }
}