using System.Windows.Media;

namespace XFiler.SDK
{
    public interface IIconProvider
    {
        ImageSource? GetIcon(XFilerRoute? route, IconSize size);
    }
}