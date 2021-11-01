using System;
using System.Collections;
using Microsoft.Xaml.Behaviors;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace XFiler.SDK;

public sealed class RegistryContextMenuBehavior : Behavior<ContextMenu>
{
    #region Attached Properties

    public static readonly DependencyProperty RootItemProperty = DependencyProperty.RegisterAttached(
        "RootItem", typeof(bool), typeof(RegistryContextMenuBehavior),
        new PropertyMetadata(false));

    public static void SetRootItem(DependencyObject element, bool value)
        => element.SetValue(RootItemProperty, value);

    public static bool GetRootItem(DependencyObject element)
        => (bool) element.GetValue(RootItemProperty);

    #endregion

    #region Dependency Properties

    public static readonly DependencyProperty NativeContextMenuLoaderProperty = DependencyProperty.Register(
        nameof(NativeContextMenuLoader), typeof(INativeContextMenuLoader),
        typeof(RegistryContextMenuBehavior),
        new PropertyMetadata(default(INativeContextMenuLoader)));

    public static readonly DependencyProperty FileInfoModelProperty = DependencyProperty.Register(
        nameof(FileInfoModel), typeof(IFileSystemModel),
        typeof(RegistryContextMenuBehavior),
        new PropertyMetadata(default(IFileSystemModel)));

    public static readonly DependencyProperty SelectedItemsContainerProperty = DependencyProperty.Register(
        nameof(SelectedItemsContainer), typeof(ObjectReference), typeof(RegistryContextMenuBehavior),
        new PropertyMetadata(default(ObjectReference)));
    
    #endregion

    #region Public Properties

    public INativeContextMenuLoader? NativeContextMenuLoader
    {
        get => (INativeContextMenuLoader) GetValue(NativeContextMenuLoaderProperty);
        set => SetValue(NativeContextMenuLoaderProperty, value);
    }

    public IFileSystemModel? FileInfoModel
    {
        get => (IFileSystemModel) GetValue(FileInfoModelProperty);
        set => SetValue(FileInfoModelProperty, value);
    }

    public ObjectReference? SelectedItemsContainer
    {
        get => (ObjectReference) GetValue(SelectedItemsContainerProperty);
        set => SetValue(SelectedItemsContainerProperty, value);
    }

    #endregion

    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject.Opened += AssociatedObjectOnOpened;
    }

    private void AssociatedObjectOnOpened(object sender, RoutedEventArgs e)
    {
        if (NativeContextMenuLoader == null)
            return;

        if (FileInfoModel != null)
        {
            LoadNativeContextMenuItems(NativeContextMenuLoader, new[] {FileInfoModel});
        }
        else if (SelectedItemsContainer != null)
        {
            SelectedItemsContainer.ValueChanged += SelectedItemsContainerOnValueChanged;
        }
    }

    private void SelectedItemsContainerOnValueChanged(object? sender, EventArgs e)
    {
        if (NativeContextMenuLoader == null)
            return;

        if (SelectedItemsContainer is {Value: IEnumerable selectedItems})
        {
            SelectedItemsContainer.ValueChanged -= SelectedItemsContainerOnValueChanged;

            LoadNativeContextMenuItems(NativeContextMenuLoader, selectedItems.OfType<IFileSystemModel>());
        }
    }

    #region Private Methods

    private void LoadNativeContextMenuItems(INativeContextMenuLoader loader, IEnumerable<IFileSystemModel> fileInfo)
    {
        ClearOldItems();

        var rootItem = AssociatedObject.Items.OfType<DependencyObject>()
            .FirstOrDefault(GetRootItem);

        var index = AssociatedObject.Items.Count;

        if (rootItem != null)
            index = AssociatedObject.Items.IndexOf(rootItem) + 1;

        var models = loader.CreateMenuItems(fileInfo.Select(f => f.Info.FullName));

        foreach (var model in models)
            AssociatedObject.Items.Insert(index++, CreateRegistryMenuItem(loader, model));
    }

    private void ClearOldItems()
    {
        if (AssociatedObject.Items is { } items)
        {
            var oldItems = items.OfType<MenuItem>()
                .Where(i => i.DataContext is IRegistryContextMenuModel)
                .ToList();

            foreach (var oldItem in oldItems)
                AssociatedObject.Items.Remove(oldItem);
        }
    }

    private static MenuItem CreateRegistryMenuItem(INativeContextMenuLoader loader, IRegistryContextMenuModel model)
    {
        List<MenuItem>? subItems = null;
        Image? icon = null;

        if (model.Children != null)
        {
            subItems = model.Children
                .Select(child => CreateRegistryMenuItem(loader, child))
                .ToList();
        }

        if (model.Icon != null)
        {
            icon = new Image
            {
                Source = model.Icon
            };
        }

        MenuItem item = new()
        {
            Header = model.Name,
            DataContext = model,
            Command = loader.InvokeCommand,
            CommandParameter = model,
            ItemsSource = subItems,
            Icon = icon,
        };

        return item;
    }

    #endregion
}