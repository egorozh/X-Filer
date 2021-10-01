using System.Windows.Media;

namespace XFiler;

internal sealed class ImageBrushToViewportConverter : BaseValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ImageSource imageSource)
            return new Rect(new Point(0, 0), new Size(imageSource.Width, imageSource.Height));

        return new Rect();
    }
}