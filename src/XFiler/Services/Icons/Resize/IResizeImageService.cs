using System.IO;
using System.Windows.Media;

namespace XFiler.Resize;

internal interface IResizeImageService
{
    bool IsSupportExtension(string extension);
                
    ImageSource ResizeImage(string fullName, int size);

    static (int width, int height) GetNewStretchSize(int width, int height, int targetSize) =>
        width > height
            ? (targetSize, height * targetSize / width)
            : (width * targetSize / height, targetSize);

    Task<Stream?> ResizeImageAsync(string fullName, int size);
}