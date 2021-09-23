using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using System.IO;
using System.Linq;
using System.Windows.Media;
using Windows.ImageOperations;

namespace XFiler.Resize
{
    internal sealed class SixLaborsResizeImageService : IResizeImageService
    {
        private readonly PngEncoder _pngFormat = new();
        private readonly IReadOnlyList<string> _supportedSixLaborsformats;

        public SixLaborsResizeImageService()
        {
            _supportedSixLaborsformats = Configuration.Default.ImageFormats
                .SelectMany(f => f.FileExtensions)
                .ToList();
        }

        public bool IsSupportExtension(string extension)
            => _supportedSixLaborsformats.Contains(extension.ToLower()[1..]);

        public ImageSource ResizeImage(string fullName, int size)
        {
            using var image = Image.Load(fullName);

            var (width, height) = IResizeImageService.GetNewStretchSize(image.Width, image.Height, size);

            image.Mutate(x => x.Resize(width, height, new NearestNeighborResampler()));

            using var ms = new MemoryStream();

            image.Save(ms, _pngFormat);

            return ImageSystem.FromStream(ms);
        }
    }
}