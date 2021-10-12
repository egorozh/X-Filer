﻿using System.IO;
using System.Windows.Media;
using Windows.ImageOperations;

namespace XFiler;

internal sealed class NativeFileIconProvider : IIconProvider
{
    public ImageSource? GetIcon(Route? route, IconSize size) => route is { Type: RouteType.File }
        ? ImageSystem.GetIcon(route.FullName, size != IconSize.Small)
        : null;

    public async Task<Stream?> GetIconStream(Route? route, IconSize size)
    {
        return null;
    }
}