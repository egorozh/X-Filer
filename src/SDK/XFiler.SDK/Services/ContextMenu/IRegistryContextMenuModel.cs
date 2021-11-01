using System.Collections.Generic;
using System.Windows.Media;

namespace XFiler.SDK;

public interface IRegistryContextMenuModel
{
    string Name { get; }

    ImageSource? Icon { get; }

    IReadOnlyList<IRegistryContextMenuModel>? Children { get; }
}