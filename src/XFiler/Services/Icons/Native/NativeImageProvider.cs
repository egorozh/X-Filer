using System.IO;
using System.Windows.Media;

namespace XFiler
{
    public class NativeImageProvider : IImageProvider
    {
        public ImageSource? GetIcon(XFilerRoute? route, int size)
        {
            if (route is { Type: RouteType.File})
            {
                var info = new FileInfo(route.FullName);

                if (info.Extension.ToLower() == ".exe")
                    return NativeIconResolver.GetIcon(route.FullName);
            }

            return null;
        }
    }
}