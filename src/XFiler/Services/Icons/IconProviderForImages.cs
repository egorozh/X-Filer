using System.Windows.Media;
using System.Windows.Media.Imaging;
using XFiler.Resize;

namespace XFiler;

internal sealed class IconProviderForImages : IIconProvider
{
    private readonly IResizeImageService _resizeImageService;
    
    public IconProviderForImages(IResizeImageService resizeImageService)
    {
        _resizeImageService = resizeImageService;
    }

    public ImageSource? GetIcon(XFilerRoute? route, IconSize size)
    {
        if (route is FileRoute fileRoute)
        {
            var fileInfo = fileRoute.File;

            var ext = fileInfo.Extension.ToLower();

            if (_resizeImageService.IsSupportExtension(fileInfo.Extension))
                return _resizeImageService.ResizeImage(fileInfo.FullName, (int)size);

            if (ext == ".ico")
                return new BitmapImage(new Uri(route.FullName));
        }

        return null;
    }
}