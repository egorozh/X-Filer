using System.ComponentModel;

namespace XFiler.SDK;

public interface IStartupOptions : INotifyPropertyChanged
{
    public string? CurrentLanguage { get; } 
}