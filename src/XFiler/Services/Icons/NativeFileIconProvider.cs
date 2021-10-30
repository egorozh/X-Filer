using System.IO;
using System.Windows.Media;
using Windows.ImageOperations;

namespace XFiler;

internal sealed class NativeFileIconProvider : IIconProvider
{
    public ImageSource? GetIcon(Route? route, IconSize size)
    {
        if (route is {Type: RouteType.File})
        {
            var icon = Win32Api.GetFileIcon(route.FullName, (int) size);

            if (icon != null)
                return icon.ToBitmapImage();
        }

        return null;
    }

    public async Task<Stream?> GetIconStream(Route? route, IconSize size)
    {
        return null;
    }
}