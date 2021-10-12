using XFiler.SDK.Themes;

namespace XFiler.GoogleChromeStyle;

public class GoogleChromeTheme : XFilerTheme
{
    private const string Win11GenericPath = "/XFiler.GoogleChromeStyle;component/Themes/Generic11.xaml";

    public override Uri GetResourceUri() => new (Win11GenericPath, UriKind.Relative);

    public override string GetId() => "e394f339-5907-4c5f-9113-6e49368b3d22";
}