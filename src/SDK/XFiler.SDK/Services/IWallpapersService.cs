using System.Windows.Media;

namespace XFiler.SDK;

public interface IWallpapersService
{
    ImageSource? CreateImageSource(string? imagePath);
}