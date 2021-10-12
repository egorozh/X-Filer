using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;

namespace XFiler.SDK;

public interface IIconLoader
{
    ImageSource? GetIcon(Route? route, IconSize size);

    Task<Stream?> GetIconStream(Route? route, IconSize size);
}