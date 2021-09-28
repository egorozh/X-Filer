using System;
using XFiler.SDK.Themes;

namespace XFiler.BaseStyle
{
    public class BaseStyleTheme : XFilerTheme
    {
        public override Uri GetResourceUri() =>
            new("/XFiler.BaseStyle;component/Themes/ExplorerBaseTheme.xaml", UriKind.Relative);

        public override string GetId() => "e394f339-5907-4c5f-9113-6e49368b3d22";
    }
}