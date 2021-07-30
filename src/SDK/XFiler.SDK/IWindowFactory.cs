using Prism.Commands;

namespace XFiler.SDK
{
    public interface IWindowFactory
    {
        DelegateCommand<FileEntityViewModel> OpenNewWindowCommand { get; }
            
        void OpenTabInNewWindow(IExplorerTabItemViewModel tabItem);
    }
}