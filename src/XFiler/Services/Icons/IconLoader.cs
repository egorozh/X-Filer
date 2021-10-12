using System.IO;
using System.Windows.Media;

namespace XFiler;

internal sealed class IconLoader : IIconLoader
{
    private readonly IReadOnlyList<IIconProvider> _imageProviders;

    public IconLoader(IReadOnlyList<IIconProvider> imageProviders)
    {
        _imageProviders = imageProviders;
    }

    public ImageSource? GetIcon(Route? route, IconSize size)
    {
        ImageSource? source = null;

        foreach (var imageProvider in _imageProviders)
        {
            source = imageProvider.GetIcon(route, size);

            if (source == null)
                continue;

            break;
        }

        return source;
    }

    public async Task<Stream?> GetIconStream(Route? route, IconSize size)
    {
        Stream? stream = null;

        foreach (var imageProvider in _imageProviders)
        {
            stream = await imageProvider.GetIconStream(route, size);

            if (stream == null)
                continue;

            break;
        }

        return stream;
    }
}