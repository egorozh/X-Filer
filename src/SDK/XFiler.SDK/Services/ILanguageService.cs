using System.Globalization;

namespace XFiler.SDK;

public interface ILanguageService
{
    void Init();

    CultureInfo[] Languages { get; }

    CultureInfo Current { get; set; }
}   