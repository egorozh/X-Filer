using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;

namespace XFiler.SDK
{
    internal class IconLoader : IIconLoader
    {
        private readonly Dictionary<string, ImageSource> _cash = new();

        private readonly IIconPathProvider _iconPathProvider;

        public IconLoader(IIconPathProvider iconPathProvider)
        {
            _iconPathProvider = iconPathProvider;
        }

        public ImageSource? GetIcon(FileEntityViewModel viewModel)
        {
            var fileInfo = _iconPathProvider.GetIconPath(viewModel);
            var path = fileInfo.FullName;

            if (_cash.ContainsKey(path))
                return _cash[path];

            ImageSource? source = null;

            if (fileInfo.Extension.ToUpper() == ".SVG")
            {
                var settings = new WpfDrawingSettings
                {
                    TextAsGeometry = false,
                    IncludeRuntime = true,
                };

                var converter = new FileSvgReader(settings);

                var drawing = converter.Read(path);

                if (drawing != null)
                {
                    source = new DrawingImage(drawing);
                    _cash.Add(path, source);
                }
                   
            }
            else
            {
                source = new BitmapImage(new Uri(path));
                _cash.Add(path, source);
            }

            return source;
        }
    }
}