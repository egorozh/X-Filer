using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace XFiler
{
    public class NativeImageProvider : IImageProvider
    {
        public ImageSource? GetIcon(XFilerRoute? route, int size)
        {
            if (route != null && route.Type == RouteType.File)
            {
                var info = new FileInfo(route.FullName);

                if (info.Extension.ToLower() == ".exe")
                {
                    Icon? icon = Icon.ExtractAssociatedIcon(route.FullName);

                    if (icon != null)
                    {
                        using var ms = new MemoryStream();

                        icon.Save(ms);
                        ms.Seek(0, SeekOrigin.Begin);

                        var bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = ms;
                        bitmapImage.EndInit();

                        return bitmapImage;
                    }
                }
            }

            return null;
        }
    }
}