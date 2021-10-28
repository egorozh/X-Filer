using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace XFiler.SDK;

public class NativeContextMenuLoaderReference : Freezable, INativeContextMenuLoader
{
    public static readonly DependencyProperty NativeContextMenuLoaderProperty = DependencyProperty.Register(
        "NativeContextMenuLoader", typeof(INativeContextMenuLoader), typeof(NativeContextMenuLoaderReference),
        new PropertyMetadata(default(INativeContextMenuLoader)));

    public INativeContextMenuLoader NativeContextMenuLoader
    {
        get => (INativeContextMenuLoader) GetValue(NativeContextMenuLoaderProperty);
        set => SetValue(NativeContextMenuLoaderProperty, value);
    }
    
    public ICommand InvokeCommand => NativeContextMenuLoader.InvokeCommand;

    public IReadOnlyList<IRegistryContextMenuModel> CreateMenuItems(IEnumerable<string> selectedItems)
    {
        return NativeContextMenuLoader?.CreateMenuItems(selectedItems) ?? new List<IRegistryContextMenuModel>();
    }

    public void DisposeContextMenu()
    {
        NativeContextMenuLoader?.DisposeContextMenu();
    }

    #region Freezable

    protected override Freezable CreateInstanceCore() => throw new NotImplementedException();

    #endregion
}