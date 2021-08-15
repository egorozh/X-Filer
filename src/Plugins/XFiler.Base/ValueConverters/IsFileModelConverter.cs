using System;
using System.Globalization;
using System.IO;
using XFiler.SDK;

namespace XFiler.Base
{
    internal class IsFileModelConverter : BaseValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IFileSystemModel model)
                return model.Info is FileInfo;

            return false;
        }
    }
}