using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using Serilog;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using XFiler.SDK;

namespace XFiler.Base.Icons
{
    internal sealed class SvgIconProvider : IIconProvider
    {
        private readonly ILogger _logger;

        public SvgIconProvider(ILogger logger)
        {
            _logger = logger;
        }

        public ImageSource? GetIcon(XFilerRoute? route, IconSize size)
        {
            if (route is FileRoute fileRoute)
            {
                if (fileRoute.File.Extension.ToLower() == ".svg")
                {
                    try
                    {
                        return CreateImageSourceFromSvg(fileRoute.File.FullName);
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e, "SvgIconProvider error");
                    }
                }
                    
            }

            return null;
        }

        public async Task<Stream?> GetIconStream(XFilerRoute? route, IconSize size)
        {
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