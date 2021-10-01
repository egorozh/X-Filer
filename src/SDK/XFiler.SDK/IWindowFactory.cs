using Prism.Commands;
using System.Collections.Generic;

namespace XFiler.SDK;

public interface IWindowFactory
{
    DelegateCommand<object> OpenNewWindowCommand { get; }

    void OpenTabInNewWindow(ITabItemModel tabItem);
    void OpenTabInNewWindow(IEnumerable<ITabItemModel> tabs);

    IMainWindow GetWindowWithRootTab();
}