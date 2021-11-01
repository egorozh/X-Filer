using System.Windows.Controls;

namespace XFiler.SDK;

public interface IAddNewContextMenuModel
{
    string Extension { get; }
    string Name { get; }
    byte[]? Data { get; }
    string? Template { get; }
    Image? Icon { get; }
}