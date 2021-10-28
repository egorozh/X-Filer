using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;

namespace XFiler.Base.Icons;

internal sealed class SvgIconProvider : IIconProvider
{
    private readonly ILogger _logger;

    public SvgIconProvider(ILogger logger)
    {
        _logger = logger;
    }

    public ImageSource? GetIcon(Route? route, IconSize size)
    {
        if (route is FileRoute fileRoute)
        {
            if (fileRoute.File.Extension.ToLower() == ".svg")
            {
                var svgPath = fileRoute.File.FullName;

                try
                {
                    return CreateImageSourceFromSvg(svgPath);
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"SvgIconProvider error for path:\"{svgPath}\"");
                }
            }
        }

        return null;
    }

    public async Task<Stream?> GetIconStream(Route? route, IconSize size)
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

        var drawing = converter.Read(new Uri(filePath));

        return drawing != null 
            ? new DrawingImage(drawing) 
            : null;
    }
}