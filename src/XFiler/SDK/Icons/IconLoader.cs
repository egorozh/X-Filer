using System.Collections.Generic;
using System.Windows.Media;

namespace XFiler.SDK
{
    internal class IconLoader : IIconLoader
    {
        private readonly IEnumerable<IImageProvider> _imageProviders;

        public IconLoader(IEnumerable<IImageProvider> imageProviders)
        {
            _imageProviders = imageProviders;
        }

        public ImageSource? GetIcon(XFilerRoute route, double size)
        {
            ImageSource? source = null;

            foreach (var imageProvider in _imageProviders)
            {
                source = imageProvider.GetIcon(route, size);

                if (source == null)
                    continue;

                break;
            }

            return source;
        }

        public ImageSource? GetIcon(FileEntityViewModel viewModel, double size)
        {
            ImageSource? source = null;

            foreach (var imageProvider in _imageProviders)
            {
                source = imageProvider.GetIcon(viewModel, size);

                if (source == null)
                    continue;

                break;
            }

            return source;
        }
    }
}