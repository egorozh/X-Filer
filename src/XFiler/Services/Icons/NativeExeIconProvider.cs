using System.IO;
using System.Windows.Media;
using Windows.ImageOperations;

namespace XFiler
{
    internal class NativeExeIconProvider : IIconProvider
    {
        public ImageSource? GetIcon(XFilerRoute? route, int size)
        {   
            if (route is { Type: RouteType.File })
            {
                var info = new FileInfo(route.FullName);

                if (info.Extension.ToLower() == ".exe")
                    return ImageSystem.GetIcon(route.FullName);
            }

            return null;
        }
    }
}