using System.IO;
using System.Windows.Media;

namespace XFiler;

internal sealed class BlankIconProvider : IIconProvider
{
    public ImageSource? GetIcon(Route? route, IconSize size)
        => Application.Current.TryFindResource(IconName.Blank) as ImageSource;

    public async Task<Stream?> GetIconStream(Route? route, IconSize size)
    {
        return null;
    }
}