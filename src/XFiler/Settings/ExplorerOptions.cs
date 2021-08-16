using XFiler.SDK;

namespace XFiler
{
    public class ExplorerOptions : IExplorerOptions
    {
        public bool ShowSystemFiles { get; } = false;

        public bool ShowHiddenFiles { get; } = true;
    }
}