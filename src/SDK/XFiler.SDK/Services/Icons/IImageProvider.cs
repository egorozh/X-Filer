using System.Windows.Media;

namespace XFiler.SDK
{
    public interface IImageProvider
    {
        ImageSource? GetIcon(XFilerRoute? route, int size);
    }
}