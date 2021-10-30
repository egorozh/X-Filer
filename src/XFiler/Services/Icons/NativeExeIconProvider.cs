using System.IO;
using System.Windows.Media;
using Windows.ImageOperations;

namespace XFiler;

internal sealed class NativeExeIconProvider : IIconProvider
{
    public ImageSource? GetIcon(Route? route, IconSize size)
    {
        if (route is FileRoute fileRoute)
        {
            var info = fileRoute.File;

            if (info.Extension.ToLower() == ".exe")
            {
                var icon = Win32Api.GetFileIcon(info.FullName, (int) size);
   
                if (icon != null)
                    return icon.ToBitmapImage();
            }
        }

        return null;
    }

    public async Task<Stream?> GetIconStream(Route? route, IconSize size)
    {
        return null;
    }
}