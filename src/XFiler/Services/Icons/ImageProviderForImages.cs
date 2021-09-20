using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SixLabors.ImageSharp.Formats;
using ResizeMode = SixLabors.ImageSharp.Processing.ResizeMode;

namespace XFiler;

public class ImageProviderForImages : IImageProvider
{
    private SixLabors.ImageSharp.Formats.Png.PngFormat _pngFormat
        = SixLabors.ImageSharp.Formats.Png.PngFormat.Instance;

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

                    using (Image image = Image.Load(fileInfo.FullName))
                    {
                        var (width, height) = GetNewStretchSize(image.Width, image.Height, size);
                        
                        image.Mutate(x => x.Resize(width, height));

                        using var ms = new MemoryStream();

                        image.Save(ms, _pngFormat);

                        return XFiler.Helpers.ImageExtensions.FromStream(ms);
                    }

                case ".ico":

                    return new BitmapImage(new Uri(route.FullName));
            }
        }

        return null;
    }

    private static (int width, int height) GetNewStretchSize(int width, int height, int targetSize) => width > height
        ? (targetSize, height * targetSize / width)
        : (width * targetSize / height, targetSize);
}