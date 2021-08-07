using Prism.Commands;

namespace XFiler.SDK
{
    public interface IWindowFactory
    {
        DelegateCommand<FileEntityViewModel> OpenNewWindowCommand { get; }

        void OpenTabInNewWindow(ITabItemModel tabItem);

        IXFilerWindow GetWindowWithRootTab();
    }
}