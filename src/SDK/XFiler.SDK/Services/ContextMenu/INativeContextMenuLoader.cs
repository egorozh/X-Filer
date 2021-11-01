using System.Collections.Generic;
using System.Windows.Input;

namespace XFiler.SDK;

public interface INativeContextMenuLoader
{
    IReadOnlyList<IAddNewContextMenuModel> AddItems { get; }
    ICommand InvokeCommand { get; }
    ICommand InvokeAddNewItemsCommand { get; }

    void Init();

    IReadOnlyList<IRegistryContextMenuModel> CreateMenuItems(IEnumerable<string> selectedItems);
}