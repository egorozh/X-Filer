namespace XFiler
{
    public class ExplorerOptions : IExplorerOptions
    {
        public bool ShowSystemFiles { get; } = false;

        public bool ShowHiddenFiles { get; } = true;
        
        public string DefaultPresenterId { get; } = "9a5d97b9-628d-45fd-b36f-89936f3c9506"; // default grid presenter

        public bool AlwaysOpenDirectoryInDefaultPresenter { get; } = false;
    }
}