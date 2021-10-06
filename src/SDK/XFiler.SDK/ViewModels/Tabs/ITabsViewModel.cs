using System.Collections.Generic;

namespace XFiler.SDK;

public interface ITabsViewModel
{
    void Init(IEnumerable<ITabItemModel> initTabs);

    void OnOpenNewTab(IFileSystemModel fileEntityViewModel, bool isSelectNewTab = false);
    void OnOpenNewTab(IEnumerable<IFileSystemModel> fileEntityViewModel, bool isSelectNewTab = false);
}