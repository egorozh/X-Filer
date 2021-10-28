using System.Collections.Generic;
using System.Windows.Input;

namespace XFiler.SDK;

public interface INativeContextMenuLoader
{
    ICommand InvokeCommand { get; }

    IReadOnlyList<IRegistryContextMenuModel> CreateMenuItems(IEnumerable<string> selectedItems);

    void DisposeContextMenu();
}

public interface IRegistryContextMenuModel
{
    string Name { get; }

    IReadOnlyList<IRegistryContextMenuModel>? Children { get; }
}