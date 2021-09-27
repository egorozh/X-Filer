namespace XFiler.SDK
{
    public interface IReactiveOptions
    {
        bool ShowSystemFiles { get; }
        bool ShowHiddenFiles { get; }   

        string DefaultPresenterId { get; }
        bool AlwaysOpenDirectoryInDefaultPresenter { get; }
            
        string? CurrentThemeId { get; }
    }
}