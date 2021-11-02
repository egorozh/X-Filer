using System.Globalization;

namespace XFiler.SDK;

public interface ILanguageService : IInitializeService
{
    CultureInfo[] Languages { get; }

    CultureInfo Current { get; set; }
}