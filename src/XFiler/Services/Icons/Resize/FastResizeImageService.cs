using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Media;
using Windows.ImageOperations;

namespace XFiler.Resize;

internal sealed class FastResizeImageService : IResizeImageService
{
    private readonly IReadOnlyList<string> _supportedExtensions;

    public FastResizeImageService()
    {
        _supportedExtensions = ImageCodecInfo.GetImageDecoders()
            .SelectMany(d =>
                d.FilenameExtension.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            .Select(ext => ext[1..])
            .ToArray();
    }

    public bool IsSupportExtension(string extension)
        => _supportedExtensions.Contains(extension.ToUpper());

    public ImageSource ResizeImage(string fullName, int size)
    {
        using var image = Image.FromFile(fullName);

        var (width, height) = IResizeImageService.GetNewStretchSize(image.Width, image.Height, size);

        Bitmap b = new(width, height);

        using Graphics g = Graphics.FromImage(b);

        g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;

        g.DrawImage(image, 0, 0, width, height);

        return b.ToBitmapImage();
    }

    public async Task<Stream?> ResizeImageAsync(string fullName, int size)
    {
        return await Task.Run(() =>
        {
            using var image = Image.FromFile(fullName);

            var (width, height) = IResizeImageService.GetNewStretchSize(image.Width, image.Height, size);

            Bitmap b = new(width, height);

            using Graphics g = Graphics.FromImage(b);

            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;

            g.DrawImage(image, 0, 0, width, height);

            return b.ToStream();
        });
    }
}