using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using XFiler.SDK;

namespace XFiler.ValueConverters
{
    internal class IconPathToImageConverter : BaseValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var image = new Image();
            image.Stretch = Stretch.Uniform;

            if (value is string imagePaths)
            {
                var imagePath = new FileInfo(imagePaths);

                if (imagePath.Extension.ToUpper() == ".SVG")
                {
                    var settings = new WpfDrawingSettings
                    {
                        TextAsGeometry = false,
                        IncludeRuntime = true,
                    };

                    var converter = new FileSvgReader(settings);

                    var drawing = converter.Read(imagePath.FullName);

                    if (drawing != null)
                    {
                        var drawImage = new DrawingImage(drawing);
                        image.Source = drawImage;
                    }
                }
                else
                {
                    var bitmapSource = new BitmapImage(new Uri(imagePath.FullName));
                    image.Source = bitmapSource;
                }
            }

            return image;
        }
    }
}