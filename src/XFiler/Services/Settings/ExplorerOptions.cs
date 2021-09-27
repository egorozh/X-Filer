namespace XFiler
{
    public class ExplorerOptions : IExplorerOptions
    {
        public bool ShowSystemFiles { get; set; } = false;

        public bool ShowHiddenFiles { get; set; } = true;
        
        public string DefaultPresenterId { get; set; } = "9a5d97b9-628d-45fd-b36f-89936f3c9506"; // default grid presenter

        public bool AlwaysOpenDirectoryInDefaultPresenter { get; set; } = false;
    }
}