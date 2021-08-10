using System.Windows.Media;

namespace XFiler.SDK
{
    public interface IIconLoader
    {
        ImageSource? GetIcon(XFilerRoute route, double size);
    }
}