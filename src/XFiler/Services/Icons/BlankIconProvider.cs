using System.Windows.Media;

namespace XFiler
{
    internal sealed class BlankIconProvider : IIconProvider
    {
        public ImageSource? GetIcon(XFilerRoute? route, IconSize size)
            => Application.Current.TryFindResource(IconName.Blank) as ImageSource;
    }
}