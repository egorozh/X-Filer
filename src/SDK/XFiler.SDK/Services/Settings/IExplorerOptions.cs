namespace XFiler.SDK
{
    public interface IExplorerOptions
    {
        bool ShowSystemFiles { get; }
        bool ShowHiddenFiles { get; }

        string DefaultPresenterId { get; }
        bool AlwaysOpenDirectoryInDefaultPresenter { get; }
    }
}