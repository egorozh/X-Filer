using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.ImageOperations;

namespace XFiler;

internal class IconProviderForIcons : IIconProvider
{
    private readonly PngEncoder _pngFormat = new();
    private readonly IReadOnlyList<string> _supportedSixLaborsformats;

    public IconProviderForIcons()
    {
        _supportedSixLaborsformats = Configuration.Default.ImageFormats
            .SelectMany(f => f.FileExtensions)
            .ToList();
    }

    public ImageSource? GetIcon(XFilerRoute? route, int size)
    {
        if (route == null)
            return null;

        if (route.Type == RouteType.File)
        {
            var fileInfo = new FileInfo(route.FullName);

            var ext = fileInfo.Extension.ToLower();

            if (_supportedSixLaborsformats.Contains(ext[1..]))
                return GetResizeImage(fileInfo.FullName, size);

            if (ext == ".ico")
                return new BitmapImage(new Uri(route.FullName));
        }

        return null;
    }

    private BitmapImage GetResizeImage(string fullName, int size)
    {
        using var image = Image.Load(fullName);

        var (width, height) = GetNewStretchSize(image.Width, image.Height, size);

        image.Mutate(x => x.Resize(width, height, new NearestNeighborResampler()));

        using var ms = new MemoryStream();

        image.Save(ms, _pngFormat);

        return ImageSystem.FromStream(ms);
    }

    private static (int width, int height) GetNewStretchSize(int width, int height, int targetSize) => width > height
        ? (targetSize, height * targetSize / width)
        : (width * targetSize / height, targetSize);
}