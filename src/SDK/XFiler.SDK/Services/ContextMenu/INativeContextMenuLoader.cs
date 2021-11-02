using System.Collections.Generic;
using System.Windows.Input;

namespace XFiler.SDK;

public interface INativeContextMenuLoader : IInitializeService
{
    IReadOnlyList<IAddNewContextMenuModel> AddItems { get; }
    ICommand InvokeCommand { get; }
    ICommand InvokeAddNewItemsCommand { get; }
    
    IReadOnlyList<IRegistryContextMenuModel> CreateMenuItems(IEnumerable<string> selectedItems);
}