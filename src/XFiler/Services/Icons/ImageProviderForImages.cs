using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace XFiler;

public class ImageProviderForImages : IImageProvider
{
    public ImageSource? GetIcon(XFilerRoute? route, int size)
    {
        if (route == null)
            return null;

        if (route.Type == RouteType.File)
        {
            var fileInfo = new FileInfo(route.FullName);

            var ext = fileInfo.Extension.ToLower();

            switch (ext)
            {
                case ".jpg":
                case ".png":
                case ".bmp":

                    using (Image image = Image.Load(fileInfo.FullName, out var format))
                    {
                        image.Mutate(x => x.Resize(size, size));

                        using (var ms = new MemoryStream())
                        {
                            image.Save(ms, format);
                          
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
        }

        return null;
    }
}