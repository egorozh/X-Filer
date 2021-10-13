using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace XFiler
{
    internal class WallpapersService : IWallpapersService
    {
        private readonly IStorage _storage;

        private Dictionary<string, string> _inlineWallpapers = new()
        {
            { "e8009541-5e2f-4def-adaa-0e682630e222", "1.jpg"},
            { "5bde1d83-9094-4995-8242-ca1c7b56dde7", "2.png"}
        };

        public WallpapersService(IStorage storage)
        {
            _storage = storage;
        }

        public ImageSource? CreateImageSource(string? imagePath)
        {
            if (imagePath != null)
            {
                var fullPath = Path.Combine(_storage.ExplorerWallpapersDirectory, imagePath);

                if (File.Exists(fullPath))
                    return new BitmapImage(new Uri(fullPath));
            }

            return null;
        }
    }

    
}