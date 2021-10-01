using System;
using System.Globalization;
using XFiler.SDK.Localization;

namespace XFiler.SDK;

public class FileSizeToStringConverter : BaseValueConverter
{
    private static readonly string[] Sizes;

    static FileSizeToStringConverter()
    {
        Sizes = new[] { Strings.B, Strings.KB, Strings.MB, Strings.Gb, Strings.TB };
    }

    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double len = (long)value;

        var order = 0;
         
        while (len >= 1024 && order < Sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }

        return $"{len:0.##} {Sizes[order]}";
    }
}