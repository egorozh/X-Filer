using System;

namespace XFiler.SDK.Themes
{
    public interface ITheme
    {
        string Guid { get; }
        
        Uri GetResourceUri();
    }
}