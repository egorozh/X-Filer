using System.Windows.Media;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using XFiler.SDK;

namespace XFiler.Base.Icons
{
    internal sealed class SvgIconProvider : IIconProvider
    {
        public ImageSource? GetIcon(XFilerRoute? route, IconSize size)
        {
            if (route is FileRoute fileRoute)
            {
                if (fileRoute.File.Extension.ToLower() == ".svg")
                    return CreateImageSourceFromSvg(fileRoute.File.FullName);
            }

            return null;
        }

        private static DrawingImage? CreateImageSourceFromSvg(string filePath)
        {
            WpfDrawingSettings settings = new()
            {
                IncludeRuntime = true,
                TextAsGeometry = false,
            };
 
            FileSvgReader converter = new(settings);
            
            var drawing = converter.Read(filePath);

            if (drawing != null)
            {
                DrawingImage imageSource = new(drawing);

                return imageSource;
            }

            return null;
        }
    }
}