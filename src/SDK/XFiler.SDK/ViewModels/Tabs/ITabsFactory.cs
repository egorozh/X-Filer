using System.Collections.Generic;

namespace XFiler.SDK;

public interface ITabsFactory
{
    ITabsViewModel CreateTabsViewModel(IEnumerable<ITabItemModel> initItems);
    ITabsViewModel CreateTabsViewModel();   
}