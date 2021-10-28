using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

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
    
    ImageSource? Icon { get; }

    IReadOnlyList<IRegistryContextMenuModel>? Children { get; }
}