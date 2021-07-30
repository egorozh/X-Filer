using System;
using XFiler.SDK.Themes;

namespace XFiler.GoogleChromeStyle
{
    public class GoogleChromeTheme : ExplorerTheme
    {   
        public override Uri GetResourceUri() =>
            new("/XFiler.GoogleChromeStyle;component/Themes/Generic.xaml", 
                UriKind.Relative);

        public override string GetGuid() => "e394f339-5907-4c5f-9113-6e49368b3d22";
    }
}