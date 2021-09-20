using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace XFiler.Helpers
{
    internal static class ImageExtensions
    {
        public static BitmapImage ToBitmapImage(this Icon icon)
        {
            using var ms = new MemoryStream();

            icon.ToBitmap().Save(ms, ImageFormat.Png);
           
            return FromStream(ms);
        }

        public static BitmapImage FromStream(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}
