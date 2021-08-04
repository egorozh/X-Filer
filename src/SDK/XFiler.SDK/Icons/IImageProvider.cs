using System.Windows.Media;

namespace XFiler.SDK
{
    public interface IImageProvider
    {
        ImageSource? GetIcon(FileEntityViewModel viewModel, double size);

        ImageSource? GetIcon(IMenuItemViewModel viewModel, double size);
    }
}