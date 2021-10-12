using System.IO;
using System.Windows.Media;
using XFiler.Resize;

namespace XFiler;

internal sealed class IconProviderForImages : IIconProvider
{
    private readonly IResizeImageService _resizeImageService;
    
    public IconProviderForImages(IResizeImageService resizeImageService)
    {
        _resizeImageService = resizeImageService;
    }

    public ImageSource? GetIcon(Route? route, IconSize size)
    {
        if (route is FileRoute fileRoute)
        {
            var fileInfo = fileRoute.File;

            var ext = fileInfo.Extension.ToLower();

            if (_resizeImageService.IsSupportExtension(fileInfo.Extension))
                return _resizeImageService.ResizeImage(fileInfo.FullName, (int)size);
        }

        return null;
    }

    public async Task<Stream?> GetIconStream(Route? route, IconSize size)
    {
        if (route is FileRoute fileRoute)
        {
            var fileInfo = fileRoute.File;
            
            if (_resizeImageService.IsSupportExtension(fileInfo.Extension))
                return await _resizeImageService.ResizeImageAsync(fileInfo.FullName, (int)size);
        }

        return null;
    }
}