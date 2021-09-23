using System.Windows.Media;
using Windows.ImageOperations;

namespace XFiler
{
    internal sealed class NativeExeIconProvider : IIconProvider
    {
        public ImageSource? GetIcon(XFilerRoute? route, IconSize size)
        {   
            if (route is FileRoute fileRoute)
            {
                var info = fileRoute.File;

                if (info.Extension.ToLower() == ".exe")
                    return ImageSystem.GetIcon(route.FullName, size != IconSize.Small);
            }

            return null;
        }
    }
}