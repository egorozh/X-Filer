using System;
using System.Windows;

namespace XFiler.SDK.Themes
{
    public abstract class XFilerTheme : DependencyObject, ITheme
    {
        public string Guid => GetGuid();
        
        public abstract Uri GetResourceUri();

        public abstract string GetGuid();
    }
}   