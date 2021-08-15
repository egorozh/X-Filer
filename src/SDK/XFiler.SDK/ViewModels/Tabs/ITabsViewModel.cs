using System.Collections.Generic;

namespace XFiler.SDK
{
    public interface ITabsViewModel
    {
        void OnOpenNewTab(IFileSystemModel fileEntityViewModel, bool isSelectNewTab = false);
        void OnOpenNewTab(IEnumerable<IFileSystemModel> fileEntityViewModel, bool isSelectNewTab = false);
    }
}