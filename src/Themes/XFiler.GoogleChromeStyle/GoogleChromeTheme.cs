using System;
using XFiler.SDK.Themes;

namespace XFiler.GoogleChromeStyle
{
    public class GoogleChromeTheme : XFilerTheme
    {
        private const string GenericPath = "/XFiler.GoogleChromeStyle;component/Themes/Generic.xaml";
        private const string Win11GenericPath = "/XFiler.GoogleChromeStyle;component/Themes/Generic11.xaml";

        public override Uri GetResourceUri() => IsWindows11() 
            ? new Uri(Win11GenericPath, UriKind.Relative)
            : new Uri(GenericPath, UriKind.Relative);

        public override string GetId() => "e394f339-5907-4c5f-9113-6e49368b3d22";

        private static bool IsWindows11()
            => Environment.OSVersion.Version.Build.ToString().StartsWith("22");
        //=>false;
    }
}