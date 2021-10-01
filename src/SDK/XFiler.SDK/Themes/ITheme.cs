using System;

namespace XFiler.SDK.Themes;

public interface ITheme
{
    string Id { get; }

    Uri ResourceUri { get; }
}