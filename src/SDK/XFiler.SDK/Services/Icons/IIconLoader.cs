using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;

namespace XFiler.SDK
{
    public interface IIconLoader
    {
        ImageSource? GetIcon(XFilerRoute? route, IconSize size);

        Task<Stream?> GetIconStream(XFilerRoute? route, IconSize size);
    }
}