using System.ComponentModel;

namespace XFiler.SDK;

public interface IReactiveOptions : INotifyPropertyChanged
{
    bool ShowSystemFiles { get; }
    bool ShowHiddenFiles { get; }

    string DefaultPresenterId { get; }
    bool AlwaysOpenDirectoryInDefaultPresenter { get; }

    string? CurrentThemeId { get; }

    string? ExplorerBackgroundImagePath { get; }
        
    bool LaunchAtStartup { get; }
}   